import { Component, Input, Output, EventEmitter, Inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Job } from '../shared/job';
import { JQ_TOKEN } from '../shared/jquery.service';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html',
  styleUrls: ['./jobs.component.css']
})
export class JobsComponent implements OnInit {
  @Input() jobs: Job[];
  @Input() workerId: string;
  @Output() redrawMasonryRequested: EventEmitter<any> = new EventEmitter();

  @ViewChild('jobId') jobIdRef: ElementRef;

  constructor(@Inject(JQ_TOKEN) private $: any, private httpClient: HttpClient, private authService: AuthService) { }

  ngOnInit(): void {
    setTimeout(() => {
      this.$('#jobs-' + this.workerId).on('shown.bs.collapse', () => this.redrawMasonryRequested.emit(null));
      this.$('#jobs-' + this.workerId).on('hidden.bs.collapse', () => this.redrawMasonryRequested.emit(null));
    }, 1);
  }

  private removeJob(): void {
    const jobId = this.jobIdRef.nativeElement.value;

    this.httpClient.post<void>('api/server/workers/' + this.workerId + '/jobs/' + jobId + '/remove', {})
      .pipe(catchError(e => EMPTY))
      .subscribe();
  }

  private fillModal(job: Job): void {
    this.jobIdRef.nativeElement.value = job.jobId;
  }
}
