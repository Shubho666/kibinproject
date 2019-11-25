import { Injectable } from '@angular/core';
import {Task} from '../models/task';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from '../../environments/environment';
import {AuthService} from '../services/auth.service';
@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private http: HttpClient, private auth: AuthService) { }
  projectId;
  UserId;

  getActivity() {
    this.projectId = localStorage.getItem('ideazone_projectid');
    return this.http.get(environment.Gloggerurl + '/Logger/project/' + this.projectId );
  }
  getSpecificByUser() {
    this.UserId = localStorage.getItem('ideazone_userid');
    this.projectId = localStorage.getItem('ideazone_projectid');
    return this.http.get(environment.Gloggerurl  + '/Logger/project/userid/' + this.projectId +'/' + this.UserId);
  }


  public get(msg): Observable<any> {
     return this.http.get(environment.taskurl + 'gantt/' + msg);
  }

  public insert(msg, projectid): Observable<any> {
    let flag = localStorage.getItem(msg.id);
    // console.log("INSERTED",msg);
    if (flag != 'called'){
    localStorage.setItem(msg.id, 'called');
    const username = this.auth.jwtToken().decodedToken.username;
    const userid = this.auth.jwtToken().decodedToken.id;
    // console.log(userid);
    // console.log(username);
    console.log('hitting here');
    return this.http.post(environment.taskurl + '?username=' + username + '&userid=' + userid + '&projectid=' + projectid, msg);
  }
}

  public remove(msg, projectid): Observable<any> {
    const username = this.auth.jwtToken().decodedToken.username;
    const userid = this.auth.jwtToken().decodedToken.id;
    console.log(userid);
    console.log(username);
    console.log('DELETED', msg);
    return this.http.delete(environment.taskurl + 'gantt/' + msg + '?username=' + username + '&userid=' + userid + '&projectid=' + projectid);
  }

  public update(msg, projectid): Observable<any> {
    const username = this.auth.jwtToken().decodedToken.username;
    const userid = this.auth.jwtToken().decodedToken.id;
    console.log(userid);
    console.log(username);
    console.log('Updated');
    return this.http.put(environment.taskurl + 'gantt/' + msg.id + '?username=' + username + '&userid=' + userid + '&projectid=' + projectid, msg);

  }

}
