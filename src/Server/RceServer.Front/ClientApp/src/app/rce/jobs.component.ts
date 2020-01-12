import { Component, Input } from '@angular/core';
import { Job } from '../shared/job';

@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html',
  styleUrls: ['./jobs.component.css']
})
export class JobsComponent {
  @Input() jobs: Job[];
  @Input() workerId: string;

  jobClose($event: MouseEvent, job: Job) {
    // todo send remove job to server
  }
}
