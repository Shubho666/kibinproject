import { Component, OnInit } from '@angular/core';
import { IdeazoneDataSplitService } from 'src/app/services/ideazone-data-split.service';
import { DashboardService } from 'src/app/services/dashboard.service';
import { AuthService } from 'src/app/services/auth.service';
@Component({
  selector: 'app-privatespace',
  templateUrl: './privatespace.component.html',
  styleUrls: ['./privatespace.component.css']
})
export class PrivatespaceComponent implements OnInit {
  requestsView;
  ProjectId = localStorage.getItem('ideazone_projectid');
  UserId;
  ProjectName;

  constructor(private dashBoardService: DashboardService , private ds: IdeazoneDataSplitService,
              private auth: AuthService) { }

  ngOnInit() {
    const userdetails = this.auth.jwtToken();
    this.UserId = userdetails.decodedToken.id;
    this.dashBoardService.getProjectByOwner(this.UserId).subscribe(x => {
   // tslint:disable-next-line: triple-equals
   const index = x.findIndex( y => y.id == this.ProjectId );
   this.ProjectName = x[index].projectName;
  });
    this.requestsView = this.ds.getView();
}
changeView(view) {
  this.ds.changeView(view);
}
}
