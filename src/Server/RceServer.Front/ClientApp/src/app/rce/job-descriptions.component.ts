import { Component, Input, Output, EventEmitter, Inject, OnInit } from '@angular/core';
import { JobDescription } from '../shared/rce-message-intefaces';
import { JQ_TOKEN } from '../shared/jquery.service';

@Component({
  selector: 'app-job-descriptions',
  templateUrl: './job-descriptions.component.html',
  styleUrls: ['./job-descriptions.component.css']
})
export class JobDescriptionsComponent implements OnInit {
  @Input() jobDescriptions: JobDescription[];
  @Input() workerId: string;
  @Output() redrawMasonryRequested: EventEmitter<any> = new EventEmitter();

  constructor(@Inject(JQ_TOKEN) private $: any) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.$('#jobDescriptions-' + this.workerId).on('shown.bs.collapse', () => this.redrawMasonryRequested.emit(null));
      this.$('#jobDescriptions-' + this.workerId).on('hidden.bs.collapse', () => this.redrawMasonryRequested.emit(null));
    }, 1);
  }
}
