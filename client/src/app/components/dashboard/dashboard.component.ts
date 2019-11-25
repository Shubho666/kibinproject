import * as signalR from '@aspnet/signalr';
import { ActivatedRoute } from '@angular/router';
import { ApiCallsService } from 'src/app/services/api-calls.service';
import { AuthService } from '../../services/auth.service';
import { BoardService } from 'src/app/services/board.service';
import { Component, Inject, OnInit, DoCheck } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { environment } from '../../../environments/environment';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import {MatSnackBar} from '@angular/material/snack-bar';



//var this.token.decodedToken.id;
export interface DialogData {
  project_name: string;
  project_description: string;
  project_startdate;
  project_enddate;


}
let connection;
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  constructor(
    private dashboardService: DashboardService,
    public dialog: MatDialog,
    private router: Router,
    private boardService: BoardService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private idzoneapi: ApiCallsService,
    private _snackBar: MatSnackBar
  ) { }
  // connection;
  project_name;
  project_description;
  project_startdate;
  project_enddate;
  newproject;
  all_projects;
  logged_in_user;

  token;
  // dashboardid;


  
  
  ngOnInit() {
    
    this.token = this.authService.jwtToken();
    this.dashboardService.getProjectByOwner(this.token.decodedToken.id).subscribe(x => {
      this.all_projects = x;
      console.log(this.all_projects);
    });

    this.dashboardService.getUserById(this.token.decodedToken.id).subscribe(x => {
      this.logged_in_user = x;
      console.log(this.logged_in_user);
    });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(ProjectDialog, {
      width: '400px',
      data: {
        project_name: this.project_name,
        project_description: this.project_description,
        project_startdate: this.project_startdate,
        project_enddate: this.project_enddate
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.newproject = result;
      console.log(this.newproject);
      // if(this.newproject !== undefined){
      //   localStorage.setItem('Role','owner');
      // }
    
      let post_new_project = {
        projectName: this.newproject.project_name,
        projectType: null,
        projectDescription: this.newproject.project_description,
        startTime: this.newproject.project_startdate,
        endTime: this.newproject.project_enddate,
        owner: [this.token.decodedToken.id],
        members: []
      };


      this.dashboardService.postProject(post_new_project).subscribe(x => {
        console.log(x);
        this.all_projects.push(x);

        const list = {
          name: 'Product Backlog',
          userStory: [],
          projectId: x.id,
          index: 0
        };
        this.boardService.postColumn(list).subscribe();
        // connection.invoke('SendAllProject', dashboardid, this.all_projects);
        console.log(this.all_projects);

        console.log(this.logged_in_user);
        // this.dashboardService.getUserById(this.token.decodedToken.id).subscribe(y=>{
        //   logged_in_user=y;
        //   let project={
        //     projectId:x.id,
        //     role:"Owner"
        //   };
        //   logged_in_user.projectDetails.push(project);
        //   this.dashboardService.putUser(logged_in_user).subscribe(z=>"put project");
        // })
        const project = {
          projectId: x.id,
          role: 'Owner'
        };
        this.logged_in_user.projectDetails.push(project);
        this.dashboardService.putUser(this.logged_in_user).subscribe(z => 'put project');

      });
    });
  }
  deleteProject(to_delete_project) {
    if(confirm("Are you sure you want to delete the project?")){
    let i = 0;
    let j = 0;
    const localProjectDetails = JSON.parse(JSON.stringify(this.all_projects));
    localProjectDetails.forEach(x => {
      if (x.id == to_delete_project.id) {
        localProjectDetails.splice(i, 1);
      }
      i++;
    });


    // this.dashboardService.getUserById(this.token.decodedToken.id)


    this.dashboardService.deleteProject(to_delete_project).subscribe(y => {
      console.log('Deleted Project');
      this.all_projects = localProjectDetails;
      this.logged_in_user.projectDetails.forEach(x => {
        if (x.projectId === to_delete_project.id) {
          console.log(x);
          this.logged_in_user.projectDetails.splice(j, 1);
        }
        j += 1;
      });
      console.log(this.logged_in_user);
      this.dashboardService.putUser(this.logged_in_user).subscribe();
      // connection.invoke('SendAllProject', dashboardid, this.all_projects);
    },
      err => this._snackBar.open("You cannot delete this project","",{duration:2000}));
  }
  }

  routeTo(open_project) {
    // if(this.token.decodedToken.id === open_project.owner){
    // console.log('role of user',open_project.owner);
    // localStorage.setItem('Role','owner');
    // }else{
    //   localStorage.setItem('Role','member');
    // }
    this.router.navigate(['ideazone/workspace/' + open_project.id]);
    // this.idzoneapi.setProjectId(open_project.id);

    localStorage.setItem('ideazone_projectid', open_project.id);
  }

  onBlurName(edited_title, project_edited) {
    console.log(edited_title);
    console.log(project_edited);
    this.all_projects.forEach(x => {
      if (x.id == project_edited.id) {
        x.projectName = edited_title;
      }
    });
    project_edited.projectName = edited_title;
    // connection.invoke('SendAllProject', dashboardid, this.all_projects);

    this.dashboardService.putProject(project_edited).subscribe();
  }

  onBlurDescription(edited_description, project_edited) {
    this.all_projects.forEach(x => {
      if (x.id == project_edited.id) {
        x.projectDescription = edited_description;
      }
    });
    project_edited.projectDescription = edited_description;
    // connection.invoke('SendAllProject', dashboardid, this.all_projects);

    this.dashboardService.putProject(project_edited).subscribe();
  }
}

@Component({
  selector: "project-dialog",
  templateUrl: 'project-dialog.html'
})
export class ProjectDialog {
  constructor(
    public dialogRef: MatDialogRef<ProjectDialog>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) { }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
