import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(private httpClient: HttpClient) { }

  getProjectByOwner(dashboardid): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/Project/owner/' + dashboardid);
  }

  getProjectById(project): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/Project/' + project);
  }

  postProject(project): Observable<any> {
    return this.httpClient.post(environment.starturlserver + '/Project', project);
  }

  putProject(project): Observable<any> {
    return this.httpClient.put(environment.starturlserver + '/Project/' + project.id, project);
  }

  deleteProject(project): Observable<any> {
    return this.httpClient.delete(environment.starturlserver + '/Project/' + project.id);
  }

  getUserById(userid):Observable<any>{
    console.log('userid', userid);
    return this.httpClient.get(environment.starturlserver+'/Users/'+userid);
  }

  getByUserName(username): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/Users/' + username);
  }

  putUser(user): Observable<any> {
    return this.httpClient.put(environment.starturlserver + '/Users/' + user.id, user);
  }
}
