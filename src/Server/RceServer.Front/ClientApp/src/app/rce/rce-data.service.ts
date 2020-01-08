import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { AuthService } from '../auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { JobDescription, RceMessage, WorkerAddedMessage, JobAddedMessage, JobRemovedMessage, JobPickedUpMessage,
  JobUpdatedMessage, JobCompletedMessage, Statuses, WorkerRemovedMessage } from '../shared/rce-message-intefaces';
import { Job, JobStates } from '../shared/job';

enum MessageTypes {
  JobAddedMessage = 'JobAddedMessage',
  JobCompletedMessage = 'JobCompletedMessage',
  JobPickedUpMessage = 'JobPickedUpMessage',
  JobRemovedMessage = 'JobRemovedMessage',
  JobUpdatedMessage = 'JobUpdatedMessage',
  KeepAliveSentMessage = 'KeepAliveSentMessage',
  WorkerAddedMessage = 'WorkerAddedMessage',
  WorkerRemovedMessage = 'WorkerRemovedMessage'
}

enum ConnectionStatuses {
  Undefined,
  ClosedByWorker,
  ClosedByServer,
  ConnectionLost
}

class Worker {
  workerId: string;
  name: string;
  description: string;
  base64Logo: string;
  jobDescriptions: JobDescription[];
  jobs: Job[];
  error: string;
}

@Injectable({
  providedIn: 'root'
})
export class RceDataService {
  private readonly KeepAlive = 15000;
  private running = false;
  private connection: HubConnection;
  private messages: RceMessage[] = [];

  workers: Worker[] = [];

  constructor(private authService: AuthService, private httpClient: HttpClient) {
    this.connection = new HubConnectionBuilder()
      .withUrl('/rce', { accessTokenFactory: () => this.authService.accessToken })
      .build();
    this.connection.keepAliveIntervalInMilliseconds = this.KeepAlive;
    this.connection.on('messageReceived', e => this.messages.push(e));
    this.connection.onclose(() => {
      if (this.running) {
        this.reconnectToRceServer(this.connection);
      }
    });
  }

  connect(): void {
    this.connectToRceServer(this.connection);
  }

  private connectToRceServer(connection: HubConnection): void {
    this.running = false;
    this.workers = [];
    this.connection.stop().then(() => {
      connection.start()
        .then(() => {
          this.running = true;
          this.httpClient.get<RceMessage[]>('/api/server')
            .subscribe(e => {
              this.messages.unshift(...e);
              this.runMessageProcessor();
            });
        })
        .catch(e => console.error('Connection error: ' + e.message));
    });
  }

  private reconnectToRceServer(connection: HubConnection): void {
    setTimeout(() => this.connectToRceServer(connection), 3000);
  }

  private runMessageProcessor(): void {
    if (this.running === false) {
      return;
    }

    const message = this.messages.shift();
    if (message != null) {
      this.processMessage(message);
    }

    setTimeout(() => this.runMessageProcessor(), this.messages.length ? 0 : 100);
  }

  private processMessage(message: RceMessage): void {
    switch (message.messageType) {
      case MessageTypes.WorkerAddedMessage: {
        const workerAddedMessage = (message as WorkerAddedMessage);
        if (this.workers.find(e => e.workerId === workerAddedMessage.workerId)) {
          break;
        }
        this.workers.push(<Worker>{
          workerId: workerAddedMessage.workerId,
          name: workerAddedMessage.name,
          description: workerAddedMessage.description,
          base64Logo: workerAddedMessage.base64Logo,
          jobDescriptions: workerAddedMessage.jobDescriptions,
          jobs: []
        });
        break;
      }

      case MessageTypes.WorkerRemovedMessage: {
        const workerRemovedMessage = (message as WorkerRemovedMessage);
        const worker = this.workers.find(e => e.workerId === workerRemovedMessage.workerId);
        if (worker == null) {
          break;
        }
        worker.error = ConnectionStatuses[workerRemovedMessage.connectionStatus];
        break;
      }

      case MessageTypes.JobAddedMessage: {
        const jobAddedMessage = (message as JobAddedMessage);
        const parentWorker = this.workers.find(e => e.workerId === jobAddedMessage.workerId);
        if (parentWorker == null || parentWorker.jobs.find(e => e.jobId === jobAddedMessage.jobId)) {
          break;
        }
        parentWorker.jobs.push(<Job>{
          jobId: jobAddedMessage.jobId,
          workerId: jobAddedMessage.workerId,
          name: jobAddedMessage.name,
          payload: jobAddedMessage.payload,
          jobState: JobStates.Added,
        });
        break;
      }

      case MessageTypes.JobRemovedMessage: {
        const jobRemovedMessage = (message as JobRemovedMessage);
        const parentWorker = this.workers.find(e => e.workerId === jobRemovedMessage.workerId);
        if (parentWorker == null) {
          break;
        }
        parentWorker.jobs = parentWorker.jobs.filter(e => e.jobId !== jobRemovedMessage.jobId);
        break;
      }

      case MessageTypes.JobPickedUpMessage: {
        const jobPickedUpMessage = (message as JobPickedUpMessage);
        const parentWorker = this.workers.find(e => e.workerId === jobPickedUpMessage.workerId);
        if (parentWorker == null) {
          break;
        }
        const job = parentWorker.jobs.find(e => e.jobId === jobPickedUpMessage.jobId);
        if (job) {
          job.jobState = JobStates.PickedUp;
        }
        break;
      }

      case MessageTypes.JobUpdatedMessage: {
        const jobUpdatedMessage = (message as JobUpdatedMessage);
        const parentWorker = this.workers.find(e => e.workerId === jobUpdatedMessage.workerId);
        if (parentWorker == null) {
          break;
        }
        const job = parentWorker.jobs.find(e => e.jobId === jobUpdatedMessage.jobId);
        if (job) {
          job.output = jobUpdatedMessage.output;
          job.jobState = JobStates.Updated;
        }
        break;
      }

      case MessageTypes.JobCompletedMessage: {
        const jobCompletedMessage = (message as JobCompletedMessage);
        const parentWorker = this.workers.find(e => e.workerId === jobCompletedMessage.workerId);
        if (parentWorker == null) {
          break;
        }
        const job = parentWorker.jobs.find(e => e.jobId === jobCompletedMessage.jobId);
        if (job) {
          job.output = jobCompletedMessage.output;
          if (jobCompletedMessage.jobStatus === Statuses.Success) {
            job.jobState = JobStates.Success;
          }
          if (jobCompletedMessage.jobStatus === Statuses.Warning || jobCompletedMessage.jobStatus === Statuses.Undefined) {
            job.jobState = JobStates.Warning;
          }
          if (jobCompletedMessage.jobStatus === Statuses.Failure) {
            job.jobState = JobStates.Failure;
          }
        }
        break;
      }

      case MessageTypes.JobRemovedMessage: {
        const jobRemovedMessage = (message as JobRemovedMessage);
        const parentWorker = this.workers.find(e => e.workerId === jobRemovedMessage.workerId);
        if (parentWorker == null) {
          break;
        }
        parentWorker.jobs = parentWorker.jobs.filter(e => e.jobId !== jobRemovedMessage.jobId);
        break;
      }
    }
  }
}
