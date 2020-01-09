import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkersComponent } from './workers.component';
import { RceRoutingModule } from './rce-routing.module';
import { JobsComponent } from './jobs.component';
import { JobDescriptionsComponent } from './job-descriptions.component';

@NgModule({
  declarations: [
    WorkersComponent,
    JobsComponent,
    JobDescriptionsComponent],
  imports: [
    CommonModule,
    RceRoutingModule
  ]
})
export class RceModule { }
