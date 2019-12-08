import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import { Job } from '../shared/job';

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
  private jobs: Job[] = [];

  workers: Worker[] = [];
  jobs$: BehaviorSubject<Job[]> = new BehaviorSubject(this.jobs);

  constructor() {
    this.foo();
    const connection: HubConnection = new HubConnectionBuilder()
      .withUrl('/rce')
      .build();
    connection.start().then(function () {
      console.log('connected');
    })
      .catch(function (error) {
        console.error(error.message);
      });
    connection.on('foo', e => console.log(e));
  }

  foo(): void {
    // tslint:disable-next-line: max-line-length
    this.workers = [...this.workers, { 'workerId': (this.i++).toString(), 'name': (this.i).toString(), 'description': (this.i).toString(), 'base64Logo': (this.i).toString(), 'jobDescriptions': [] }];
    // tslint:disable-next-line: max-line-length
    this.jobs = [...this.jobs, { 'jobId': (this.i + 1).toString(), workerId: (this.i + 1).toString(), name: (this.i + 1).toString(), payload: null }];
    this.jobs$.next(this.jobs);
    setTimeout(() => this.foo(), 3000);
  }
}
