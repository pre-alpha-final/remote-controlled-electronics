import { Component, Input, Output, EventEmitter, Inject, OnInit } from '@angular/core';
import { Job } from '../shared/job';
import { JQ_TOKEN } from '../shared/jquery.service';

@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html',
  styleUrls: ['./jobs.component.css']
})
export class JobsComponent implements OnInit {
  @Input() jobs: Job[];
  @Input() workerId: string;
  @Output() redrawMasonryRequested: EventEmitter<any> = new EventEmitter();

  private modalVarRemoveJob: Job;

  constructor(@Inject(JQ_TOKEN) private $: any) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.$('#jobs-' + this.workerId).on('shown.bs.collapse', () => this.redrawMasonryRequested.emit(null));
      this.$('#jobs-' + this.workerId).on('hidden.bs.collapse', () => this.redrawMasonryRequested.emit(null));
    }, 1);
  }

  jobClose($event: MouseEvent) {
    // todo send remove job to server
  }

  private setModalVarRemoveJob(job: Job) {
    this.modalVarRemoveJob = job;
  }
}
