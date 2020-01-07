import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { JobDescription, RceMessage, WorkerAddedMessage, JobAddedMessage, JobRemovedMessage } from '../shared/rce-message-intefaces';
import { Job } from '../shared/job';

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

class Worker {
  workerId: string;
  name: string;
  description: string;
  base64Logo: string;
  jobDescriptions: JobDescription[];
}

@Injectable({
  providedIn: 'root'
})
export class RceDataService {
  private readonly KeepAlive = 15000;
  private running = false;
  private connection: HubConnection;
  private messages: RceMessage[] = [];
  private jobs: Job[] = [];

  workers: Worker[] = [];
  jobs$: BehaviorSubject<Job[]> = new BehaviorSubject(this.jobs);

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
        if (this.workers.some(e => e.workerId === workerAddedMessage.workerId)) {
          break;
        }
        this.workers.push(<Worker>{
          workerId: workerAddedMessage.workerId,
          name: workerAddedMessage.name,
          description: workerAddedMessage.description,
          base64Logo: workerAddedMessage.base64Logo,
          jobDescriptions: workerAddedMessage.jobDescriptions
        });
        break;
      }

      case MessageTypes.WorkerRemovedMessage: {
        break;
      }

      case MessageTypes.JobAddedMessage: {
        const jobAddedMessage = (message as JobAddedMessage);
        if (this.jobs.some(e => e.jobId === jobAddedMessage.jobId)) {
          break;
        }
        this.jobs.push(<Job> {
          jobId: jobAddedMessage.jobId,
          workerId: jobAddedMessage.workerId,
          name: jobAddedMessage.name,
          payload: jobAddedMessage.payload,
        });
        this.jobs$.next(this.jobs);
        break;
      }

      case MessageTypes.JobRemovedMessage: {
        const jobRemovedMessage = (message as JobRemovedMessage);
        this.jobs = this.jobs.filter(e => e.jobId !== jobRemovedMessage.jobId);
        this.jobs$.next(this.jobs);
        break;
      }
    }
  }
}
