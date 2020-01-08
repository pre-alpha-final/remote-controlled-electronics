import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WorkersComponent } from './workers.component';
import { RceRoutingModule } from './rce-routing.module';
import { JobsComponent } from './jobs.component';

@NgModule({
  declarations: [
    WorkersComponent,
    JobsComponent],
  imports: [
    CommonModule,
    RceRoutingModule
  ]
})
export class RceModule { }
