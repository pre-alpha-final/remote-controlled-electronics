import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { RceDataService } from './rce-data.service';
import { Subscription } from 'rxjs';
import { Job } from '../shared/job';

@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html'
})
export class JobsComponent implements OnInit, OnDestroy {
  @Input() parentWorkerId: string;

  private jobsSubscription: Subscription;

  workerJobs: Job[] = [];

  constructor(public rceDataService: RceDataService) { }

  ngOnInit() {
    this.jobsSubscription = this.rceDataService.jobs$.subscribe(e => this.workerJobs = e.filter(f => f.workerId === this.parentWorkerId));
  }

  ngOnDestroy(): void {
    this.jobsSubscription.unsubscribe();
  }
}
