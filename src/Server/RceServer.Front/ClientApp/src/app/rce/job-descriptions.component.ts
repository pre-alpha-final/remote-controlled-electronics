import { Component, Input, Output, EventEmitter, Inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { JobDescription } from '../shared/rce-message-intefaces';
import { JQ_TOKEN } from '../shared/jquery.service';
import { JsonPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { AuthService } from '../auth/auth.service';

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

  constructor(@Inject(JQ_TOKEN) private $: any, private jsonPipe: JsonPipe,
    private httpClient: HttpClient, private authService: AuthService) { }

  ngOnInit(): void {
    setTimeout(() => {
      this.$('#jobDescriptions-' + this.workerId).on('shown.bs.collapse', () => this.redrawMasonryRequested.emit(null));
      this.$('#jobDescriptions-' + this.workerId).on('hidden.bs.collapse', () => this.redrawMasonryRequested.emit(null));
    }, 1);
  }

  fillModal(jobDescription: JobDescription): void {
    this.jobNameRef.nativeElement.innerText = jobDescription.name;
    this.jobTextAreaRef.nativeElement.value = this.jsonPipe.transform(jobDescription.defaultPayload);
  }

  runJob(): void {
    const jobName = this.jobNameRef.nativeElement.innerText;
    const jobPayload = this.jobTextAreaRef.nativeElement.value;

    this.httpClient.post<void>('/api/server/workers/' + this.workerId + '/runjob', { jobName, jobPayload })
      .pipe(catchError(e => EMPTY))
      .subscribe();
  }

  runJobWithDefaults(jobDescription: JobDescription): void {
    const jobName = jobDescription.name;
    const jobPayload = this.jsonPipe.transform(jobDescription.defaultPayload);

    this.httpClient.post<void>('/api/server/workers/' + this.workerId + '/runjob', { jobName, jobPayload })
      .pipe(catchError(e => EMPTY))
      .subscribe();
  }
}
