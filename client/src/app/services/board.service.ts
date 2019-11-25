import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BoardService {

  constructor(private httpClient: HttpClient) { }

  // getapi(){
  //   return this.httpClient.get('https://api.kucoin.com/api/v1/symbols');
  // }

  sendmail(usid, boardid, role) {
    console.log('boardservice post mail');
    return this.httpClient.post(environment.starturlserver + '/List/sendmail?usid=' + usid + '&boardid=' + boardid + '&role=' + role, 's');
  }
  getValue(): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/Values');
  }
  getUserStories(): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/KanbanUS');
  }
  getUSById(id): Observable<any> {
    // console.log(id);
    return this.httpClient.get(environment.starturlserver + '/KanbanUS/' + id);
  }

  getByLinkedId(id): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/KanbanUS/linkedid/' + id);
  }
  getByUniqueId(id): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/KanbanUS/uniqueid/' + id);
  }

  getUSByProjectId(id): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/KanbanUS/projectid/' + id);
  }
  getList(listname): Observable<any> {
    const x = listname.replace(' ', '%20');
    console.log(x);
    return this.httpClient.get(environment.starturlserver + '/List/listname/' + x);
  }

  getFullList(projectid): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/List/projectid/' + projectid);
  }

  getLastUserId(name): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/KanbanUS/lastuserid/' + name);
  }
  getAllUserData(): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/Users');
  }

  putList(list): Observable<any> {
    console.log(list);
    return this.httpClient.put(environment.starturlserver + '/List/' + list.id, list);
  }
  postColumn(col): Observable<any> {
    return this.httpClient.post(environment.starturlserver + '/List', col);
  }
  postUserStories(us): Observable<any> {
    return this.httpClient.post(environment.starturlserver + '/KanbanUS', us);
  }
  putUserStories(us): Observable<any> {
    return this.httpClient.put(environment.starturlserver + '/KanbanUS/' + us.id, us);
  }

  putUser(user): Observable<any> {
    return this.httpClient.put(environment.starturlserver + '/Users/' + user.id, user);
  }

  deleteUserStory(us): Observable<any> {
    return this.httpClient.delete(environment.starturlserver + '/KanbanUS/' + us);
  }
  deleteProjectFromList(projectid): Observable<any> {
    return this.httpClient.delete(environment.starturlserver + '/List/' + projectid);
  }
  getActivity(projectid): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/LoggerActivity/projectid/' + projectid);
  }
  getSpecificByUser(usid , projectid) {
    return this.httpClient.get(environment.starturlserver + '/LoggerActivity/userid/projectid/' + usid + '/' + projectid);
  }
}

