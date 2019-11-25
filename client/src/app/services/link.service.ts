import { Injectable } from '@angular/core';
import {Link} from '../models/link';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from '../../environments/environment';
import {AuthService} from '../services/auth.service';
@Injectable({
  providedIn: 'root'
})
export class LinkService {
  constructor(private http: HttpClient, private auth: AuthService) { }

  public get(msg): Observable<any> {
    return this.http.get(environment.linkurl + 'gantt/' + msg);
  }
  public insert(msg, projectid): Observable<any> {
    console.log('LINK INSERTED');
    const username = this.auth.jwtToken().decodedToken.username;
    const userid = this.auth.jwtToken().decodedToken.id;
    console.log(userid);
    console.log(username);
    return this.http.post(environment.linkurl + '?username=' + username + '&userid=' + userid + '&projectid=' + projectid, msg);
  }

  public remove(msg, projectid): Observable<any> {
    console.log('LINK DELETED');
    const username = this.auth.jwtToken().decodedToken.username;
    const userid = this.auth.jwtToken().decodedToken.id;
    console.log(userid);
    console.log(username);
    return this.http.delete(environment.linkurl + 'gantt/' + msg + '?username=' + username + '&userid=' + userid + '&projectid=' + projectid);
  }


}
