import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { IdeazoneDataSplitService } from 'src/app/services/ideazone-data-split.service';
import {  ApiCallsService } from 'src/app/services/api-calls.service';
import { AuthService } from 'src/app/services/auth.service';
import { DashboardService } from 'src/app/services/dashboard.service';
import * as signalR from '@aspnet/signalr';
import {environment} from '../../../environments/environment';
import { MatBottomSheetRef, MatBottomSheet } from '@angular/material';
// var connection;
@Component({
 selector: 'app-ideazone',
 templateUrl: './ideazone.component.html',
 styleUrls: ['./ideazone.component.css']
})
export class IdeazoneComponent implements OnInit {
// epics: any[];
// epicsP: any[] = [];
// newUserStory = {};
requestsView;
ProjectId;
ProjectName;
UserId;
returned;
Role;
            //
 constructor(private dashBoardService: DashboardService , private ds: IdeazoneDataSplitService ,
             private api: ApiCallsService , private auth: AuthService, private bottomSheet: MatBottomSheet) { }
 ngOnInit() {
    // connection = new signalR.HubConnectionBuilder()
    //   .configureLogging(signalR.LogLevel.Information)
    //   .withUrl(environment.starturlconnection + "/notify", {
    //     skipNegotiation: true,
    //     transport: signalR.HttpTransportType.WebSockets
    //   })
    //   .build();
    // connection
    //   .start()
    //   .then(function () {
    //     console.log("Connected!", connection);
    //   })
    //   .catch(function (err) {
    //     return console.error(err.toString());
    //   });
     const userdetails = this.auth.jwtToken();
     this.UserId = userdetails.decodedToken.id;
     this.dashBoardService.getProjectByOwner(this.UserId).subscribe(x => {
    // tslint:disable-next-line: triple-equals
    const index = x.findIndex( y => y.id == this.ProjectId );
    this.ProjectName = x[index].projectName;
    });
     localStorage.setItem('ideazone_userid', this.UserId);
     console.log(this.UserId);
     this.api.setUserId(this.UserId);
     this.Role = localStorage.getItem('Role');
     console.log(this.Role);
     this.ProjectId = this.api.getProjectId();
     console.log(this.ProjectId);
     this.api.initideazone();
     this.requestsView = this.ds.getView();
    //  setTimeout(() => {
    //     connection
    //       .invoke("JoinGroup", this.ProjectId)
    //       .then(res => console.log("Joined Group", this.ProjectId))
    //       .catch(err => {
    //         console.log("Error", err);
    //       });
    //   }, 1000);
}
// Rabbitcheck(){
//     connection.invoke("SendUserStoryToAddOnList",this.ProjectId,"success");
// }
openBottomSheet(): void {
    this.bottomSheet.open(IdeazoneActivity);
  }
changeView(view) {
this.ds.changeView(view);
}
}
@Component({
    selector: 'app-ideazone-activity',
    templateUrl: 'ideazone-activity.html',
  })
  // tslint:disable-next-line: component-class-suffix
  export class IdeazoneActivity implements OnInit {
    constructor(private bottomSheetRef: MatBottomSheetRef<IdeazoneActivity>, private ideaService: ApiCallsService) {}
    activity;
    activityreverse;
    Activities  = [];
    ngOnInit(): void {
      this.ideaService.getActivity().subscribe(x => {
        this.activity = x;
        // console.log(this.activity);
        this.activityreverse = this.activity.reverse();
        // tslint:disable-next-line: forin
        for (const i in this.activityreverse) {
          // console.log(new Date(this.activityreverse[i].published));
          this.Activities.push(
            {
            description : this.activityreverse[i].description,
            published : (new Date(this.activityreverse[i].published)).toString()
            }
          );
        }
        console.log(this.activityreverse);
      });
    }
    openLink(event: MouseEvent): void {
      this.bottomSheetRef.dismiss();
      event.preventDefault();
    }
  }
