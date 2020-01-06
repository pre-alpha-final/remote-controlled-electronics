import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import { Job } from '../shared/job';
import { AuthService } from '../auth/auth.service';

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

@Injectable({
  providedIn: 'root'
})
export class RceDataService {
  i = 0;
  private KeepAlive = 15000;
  private jobs: Job[] = [];

  workers: Worker[] = [];
  jobs$: BehaviorSubject<Job[]> = new BehaviorSubject(this.jobs);

  constructor(private authService: AuthService) {
    this.foo();
    const connection: HubConnection = new HubConnectionBuilder()
      .withUrl('/rce', { accessTokenFactory: () => authService.accessToken })
      .build();
    connection.keepAliveIntervalInMilliseconds = this.KeepAlive;

    this.connectSignalR(connection);

    connection.on('foo', e => console.log(e));

    connection.onclose(() => this.reconnectSignalR(connection));
  }

  foo(): void {
    // tslint:disable-next-line: max-line-length
    this.workers = [...this.workers, { 'workerId': (this.i++).toString(), 'name': (this.i).toString(), 'description': (this.i).toString(), 'base64Logo': (this.i).toString(), 'jobDescriptions': [] }];
    // tslint:disable-next-line: max-line-length
    this.jobs = [...this.jobs, { 'jobId': (this.i + 1).toString(), workerId: (this.i + 1).toString(), name: (this.i + 1).toString(), payload: null }];
    this.jobs$.next(this.jobs);
    setTimeout(() => this.foo(), 3000);
  }

  private connectSignalR(connection: HubConnection): void {
    connection.start()
      .then(() => console.log('Connected'))
      .catch(e => {
        console.error('Connection error: ' + e.message);
        this.reconnectSignalR(connection);
      });
  }

  private reconnectSignalR(connection: HubConnection): void {
    setTimeout(() => this.connectSignalR(connection), 3000);
  }
}
