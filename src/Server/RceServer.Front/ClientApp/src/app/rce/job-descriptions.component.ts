import { Component, Input } from '@angular/core';
import { JobDescription } from '../shared/rce-message-intefaces';

@Component({
  selector: 'app-job-descriptions',
  templateUrl: './job-descriptions.component.html',
  styleUrls: ['./job-descriptions.component.css']
})
export class JobDescriptionsComponent {
  @Input() jobDescriptions: JobDescription[];
  @Input() workerId: string;

  constructor() { }
}
