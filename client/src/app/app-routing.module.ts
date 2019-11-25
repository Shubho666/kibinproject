import { AuthComponent } from './components/auth/auth.component';
import { AuthguardService as AuthGuard } from './services/authguard.service';
import { Board3Component } from './components/board3/board3.component';
import { CardsRequestsComponent } from './components/cards-requests/cards-requests.component';
import { CommonModule } from '@angular/common';
import { ChartsModule } from 'ng2-charts';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { GanttComponent } from './components/gantt/gantt.component';
import { IdeazoneComponent } from './components/ideazone/ideazone.component';
import { NgModule } from '@angular/core';
import { PrivatespaceComponent } from './components/privatespace/privatespace.component';
import { RouterModule, Routes } from '@angular/router';
import { AddMemberComponent } from './components/add-member/add-member.component';
import { ChatsystemComponent } from './components/chatsystem/chatsystem.component';
import {PrivatetokenComponent} from './components/privatetoken/privatetoken.component';
import { ReportComponent } from './components/report/report.component';
import { HomepageComponent } from './components/homepage/homepage.component';
import { MyActivityComponent } from './components/my-activity/my-activity.component';
import { WorkspaceComponent } from './components/workspace/workspace.component';

// import {addMemberComponent} from './components/'
const routes: Routes = [
  {path: '', redirectTo: 'login', pathMatch: 'full'},
  { path: 'board/:boardid', component: Board3Component, canActivate: [AuthGuard] },
  { path: 'gantt/:id', component: GanttComponent, canActivate: [AuthGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'login', component: AuthComponent },
  { path: 'chat/:userid' , component : ChatsystemComponent},
  { path: 'invitation/:usid/:boardid/:role' , component : AddMemberComponent, canActivate: [AuthGuard]},
  { path: 'ideazone/workspace/:projectid', component: IdeazoneComponent, canActivate: [AuthGuard] },
  { path: 'ideazone/privatespace', component: PrivatespaceComponent, canActivate: [AuthGuard] },
  {path: 'accessToken', component: PrivatetokenComponent},
  {path: 'report/:projectid', component: ReportComponent} ,
  {path: 'homepage', component: HomepageComponent},
  {path: 'ideazone/Workspace/:projectid' , component: WorkspaceComponent},

  { path: 'myactivity/:id', component: MyActivityComponent, canActivate: [AuthGuard] }
];
@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
