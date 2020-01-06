import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import { Job } from '../shared/job';
import { AuthService } from '../auth/auth.service';
import { HttpClient } from '@angular/common/http';

interface JobDescription {
  name: string;
  description: string[];
  defaultPayload: any;
}

interface Worker {
  workerId: string;
  name: string;
  description: string;
  base64Logo: string;
  jobDescriptions: JobDescription[];
}

interface RceMessage {
  messageId: string;
  messageTimestamp: number;
  messageType: string;
}

@Injectable({
  providedIn: 'root'
})
export class RceDataService {
  private KeepAlive = 15000;
  private messages: RceMessage[] = [];
  private jobs: Job[] = [];

  workers: Worker[] = [];
  jobs$: BehaviorSubject<Job[]> = new BehaviorSubject(this.jobs);

  constructor(private authService: AuthService, private httpClient: HttpClient) {
    const connection: HubConnection = new HubConnectionBuilder()
      .withUrl('/rce', { accessTokenFactory: () => this.authService.accessToken })
      .build();
    connection.keepAliveIntervalInMilliseconds = this.KeepAlive;
    connection.on('messageReceived', e => this.messages.push(e));
    connection.onclose(() => this.reconnectSignalR(connection));
    this.connectSignalR(connection);
  }

  private connectSignalR(connection: HubConnection): void {
    connection.start()
      .then(() => {
        this.httpClient.get<RceMessage[]>('/api/server')
          .subscribe(e => {
            this.messages.unshift(...e);
            this.runMessageProcessor();
          });
      })
      .catch(e => {
        console.error('Connection error: ' + e.message);
        this.reconnectSignalR(connection);
      });
  }

  private reconnectSignalR(connection: HubConnection): void {
    setTimeout(() => this.connectSignalR(connection), 3000);
  }

  private runMessageProcessor(): void {
    const message = this.messages.shift();
    if (message != null) {
      this.processMessage(message);
    }

    setTimeout(() => this.runMessageProcessor(), this.messages.length ? 0 : 100);
  }

  private processMessage(message: RceMessage): void {
    console.log(message.messageType);
  }
}
