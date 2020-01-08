import { Component, Input } from '@angular/core';
import { Job } from '../shared/job';

@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html'
})
export class JobsComponent {
  @Input() jobs: Job[];
}
