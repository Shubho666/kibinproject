import { Component, OnInit, Input, Output, EventEmitter, DoCheck } from '@angular/core';
import { ApiCallsService } from 'src/app/services/api-calls.service';
import { IdeazoneDataSplitService } from 'src/app/services/ideazone-data-split.service';
import { DatasharingService} from 'src/app/services/datasharing.service';
import * as signalR from '@aspnet/signalr';
import { ActivatedRoute } from '@angular/router';
import { ErrorStateMatcher } from '@angular/material/core';
import { environment } from 'src/environments/environment';
let connection;
@Component({
  selector: 'app-epics-creation-area',
  templateUrl: './epics-creation-area.component.html',
  styleUrls: ['./epics-creation-area.component.css']
})
export class EpicsCreationAreaComponent implements OnInit, DoCheck {

  @Input() ProjectId;
  @Input() UserId;
  // @Input() Role;

  projectid;
  // differentiating variable
  @Input() calledBy;
  @Input() requestsView;
  requests;
  myrequests;
  @Output() messageEvent = new EventEmitter<number>();

  // Epic Error Message
  EpicErrorMessage = false;
  matcher = new ErrorStateMatcher();


  //     Front end variables
  satya = [true];
  indexes = [true];
  newstory; stories = []; createdUserstoriesIds = []; createdEpicID;
  createdEpic = { EpicId: '', Epic: '', userStoriesIds: [], userstories: [], status: '',
  userstoryStatuses: [], userstorychecked: []};
  panelOpenState; remove = false; showX; deleted = 36;
  //
  // data;
  // Backend variables
  EPIC;
  Totalepics = [];
  epicw = [];
  epics = []; epicsh = []; epicsR = [];
  epicsp = []; epicsph = []; epicsPR = [];
  pseI;
  PrivateSpaceobject;
  Role;

  constructor( private api: ApiCallsService, private ds: IdeazoneDataSplitService, private route: ActivatedRoute) { }

  ngDoCheck() {
    // this.Role =this.dss.getRole();

    this.Role = localStorage.getItem('Role');
    // owner,member

  }

ngOnInit() {

this.ProjectId = localStorage.getItem('ideazone_projectid');
this.UserId = localStorage.getItem('ideazone_userid');

this.EpicErrorMessage = false;
this.getId();
connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.ideazoneurl + '/colhub', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();
connection
      .start()
      // tslint:disable-next-line: only-arrow-functions
      .then(function() {
        console.log('Connected!', connection);
      })
      // tslint:disable-next-line: only-arrow-functions
      .catch(function(err) {
        return console.error(err.toString());
      });
connection.on('ReceiveMessage', message => {
      console.log(message);
      const Message = {
        Epic: message.epic,
        EpicId: message.epicId,
        status: message.status,
        userStoriesIds: message.userStoriesIds,
        userstories: message.userstories,
        userstoryStatuses: message.userstoryStatuses,
        userstorychecked: message.userstorychecked
      };
      this.epics.unshift(Message);
      // this.Totalepics.unshift(Message);
    });

    // Delete Epic using SignalR
connection.on('ReceiveMessageD', message => {
      console.log('Message From SignalR During Epic deletion', message);
      const indexEpic = this.epics.findIndex(x => x.Epic === message);
      this.epics.splice(indexEpic, 1);
      // this.Totalepics.splice(indexEpic, 1)
    });

    // Adding User Story using SignalR
connection.on('ReceiveMessageAU', message => {
      console.log('Message From SignalR Duuring adding UserStory', message);
      const indexEpic = this.epics.findIndex(x => x.Epic === message.epic);
      console.log('EpicIndex ', indexEpic);
      console.log('Story Index ', message.sindex);
      this.epics[indexEpic].userstories[message.sindex] = message.addeduserstory;
      this.epics[indexEpic].userStoriesIds[message.sindex] = message.addeduserstoryId;
    });


    // Modal Change using SignalR
connection.on('ReceiveMessageM', message => {
      console.log('Message From SignalR After Modal Change', message);
      const i = this.epics.findIndex(x => x.EpicId === message.epicId);
      if (message.flag === false) {
        this.epics[i].userstories[message.storyIndex] = message.storyValue;
      } else {
        this.epics[i].userstories.splice(message.storyIndex, 1);
      }
    });

    // console.log("received message", this.all_lists);
setTimeout(() => {
      connection
        .invoke('JoinGroup', this.projectid)
        .then(res => console.log('Joined Group', this.projectid))
        .catch(err => {
          console.log('Error', err);
          console.log(this.projectid);
        });
    }, 1000);
this.PrivateSpaceobject = {
      id: '',
      userId: this.UserId,
      pse: [{
        projectId: '',
        epics: []
      }
      ]
    };
if (this.calledBy === 'Workspace') {
      this.getdata();
    } else {
      // this.getdataforPs('101');
      console.log(this.UserId);
      this.getdataforPs(this.UserId);
    }
  }
  getId() {
    this.projectid = this.route.snapshot.paramMap.get('projectid');
  }
  // Backend call for getting epics in ideaZone
  getdata() {
    this.api.getEpicsFromProjectO(this.ProjectId)
      .subscribe((data: any) => {
        const epicsarray = data.epics;
        console.log(epicsarray);
        // this.epicsh = data;
        if (epicsarray !== null) {
          // tslint:disable-next-line: prefer-for-of
          for (let i = 0; i < epicsarray.length; i++) {
            this.api.getEpicsByIdFromProject(epicsarray[i]).subscribe((datai: any) => {
              this.epicsh.push(datai);
              if (this.epicsh.length === epicsarray.length) {
                console.log('All epics are loaded');
                this.getstories(this.epicsh);
              }
            });
          }
        }
      });
  }
  // Backend call for getting PersonalSpcae epics of a user
  // Call Should be User Specific and Project specific
  getdataforPs(UserId) {
    this.api.getEpicsFromPersonalSpace(UserId)
      .subscribe((data: any) => {
        console.log('Personal Workspace', data);
        this.PrivateSpaceobject.id = data.id;
        this.PrivateSpaceobject.pse = data.pse;
        this.pseI = data.pse.findIndex(x => x.projectId === this.ProjectId);
        const epicp = data.pse[this.pseI].epics;
        this.PrivateSpaceobject.pse[this.pseI].epics = data.pse[this.pseI].epics;
        console.log(epicp.length);
        // tslint:disable-next-line: forin
        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < epicp.length; i++) {
          // console.log('epic id in personal space', epicp[i]);
          this.api.getEpicsByIdFromProject(epicp[i])
            .subscribe((datai: any) => {
              // console.log(' epic data from epic id zonee in perrsonal space ', datai);
              this.epicsph.push(datai);
              if (this.epicsph.length === epicp.length) {
                this.getstories(this.epicsph);
                console.log('Loaded epics', this.epicsph.length);
              }
            });
        }
      });
  }
  // Backend call for getting user stories for each epic
  // After this method all epics from data base will be stored in the epics array
  // epics array srtucture epics {EpicId: '', Epic: '', userStoriesIds: [] , userstories: []}
  getstories(data) {

    // console.log(data);
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < data.length; i++) {
      const newepic = { EpicId: '', Epic: '', status: '', userStoriesIds: [], userstories: [],
                        userstoryStatuses: [] , userstorychecked: []};
      // console.log(data[0].userStories);
      newepic.Epic = data[i].epicName;
      newepic.EpicId = data[i].id;
      // newepic.userStoriesIds = data[i].userStories;
      newepic.status = data[i].status;
      // console.log(newepic.Epic);
      // for (let j = 0; j < data[i].userStories.length; j++)
      // tslint:disable-next-line: forin
      for (const j in data[i].userStories) {
        // console.log(data[i].userStories[j])
        this.api.getUserStory(data[i].userStories[j])
          .subscribe((userstory: any) => {
            newepic.userStoriesIds.push(data[i].userStories[j]);
            newepic.userstories.push(userstory.userStoryName);
            if (userstory.status === null) {
              userstory.status = 'Workspace';
            }
            newepic.userstoryStatuses.push(userstory.status);
            newepic.userstorychecked.push(false);
          });
        // console.log(newepic);
      }
      if (this.calledBy === 'Workspace') {
        this.epics.push(newepic);
      } else {
        // console.log('pushing to epicp');
        this.epicsp.push(newepic);
        // console.log('epicsp' , this.epicsp);
      }
    }
    if (this.calledBy === 'Workspace') {
      console.log('Actual data', this.epics);
      this.Totalepics = this.epics;
      this.epics = this.ds.Workspacedatasplit(this.epics);
      console.log('After Splitting', this.epics);
      this.epicsR = this.ds.WorkspacedatasplitR();
      console.log('requests', this.epicsR);
      this.requests = this.epicsR.length;
    } else {
      console.log('Actual data', this.epicsp);
      this.epicsp = this.ds.Privatespacedatasplit(this.epicsp);
      console.log('After Splitting', this.epicsp);
      this.epicsPR = this.ds.PrivaetespacedatasplitR();
      console.log('requests', this.epicsPR);
      this.myrequests = this.epicsPR.length;
    }
  }
  // Front End Functions
  // This methods will be called on clicking move to workspace or move to product backlog
  moveToWorkspace_Backlog_Request(epic) {
    if (this.calledBy === 'Workspace') {
      const index = this.epics.indexOf(epic);
      // this.epics.splice(index, 1);
      if (epic.status === 'workspace') {
      this.epics[index].status = 'productbacklog';
      this.api.changestatusofepic(epic.EpicId, 'productbacklog').subscribe();

      // tslint:disable-next-line: prefer-for-of
      for (let i = 0; i < epic.userStoriesIds.length; i++) {
        if (epic.userstoryStatuses[i] !== 'Backlog') {
          epic.userstoryStatuses[i] = 'Backlog';
          this.api.UpdateUserStoryStatus(epic.userStoriesIds[i], 'Backlog').subscribe();
        }
      }
      } else {
        this.epics[index].status = 'workspace';
        this.api.changestatusofepic(epic.EpicId, 'workspace').subscribe();
        // tslint:disable-next-line: prefer-for-of
        for (let i = 0; i < epic.userStoriesIds.length; i++) {
          epic.userstoryStatuses[i] = 'Workspace';
          this.api.UpdateUserStoryStatus(epic.userStoriesIds[i], 'Workspace').subscribe();
        }
      }

    } else {
      console.log('This epic is gone a change its status ', epic.EpicId);
      this.api.changestatusofepic(epic.EpicId, 'workspace').subscribe();
      epic.status = 'workspace';
      this.epicsR.unshift(epic);
      this.epicsPR.unshift(epic);
      const index = this.epicsp.indexOf(epic);
      this.epicsp.splice(index, 1);
      this.epics.unshift(epic);
    }
  }
  // This method will be called on clicking discard
  MakeEverythingNull() {
    this.EPIC = '';
    this.satya = [true];
    this.indexes = [true];
    this.stories = [];
  }
  // This method will add new row in the front end with empty space
  newStory(index) {
    console.log('index called ' , index);
    console.log(this.indexes[index]);
    // console.log(' create new row called by ' + eventt);
    if (this.indexes[index] === true ) {
    this.satya.push(true);
    this.stories.push('');
    this.indexes.push(true);
    this.indexes[index] = false;
    }
    return true;
  }
  // This method will save user story whenever we edit particular user story
  savestory(i, story) {
    console.log('save story called', story, i);
    if (story !== '') {
      console.log('story saved');
      this.stories[i] = story;
    }
  }
  // This method will be called whwnever we click on  (X) mark in the front end
  deletestory(i) {
    if (i === this.satya.length - 1) {
      // console.log('you are deleting last story');
      this.satya.push(true);
      this.indexes.push(true);
    }
    this.satya.splice(i, 1);
    this.stories.splice(i, 1);
    this.indexes.splice(i, 1);
    // this.satya.push(true);
    // this.satya[i] = false;
    // this.stories[i] = 'deleted';
    //   if (this.satya.length === 0) {
    //    this.satya.push(true);
    //  }
    if ((i - 1) === this.stories.length - 1) {
      this.deleted = (i - 1);
      console.log('deleted index ', this.deleted);
    }
    // console.log(this.satya.length);
    // for (i = 0; i < this.stories.length; i++) {
    //   console.log(i + ' ' + this.stories[i]);
    // }
  }
  // This method will be called after clicking on create button in the front end
  // This methods makes backend call to create user stories in the userstory collectiion
  createNewUserStories() {
    // tslint:disable-next-line: forin
    // for (const i in this.stories) {
    //   console.log(this.stories[i]);
    // }
    let lengthOfStories = this.stories.length;
    let indexEpic = -1;

    // For Private Space Checking

    if (this.calledBy === 'Privatespace') {
      indexEpic = -1;
      this.epicsh = [];
      console.log('Inside Private Space Epic Creation area');
      this.api.getEpicsFromProjectO(this.ProjectId)
        .subscribe((data: any) => {
          const epicsarray = data.epics;
          console.log(epicsarray.length);
          if (epicsarray.length === 0) {
            this.panelOpenState = false;
            if (this.stories.length === 0 || (this.stories.length === 1 && this.stories[0] === '')) {
              // console.log('called by me story length 0 or by the end');
              this.createNewEpic();
            } else {
              if (this.stories[this.stories.length - 1] === '') {
                this.stories.pop();
                lengthOfStories -= 1;
              }
              for (let i = 0; i <= this.stories.length - 1; i++) {
                if (this.stories[i] === '') {
                  console.log('inside null story');
                  lengthOfStories -= 1;
                  continue;
                }
                this.api.createNewUserStory(this.stories[i])
                  // tslint:disable-next-line: no-shadowed-variable
                  .subscribe((datai: any) => {
                    this.createdUserstoriesIds.push(datai.id);
                    console.log('For new epic / Story created is ', this.createdUserstoriesIds);
                    this.createdEpic.userStoriesIds.push(datai.id);
                    this.createdEpic.userstories.push(datai.userStoryName);
                    this.createdEpic.userstoryStatuses.push('Workspace');
                    this.createdEpic.userstorychecked.push(false);
                    // console.log(this.createdUserstoriesIds[0]);
                    if (this.createdEpic.userstories.length === lengthOfStories) {
                      console.log('about to call create new epic and last user story name ', data.userStoryName);
                      this.createNewEpic();
                    }
                  });
              }
            }
          } else {
            // this.epicsh = data;
            // tslint:disable-next-line: prefer-for-of
            for (let i = 0; i < epicsarray.length; i++) {
              this.api.getEpicsByIdFromProject(epicsarray[i]).subscribe((datai: any) => {
                // console.log(datai);
                this.epicsh.push(datai);
                if (datai.epicName === this.EPIC) {
                  // console.log('getting epics of workspace from private space', this.epicsh);
                  indexEpic = i;
                  console.log('Found index in private space epic creation', indexEpic);
                }
                if (this.epicsh.length === epicsarray.length) {
                  console.log(this.epicsh);
                  if (indexEpic === -1) {
                    this.EpicErrorMessage = false;
                    this.panelOpenState = false;
                    console.log('Checked whether index is -1 or not');
                    if (this.stories.length === 0 || (this.stories.length === 1 && this.stories[0] === '')) {
                      // console.log('called by me story length 0 or by the end');
                      this.createNewEpic();
                    } else {
                      if (this.stories[this.stories.length - 1] === '') {
                        this.stories.pop();
                        lengthOfStories -= 1;
                      }
                      // tslint:disable-next-line: no-shadowed-variable
                      for (let i = 0; i <= this.stories.length - 1; i++) {
                        if (this.stories[i] === '') {
                          lengthOfStories -= 1;
                          continue;
                        }
                        this.api.createNewUserStory(this.stories[i])
                          // tslint:disable-next-line: no-shadowed-variable
                          .subscribe((datai: any) => {
                            this.createdUserstoriesIds.push(datai.id);
                            console.log('For new epic / Story created is ', this.createdUserstoriesIds);
                            this.createdEpic.userStoriesIds.push(datai.id);
                            this.createdEpic.userstories.push(datai.userStoryName);
                            this.createdEpic.userstoryStatuses.push('Workspace');
                            this.createdEpic.userstorychecked.push(false);
                            console.log(this.createdUserstoriesIds[0]);
                            if (this.createdEpic.userstories.length === lengthOfStories) {
                              console.log('about to call create new epic and last user story name ', data.userStoryName);
                              this.createNewEpic();
                            }
                          });
                      }
                    }
                  } else {
                    console.log('Found the EPIC in Index', indexEpic, this.EPIC);
                    this.EpicErrorMessage = true;
                    this.panelOpenState = true;
                  }
                }
              });
            }
          }
          // console.log('Loaded epics while creating Epic in Privatespace' , this.epicsh);
          // indexEpic = this.epicsh.findIndex(x => x.Epic === this.EPIC );
        });
    }


    // tslint:disable-next-line: one-line
    else {
      indexEpic = -1;
      // this.Totalepics = this.epics;
      console.log(this.epics);
      indexEpic = this.epics.findIndex(x => x.Epic === this.EPIC);
      console.log('Index found in epics of workspace' , indexEpic);

      if (indexEpic === -1 && this.epicsR !== null) {
        indexEpic = this.epicsR.findIndex(x => x.Epic === this.EPIC );
      }

      if (indexEpic === -1) {
        this.EpicErrorMessage = false;
        this.panelOpenState = false;
        if (this.stories.length === 0 || (this.stories.length === 1 && this.stories[0] === '')) {
          // console.log('called by me story length 0 or by the end');
          this.createNewEpic();
        } else {
          if (this.stories[this.stories.length - 1] === '') {
            this.stories.pop();
            lengthOfStories -= 1;
          }
          for (let i = 0; i <= this.stories.length - 1; i++) {
            if (this.stories[i] === '') {
              lengthOfStories -= 1;
              continue;
            }
            this.api.createNewUserStory(this.stories[i])
              .subscribe((data: any) => {
                this.createdUserstoriesIds.push(data.id);
                console.log('For new epic / Story created is ', this.createdUserstoriesIds);
                this.createdEpic.userStoriesIds.push(data.id);
                this.createdEpic.userstories.push(data.userStoryName);
                this.createdEpic.userstoryStatuses.push('Workspace');
                this.createdEpic.userstorychecked.push(false);
                console.log(this.createdUserstoriesIds[0]);
                if (this.createdEpic.userstories.length === lengthOfStories) {
                  console.log('about to call create new epic and last user story name ', data.userStoryName);
                  this.createNewEpic();
                }
              });
          }
        }
      } else {
        console.log('Found the EPIC in Index', indexEpic, this.EPIC);
        this.EpicErrorMessage = true;
        this.panelOpenState = true;
      }
    }
  }

  // This method makes backend update on EpicIdzone collecion
  createNewEpic() {
    let Status;
    if (this.calledBy === 'Workspace') {
      Status = 'workspace';
    } else {
      Status = 'privatespace';
    }
    // console.log('Create new epic to backend called');
    // console.log('when called usesstory ids are' , this.createdUserstoriesIds);
    this.api.createNewEpic(this.EPIC, this.createdUserstoriesIds, Status)
      .subscribe((data: any) => {
        console.log('EPIC-ID', data);
        this.createdEpicID = data;
        this.createdEpic.EpicId = this.createdEpicID;
        this.createdEpic.Epic = this.EPIC;
        this.createdEpic.status = Status;
        // this.epics.unshift(this.createdEpic);//entire array
        if (this.calledBy === 'Workspace') {
          connection.invoke('SendMessageToGroup', this.projectid, this.createdEpic);
          this.PostEpicintoWorkspace();
        } else {
          this.epicsp.unshift(this.createdEpic);
          if (this.PrivateSpaceobject.pse[this.pseI].epics === null) {
            this.PrivateSpaceobject.pse[this.pseI].epics = [];
          }
          this.PrivateSpaceobject.pse[this.pseI].epics.push(this.createdEpicID);
          this.PostEpicintoPrivatespace(this.PrivateSpaceobject);
          this.PostEpicintoWorkspace();
        }
        console.log(this.epics);
        this.satya = [true];
        this.stories = [];
        this.EPIC = '';
        this.createdUserstoriesIds = [];
        this.createdEpic = { EpicId: '', Epic: '', userStoriesIds: [], userstories: [], status: Status,
        userstoryStatuses: [], userstorychecked: []};
        this.indexes= [true];
      });
  }
  // This method  makes backend update on Workspace collection
  PostEpicintoWorkspace() {
    this.api.postNewEpicInWorkspace(this.createdEpicID).subscribe();
  }
  // This method makes backend update on Privatespace collection
  PostEpicintoPrivatespace(epic) {
    this.api.PostEpicToPersonalSpace(epic).subscribe();
  }
  receiveMessage($event) {
    const DeletedEpicdetails = $event;
    console.log(DeletedEpicdetails);
    this.api.deleteEpic(DeletedEpicdetails.EpicId).subscribe();
    this.api.deleteNewEpicFromWorkspace(this.ProjectId, DeletedEpicdetails.EpicId).subscribe();


    if (this.calledBy === 'Workspace') {
      connection.invoke('SendMessageToGroupD', this.projectid, DeletedEpicdetails.Epic);

      this.api.getEpicsFromPersonalSpace(this.UserId)
        .subscribe((data: any) => {
          // console.log('Personal Workspace', data);
          // let DATA = data;
          console.log(data);
          const pseI = data.pse.findIndex(x => x.projectId === this.ProjectId);
          const EpicIndexPs = data.pse[pseI].epics.indexOf(DeletedEpicdetails.EpicId);
          if (EpicIndexPs !== -1) {
            console.log('Yes Epic is there in both Workspace and Private space');
            data.pse[pseI].epics.splice(EpicIndexPs, 1);
            console.log(data.pse[pseI].epics);
            this.api.PostEpicToPersonalSpace(data).subscribe();
          }
        });
    } else {
      // console.log('deleting in Personal Space');
      // console.log(typeof(DeletedEpicdetails.EpicId));
      // console.log('deleting the above epic from' , this.PrivateSpaceobject.pse[this.pseI].epics);
      const EpicIndexPs = this.PrivateSpaceobject.pse[this.pseI].epics.indexOf(DeletedEpicdetails.EpicId);
      this.PrivateSpaceobject.pse[this.pseI].epics.splice(EpicIndexPs, 1);
      // console.log('After Deleting', this.PrivateSpaceobject.pse[this.pseI].epics);
      this.api.PostEpicToPersonalSpace(this.PrivateSpaceobject).subscribe();
    }

    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < DeletedEpicdetails.userStoriesIds.length; i++) {
      this.api.deleteUserStory(DeletedEpicdetails.userStoriesIds[i]).subscribe();
    }
  }

  receiveMessage2($event) {
    const epicID = $event;
    this.moveToWorkspace_Backlog_Request(epicID);
  }
  receiveMessage3($event) {
    const epic = $event.epic;
    if ($event.status === 'workspace') {
    console.log('check' , $event.status);
    this.epics.unshift(epic);
    this.epics[0].status = $event.status;
    }
    // const index = this.epicsPR.indexOf(epic.EpicId);
    // console.log('index is' , index);
    // this.epicsPR[index].status = $event.status;
  }

  receiveMessage5($event) {
    if (this.calledBy === 'Workspace') {
      connection.invoke('SendMessageToGroupM', this.projectid, $event);
    } else {
      const i = this.epicsp.findIndex(x => x.EpicId === $event.epicId);
      if ($event.flag === false) {
        this.epicsp[i].userstories[$event.storyIndex] = $event.storyValue;
      } else {
        this.epicsp[i].userstories.splice($event.storyIndex, 1);
      }

    }
  }
  receiveMessage6($event) {
    const selectedStories = $event;
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < selectedStories.length; i++) {
      this.api.UpdateUserStoryStatus(selectedStories[i], 'Backlog').subscribe();
    }
  }
  receiveMessage7($event) {
    const selectedStories = $event;
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < selectedStories.length; i++) {
      this.api.UpdateUserStoryStatus(selectedStories[i], 'Workspace').subscribe();
    }

  }


  receiveMessage4($event) {
    if ($event.Calledby === 'Privatespace') {
      this.epicsp[$event.index].userstories[$event.sindex] = ($event.addedstory);

      this.api.createNewUserStory($event.addedstory)
        .subscribe((data: any) => {
          console.log('Newly Created User Story id', data.id);
          const addedstoryId = data.id;
          this.epicsp[$event.index].userStoriesIds[$event.sindex] = data.id;
          this.api.addUserStory($event.EpicId, data.id, data.userStoryName).subscribe();
        });
    } else {

      this.api.createNewUserStory($event.addedstory)
        .subscribe((data: any) => {
          console.log('Newly Created User Story id', data.id);
          const addedstoryId = data.id;
          this.api.addUserStory($event.EpicId, data.id, data.userStoryName).subscribe();
          // this.api.addUserStory($event.EpicId, data.id).subscribe();
          connection.invoke('SendMessageToGroupAU', this.projectid, {
            epic: $event.Epic,
            index: $event.index,
            epicId: $event.EpicId,
            addeduserstory: $event.addedstory,
            sindex: $event.sindex,
            addeduserstoryId: data.id
          });
        });
    }
  }
}