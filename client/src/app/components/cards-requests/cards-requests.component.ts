import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ApiCallsService } from 'src/app/services/api-calls.service';

@Component({
  selector: 'app-cards-requests',
  templateUrl: './cards-requests.component.html',
  styleUrls: ['./cards-requests.component.css']
})
export class CardsRequestsComponent {
  @Input() requests;
  @Input() calledBy;
  @Input() Role;
  @Output() messageEvent3 = new EventEmitter<{}>();
  constructor(private api: ApiCallsService) { }
  changeStatus(Epic, i, Status) {
    this.api.changestatusofepic(Epic.EpicId, Status).subscribe();
    this.requests.splice(i, 1);
    this.messageEvent3.emit({ epic: Epic, status: Status, requests: this.requests.length });
  }
}
