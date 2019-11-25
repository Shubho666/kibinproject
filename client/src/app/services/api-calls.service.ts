import { Injectable, ɵCodegenComponentFactoryResolver } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, switchMap } from 'rxjs/operators';
import { Observable, VirtualTimeScheduler, of } from 'rxjs';
import {  environment } from 'src/environments/environment';
import { AuthService } from 'src/app/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ApiCallsService {
projectId = localStorage.getItem('ideazone_projectid');
UserId = localStorage.getItem('ideazone_userid');
WorkspaceObject;
PersonalspaceObject;
done = false;
// const userdetails = this.auth.jwtToken();


  createdUserstoriesIds = [];

  constructor(private http: HttpClient, private auth: AuthService) { }
  getActivity() {
    this.projectId = localStorage.getItem('ideazone_projectid');
    return this.http.get(environment.ideazoneurlapi + '/LoggerActivity/projectid/' + this.projectId );
  }
  getSpecificByUser() {
    this.UserId = localStorage.getItem('ideazone_userid');
    return this.http.get(environment.ideazoneurlapi + '/LoggerActivity/userid/projectid/' + this.UserId + '/' + this.projectId);
  }


// These Methods are called initially when ideazone page loads
// this.projectId = ;

setProjectId(projectId) {
  this.projectId = projectId;
}
setUserId(userId) {
this.UserId = userId;
}

getProjectId() {
  this.projectId = localStorage.getItem('ideazone_projectid');
  return this.projectId;
}
getUserId() {
return localStorage.getItem('ideazone_userid');
}


initideazone() {
this.getEpicsFromProjectO(this.projectId).subscribe((data: any) => {
  // console.log(data);
  this.WorkspaceObject = data;
  if ( (data.projectId) === '') {
  console.log('Searching in Workspace for project' + this.projectId);
  console.log('returned this ' + data.projectId);
  this.createProjectInWorkspace(this.projectId).subscribe();
}

  // tslint:disable-next-line: no-shadowed-variable
  this.getDetailsFromPerosnalSpace(this.UserId).subscribe((data: any) => {
 // console.log(data);
  if (data.userId === 'No-such-user') {
    this.createNewUserinPersonalSpace(this.UserId).subscribe();
} else {
this.PersonalspaceObject = data;
let projectIndex;
if (this.PersonalspaceObject.pse.length === 0) {
projectIndex = -1;
} else {
projectIndex = this.PersonalspaceObject.pse.findIndex(x => x.projectId === this.projectId);
// console.log('Project index is' + projectIndex);
}
// console.log(projectIndex);
if (projectIndex === -1) {
console.log('pushing project into ' + this.PersonalspaceObject + ' ' + this.projectId);
this.PersonalspaceObject.pse.push({
                                 projectId: this.projectId,
                                 epics: []
                                  }
                                  );
this.putNewProjectinPersonalSpace(this.PersonalspaceObject.userId).subscribe();
}
}
});
  this.done = true;
});

}

// Api calls Related to User stories (User Story idea Zone COllection)
getUserStory(id) {
return this.http.get(environment.ideazoneurlapi + `/UserStoryIdeaZone/${id}`).pipe(
  map((data: any) => data)
);
}

getUserStoryC(id) {
  return this.http.get(environment.ideazoneurlapi + `/UserStoryIdeaZone/${id}`);
}

createNewUserStory(newstory) {
return this.http.post(environment.ideazoneurlapi + `/UserStoryIdeaZone`, {userStoryName: newstory}).pipe(
  map((data: any) => data)
);
}

deleteUserStory(userstoryid) {
  return this.http.delete(environment.ideazoneurlapi + `/UserStoryIdeaZone/${userstoryid}`);
}

UpdateUSerStory(data) {
// console.log(data.ac);
return this.http.put(environment.ideazoneurlapi + `/UserStoryIdeaZone/${data.storyId}`,
 {
Id: data.storyId,
UserStoryName: data.story,
UserStoryType: data.type,
UserStoryDescription: data.ac
 });
}
UpdateUserStoryStatus(storyId, status) {
  console.log('Move thse story to' , status);
  const projectId = localStorage.getItem('ideazone_projectid');
  const username = this.auth.jwtToken().decodedToken.username;
  const userid = this.auth.jwtToken().decodedToken.id;

  if (status === 'Backlog') {
  return this.http.put
  (environment.ideazoneurl + `/userstoryStatus/${storyId}?status=Backlog&ProjectId=${projectId}&username=${username}&userid=${userid}`,
  storyId);
  }
  if (status === 'Workspace') {
    return this.http.put
    (environment.ideazoneurl + `/userstoryStatus/${storyId}?status=Workspace&ProjectId=${projectId}&username=${username}&userid=${userid}`,
    storyId);
    }
}


// Api calls realted to Epics (EpicsIdZoneCollection)
// This call should be project specific instead of 101

getEpicsFromProjectO(projectId) {
  return this.http.get(environment.ideazoneurl + `/workspace/${projectId}`);
}

getEpicsByIdFromProject(id) {
return this.http.get(environment.ideazoneurlapi + `/EpicsIdZone/${id}`);
}

createNewEpic(EPIC, userstories, Status) {
  const username = this.auth.jwtToken().decodedToken.username;
  const userid = this.auth.jwtToken().decodedToken.id;
  // console.log(username);
  return this.http.post
  (environment.ideazoneurlapi + `/EpicsIdZone?username=${username}&userid=${userid}&projectId=${this.projectId}`,
  {
    epicName: EPIC,
    status: Status,
    userStories: userstories
  })
    .pipe(map((data: any) => data.id));
}

changestatusofepic(epicid, Status) {
  const username = this.auth.jwtToken().decodedToken.username;
  const userid = this.auth.jwtToken().decodedToken.id;
  // console.log(username);
  return this.http.put
(environment.ideazoneurl +
 `/EpicsIdzone/${epicid}/EpicStatus?status=${Status}&username=${username}&userid=${userid}&projectId=${this.projectId}`,
                                                                                epicid, Status);
}

deleteEpic(epicid) {
  const username = this.auth.jwtToken().decodedToken.username;
  const userid = this.auth.jwtToken().decodedToken.id;
  // console.log(username);
  return this.http.delete
  (environment.ideazoneurlapi + `/EpicsIdZone/${epicid}?username=${username}&userid=${userid}&projectId=${this.projectId}`);
}

addUserStory(epicId, usId, ustory) {
  // console.log('from service ' , epicId);
  // console.log('from service ', usId);
  const username = this.auth.jwtToken().decodedToken.username;
  const userid = this.auth.jwtToken().decodedToken.id;
  return this.http.
  put(environment.ideazoneurl
  + `/EpicsIdZone/${epicId}/addStory?userStory=${usId}&username=${username}&story=${ustory}&userid=${userid}&projectId=${this.projectId}`,
                                                                                          epicId, usId);
}

removeUserStoryFromEpic(epicId, usId , ustory) {
  console.log('To Be Deleted Story' , ustory);
  const username = this.auth.jwtToken().decodedToken.username;
  const userid = this.auth.jwtToken().decodedToken.id;
  // tslint:disable-next-line: max-line-length
  return this.http.put(environment.ideazoneurl +
`/EpicsIdZone/${epicId}/removeStory?userStory=${usId}&username=${username}&story=${ustory}&userid=${userid}&projectId=${this.projectId}`,
  epicId, usId);
}




// Api calls related to Workspace Collection
// This call should be actually project id dependent

createProjectInWorkspace(ProjectId) {
  return this.http.post(environment.ideazoneurlapi + `/Workspace`,
  {
projectId: ProjectId,
epics: []
  });

}

// this should be actually project specific call
postNewEpicInWorkspace(epicId) {
  console.log('while posting Epic in Workspace', this.projectId);
  return this.http.put(environment.ideazoneurlapi + `/Workspace/${this.projectId}?epicid=${epicId}`,
                           epicId);
}

// This call should be project specific instead of 101
deleteNewEpicFromWorkspace(projectId, epicid) {
return this.http.put(environment.ideazoneurl + `/Workspace/${projectId}/epic/${epicid}`, projectId , epicid);
}



// Api calls related to private Space collection
// Actually this should be userId specific call (instead of 101 there should be {userid})

createNewUserinPersonalSpace(UserId) {
  console.log('creating new user in perosnal space with id,projectid ' , UserId , this.projectId);
  return this.http.post(environment.ideazoneurlapi + `/PrivateSpace`,
  {
userId: UserId,
pse: [{
projectId: this.projectId,
epics: []
      }
]
  }
  );

}

putNewProjectinPersonalSpace(UserId) {
console.log(this.PersonalspaceObject);
return this.http.put(environment.ideazoneurl + `/PrivateSpace/user/${UserId}`,
this.PersonalspaceObject);
}


getDetailsFromPerosnalSpace(userId) {
  return this.http.get(environment.ideazoneurl + `/PrivateSpace/user/${userId}`);
}

getEpicsFromPersonalSpace(userId) {
return this.http.get(environment.ideazoneurl + `/PrivateSpace/user/${userId}`);
}

PostEpicToPersonalSpace(personalSpaceUpdatedObject) {
  console.log('Updating this', personalSpaceUpdatedObject);
  return this.http.put(environment.ideazoneurl + `/PrivateSpace/user/${this.UserId}`, personalSpaceUpdatedObject);
}




}
