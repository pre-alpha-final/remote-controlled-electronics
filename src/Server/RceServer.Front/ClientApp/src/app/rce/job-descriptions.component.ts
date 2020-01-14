import { Component, Input, Output, EventEmitter, Inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { JobDescription } from '../shared/rce-message-intefaces';
import { JQ_TOKEN } from '../shared/jquery.service';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-job-descriptions',
  templateUrl: './job-descriptions.component.html',
  styleUrls: ['./job-descriptions.component.css']
})
export class JobDescriptionsComponent implements OnInit {
  @Input() jobDescriptions: JobDescription[];
  @Input() workerId: string;
  @Output() redrawMasonryRequested: EventEmitter<any> = new EventEmitter();

  @ViewChild('jobName') jobNameRef: ElementRef;
  @ViewChild('jobTextArea') jobTextAreaRef: ElementRef;

  constructor(@Inject(JQ_TOKEN) private $: any, private jsonPipe: JsonPipe) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.$('#jobDescriptions-' + this.workerId).on('shown.bs.collapse', () => this.redrawMasonryRequested.emit(null));
      this.$('#jobDescriptions-' + this.workerId).on('hidden.bs.collapse', () => this.redrawMasonryRequested.emit(null));
    }, 1);
  }

  runJob(): void {
    // todo run job
  }

  private fillModal(jobDescription: JobDescription): void {
    this.jobNameRef.nativeElement.innerText = jobDescription.name;
    this.jobTextAreaRef.nativeElement.value = this.jsonPipe.transform(jobDescription.defaultPayload);
  }
}
