import { Component, OnInit } from '@angular/core';
import { RceDataService } from './rce-data.service';

@Component({
  templateUrl: './workers.component.html'
})
export class WorkersComponent implements OnInit {

  constructor(public rceDataService: RceDataService) { }

  ngOnInit() {
    this.rceDataService.connect();
  }
}
