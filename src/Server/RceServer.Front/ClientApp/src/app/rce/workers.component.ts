import { Component, OnInit } from '@angular/core';
import { RceDataService } from './rce-data.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  templateUrl: './workers.component.html',
  styleUrls: ['./workers.component.css']
})
export class WorkersComponent implements OnInit {
  updateMasonryLayout: boolean;

  constructor(public rceDataService: RceDataService, private domSanitizer: DomSanitizer) { }

  ngOnInit() {
    this.rceDataService.connect();
  }

  getBase64Image(base64Image: string): SafeResourceUrl {
    return this.domSanitizer.bypassSecurityTrustResourceUrl(base64Image);
  }

  redrawMasonry(): void {
    this.updateMasonryLayout = !this.updateMasonryLayout;
  }
}
