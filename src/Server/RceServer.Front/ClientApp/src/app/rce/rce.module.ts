import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkersComponent } from './workers.component';
import { RceRoutingModule } from './rce-routing.module';
import { JobsComponent } from './jobs.component';
import { JobDescriptionsComponent } from './job-descriptions.component';
import { NgxMasonryModule } from 'ngx-masonry';

@NgModule({
  declarations: [
    WorkersComponent,
    JobsComponent,
    JobDescriptionsComponent],
  imports: [
    CommonModule,
    RceRoutingModule,
    NgxMasonryModule,
  ]
})
export class RceModule { }
