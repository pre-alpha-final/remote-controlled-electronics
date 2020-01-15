import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../auth/auth-guard.service';
import { WorkersComponent } from './workers.component';
import { JsonPipe } from '@angular/common';

const routes: Routes = [
  {
    path: '',
    component: WorkersComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
    runGuardsAndResolvers: 'always'
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [JsonPipe]
})
export class RceRoutingModule { }
