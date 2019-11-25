import { Component, OnInit, Inject, Input } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {DashboardService} from '../../services/dashboard.service';
import { DOCUMENT } from "@angular/common";
import { environment } from 'src/environments/environment';
import * as signalR from '@aspnet/signalr';
import { userInfo } from 'os';

let connection;
@Component({
  selector: 'app-add-member',
  templateUrl: './add-member.component.html',
  styleUrls: ['./add-member.component.css']
})
export class AddMemberComponent implements OnInit {

  constructor(@Inject(DOCUMENT) private document: Document,private route:ActivatedRoute,private authService:AuthService,private dashboardService:DashboardService,private router:Router) { }
  loggedInUserId;usid;boardid;
  projectDetails;
  role;status;
  ngOnInit() {
    connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.starturlconnection + "/notify", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    connection
      .start()
      .then(function() {
        console.log("Connected!", connection);
      })
      .catch(function(err) {
        return console.error(err.toString());
      });
    setTimeout(() => {
      connection
        .invoke("JoinGroup", this.boardid)
        .then(res => console.log("Joined Group", this.boardid))
        .catch(err => {
          console.log("Error", err);
        });
    }, 1000);


      this.boardid = this.route.snapshot.paramMap.get('boardid'); 
      this.role=this.route.snapshot.paramMap.get('role');
    this.loggedInUserId=this.authService.jwtToken().decodedToken.id;   
    this.usid= this.route.snapshot.paramMap.get('usid');
    this.dashboardService.getProjectById(this.boardid).subscribe(x=>{
      this.projectDetails=x;
      if(this.usid==this.loggedInUserId){
        let flag=false;
        // if(this.projectDetails.owner==this.loggedInUserId){
        //   flag=true;
        // }
        this.projectDetails.owner.forEach(o=>{
          if(o==this.loggedInUserId){
            flag=true;
          }
        })
        this.projectDetails.members.forEach(x=>{
          if(x==this.loggedInUserId){
            flag=true;
          }
        });
        if(flag==false){
          // this.projectDetails.members.push(this.usid);
          if(this.role=='owner'){
            this.projectDetails.owner.push(this.usid);
            this.status=false;
          }
          else if(this.role=='member'){
            this.projectDetails.members.push(this.usid);
            this.status=true;
          }
          this.dashboardService.getUserById(this.usid).subscribe(user=>{
            let setrole;
            if(this.status==false){
              setrole="Owner";
            }
            else{
              setrole="Member";
            }
            let project={
              projectId:this.boardid,
              role:setrole
            };
            user.projectDetails.push(project);
            this.dashboardService.putUser(user).subscribe();
            this.dashboardService.putProject(this.projectDetails).subscribe(x=>{
              let obj={
                memberId:this.usid,
                status:this.status,
                img:user.avatar_url
              }
              connection.invoke("SendForAddMembers",this.boardid,obj);
              console.log(this.boardid);
              localStorage.setItem('ideazone_projectid',this.boardid);
              this.router.navigate(['/board/'+this.boardid]);
              // this.document.location.href=environment.starturlclient+'/board/'+this.boardid;
            })
          });
        }
        else{
          localStorage.setItem('ideazone_projectid',this.boardid);
          this.router.navigate(['/board/'+this.boardid]);
          // this.document.location.href=environment.starturlclient+'/board/'+this.boardid;
        }
      }
      else{
        alert('You cannot access this link');
      }
    
    });
    //us first board second
    
    //console.log(boardid);console.log(usid);
    // this.addMemberByEmail();
  }
  addMemberByEmail(){
    console.log("add member");}
  //   if(this.usid==this.loggedInUserId){
  //     let flag=false;
  //     if(this.projectDetails.owner==this.loggedInUserId){
  //       flag=true;
  //     }
  //     this.projectDetails.members.forEach(x=>{
  //       if(x==this.loggedInUserId){
  //         flag=true;
  //       }
  //     });
  //     if(flag==false){
  //       this.projectDetails.members.push(this.usid);
  //       this.dashboardService.putProject(this.projectDetails).subscribe(x=>{
  //         this.router.navigate(['/board/'+this.boardid]);
  //         // this.document.location.href=environment.starturlclient+'/board/'+this.boardid;
  //       });
  //     }
  //     else{
  //       this.router.navigate(['/board/'+this.boardid]);
  //       // this.document.location.href=environment.starturlclient+'/board/'+this.boardid;
  //     }
  //   }
  //   else{
  //     alert('You cannot access this link');
  //   }
  // }
}
