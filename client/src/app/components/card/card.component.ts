import { ApiCallsService } from 'src/app/services/api-calls.service';
import { Component, Input, Output, EventEmitter, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA, MatSnackBar, MatSnackBarConfig } from '@angular/material';

import { ActivatedRoute } from '@angular/router';
import * as signalR from '@aspnet/signalr';
import {environment} from '../../../environments/environment';
import { stringToKeyValue } from '@angular/flex-layout/extended/typings/style/style-transforms';
// tslint:disable-next-line: no-var-keyword
var connectionmaster;

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {
  clicked;
  deletedEpicDetails = { Epic: '', EpicId: '', userStoriesIds: [], index: 0 };
  projectId;
  @Input() epics ;
  @Input() Calledby;
  @Input() Role;
  @Output() messageEvent = new EventEmitter<{}>();
  @Output() messageEvent2 = new EventEmitter<{}>();
  @Output() messageEvent4 = new EventEmitter<{}>();
  @Output() messageEvent5 = new EventEmitter<{}>();
  @Output() messageEvent6 = new EventEmitter<{}>();
  @Output() messageEvent7 = new EventEmitter<{}>();

  userstory;
  UserStoryId; EpicId; AC; type; storyIndex;

  CheckedMenu = false;
  CheckedMenuD;
  disableCheckBox;
  selectedUserStories: string[] = [];
  selectedEpics: string[] = [];

  constructor(private route: ActivatedRoute, public dialog: MatDialog, private api: ApiCallsService, private snackBar: MatSnackBar) { }

  ngOnInit() {
    connectionmaster = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.starturlconnection + '/notify', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    connectionmaster
      .start()
      // tslint:disable-next-line: only-arrow-functions
      .then(function() {
        console.log('Connected!', connectionmaster);
      })
      // tslint:disable-next-line: only-arrow-functions
      .catch(function(err) {
        return console.error(err.toString());
      });
    this.projectId = this.route.snapshot.paramMap.get('projectid');

    setTimeout(() => {
        connectionmaster
          .invoke('JoinGroup', this.projectId)
          .then(res => console.log('Joined Group', this.projectId))
          .catch(err => {
            console.log('Error', err);
          });
      }, 1000);

  }

  UpdateKanbanList() {
    connectionmaster.invoke('SendUserStoryToAddOnList', this.projectId, 'success');
    connectionmaster.invoke('SendGanttToUpdate', this.projectId);
  }

  SendDatatoDialog(storyIndex, story, EpicId, UserStoryId) {
    this.EpicId = EpicId;
    this.storyIndex = storyIndex;
    this.UserStoryId = UserStoryId;
    this.api.getUserStoryC(UserStoryId).subscribe((data: any) => {
      this.userstory = data.userStoryName;
      this.AC = data.userStoryDescription;
      if (this.AC === null) {
        this.AC = [];
      }
      this.type = data.userStoryType;
      if (this.type === null) {
        this.type = 'Feature/Bug';
      }
      this.openDialog();
    });
  }
  deletepic(Epic, i, EpicId, userStoriesIds) {
    this.deletedEpicDetails.Epic = Epic;
    this.deletedEpicDetails.EpicId = EpicId;
    this.deletedEpicDetails.userStoriesIds = userStoriesIds;
    this.deletedEpicDetails.index = i;
    this.sendMessage();
  }
  // To add new story
  addnewstory(epic, epicId, newstory, Index, StoryIndex) {
    this.messageEvent4.emit({
      Epic: epic, EpicId: epicId, addedstory: newstory,
      index: Index, sindex: StoryIndex,
      Calledby: this.Calledby
    });
  }
  // To delete Message
  sendMessage() {
    if (this.Calledby === 'Privatespace') {
      console.log('Deleting Epic from front end in Private space');
      this.epics.splice(this.deletedEpicDetails.index, 1);
    }
    this.messageEvent.emit(this.deletedEpicDetails);
  }
  // To Change status of Epic
  sendEpicToChangeStatus(epic) {
    let message: string;
    if (this.Calledby === 'Workspace' && epic.status === 'workspace') {
      message = epic.Epic + ' is moved to Product Backlog';
    } else if (this.Calledby === 'Workspace' && epic.status === 'productbacklog') {
      message = epic.Epic + ' is moved Back to Workspace';
    } else {
      message = epic.Epic + ' is moved to Workspace';
    }
    const config = new MatSnackBarConfig();
    config.duration = 5000;
    config.panelClass = ['red-snackbar'];
    this.snackBar.open(message, '', config);
    console.log(epic);

    this.messageEvent2.emit(epic);
  }
CheckBoxClicked(index, EpicId, storyStatus, storyChecked, storyId) {

if (this.selectedUserStories.length === 0) {
  console.log(' The first Checked StoryStatus is ', storyStatus);
  if ( storyStatus === 'Backlog') {
  this.CheckedMenuD = 'revert';
  this.disableCheckBox = 'Workspace';
  console.log('Disable all check boxes with status ' , this.disableCheckBox);
  } else {
    this.disableCheckBox = 'Backlog';
    this.CheckedMenuD = 'move';
    console.log('Disable all check boxes with status ' , this.disableCheckBox);
  }
}

// console.log('Story Checked', storyChecked);
if (this.selectedEpics.indexOf(EpicId) === -1) {
// console.log('pushed Epic Id' , EpicId);
this.selectedEpics.push(EpicId);
}
if (storyChecked) {
  this.selectedUserStories.push(storyId);
 } else {
  const ind = this.selectedUserStories.indexOf(storyId);
  this.selectedUserStories.splice(ind, 1);
 }

if (this.selectedUserStories.length === 0) {
  this.disableCheckBox = null;
  this.CheckedMenu = false;
} else {
  this.CheckedMenu = true;
}

}

MoveSelectedToBacklog() {
  const message = 'Selected Stories are Moved to ProductBacklog ';
  const config = new MatSnackBarConfig();
  config.duration = 5000;
  config.panelClass = ['red-snackbar'];
  this.snackBar.open(message, '', config);
// console.log('Epics Geluked from MovetoBacklog', this.selectedEpics);
// console.log('GElikina stories', this.selectedUserStories);
// tslint:disable-next-line: forin
  for (const i in this.selectedEpics) {
    const index = this.epics.findIndex(x => x.EpicId === this.selectedEpics[i]);
   // console.log('Gelikina Epic index', index);
    // tslint:disable-next-line: forin
    for (const j in this.selectedUserStories) {
    const indexOfStory = this.epics[index].userStoriesIds.indexOf(this.selectedUserStories[j]);
    console.log('In that Epic Story index is ', indexOfStory);
    if ( indexOfStory !== -1) {
this.epics[index].userstoryStatuses[indexOfStory] = 'Backlog';
this.epics[index].userstorychecked[indexOfStory] = false;
    }

}
    }
  this.messageEvent6.emit(this.selectedUserStories);
  this.selectedUserStories = [];
  this.selectedEpics = [];
  this.disableCheckBox = null;
  this.CheckedMenu = false;
  }


RemoveSelectedFRomBacklog() {
  const message = 'Selected Stories are Moved Back to Workspace';
  const config = new MatSnackBarConfig();
  config.duration = 5000;
  config.panelClass = ['red-snackbar'];
  this.snackBar.open(message, '', config);
  // console.log('Epics Geluked from REvert', this.selectedEpics);
  // tslint:disable-next-line: forin
  for (const i in this.selectedEpics) {
  const index = this.epics.findIndex(x => x.EpicId === this.selectedEpics[i]);
 // console.log('Gelikina Epic index', index);
  // tslint:disable-next-line: forin
  for (const j in this.selectedUserStories) {
  const indexOfStory = this.epics[index].userStoriesIds.indexOf(this.selectedUserStories[j]);
  console.log('In that Epic Story index is ', indexOfStory);
  if ( indexOfStory !== -1) {
this.epics[index].userstoryStatuses[indexOfStory] = 'Workspace';
this.epics[index].userstorychecked[indexOfStory] = false;
  }
}
}
  this.messageEvent7.emit(this.selectedUserStories);
  this.selectedUserStories = [];
  this.selectedEpics = [];
  this.disableCheckBox = null;
  this.CheckedMenu = false;
}

  openDialog(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '35%';
    dialogConfig.data = {
      story: this.userstory,
      type: this.type,
      ac: this.AC,
      storyId: this.UserStoryId,
      epicId: this.EpicId,
      storyIndex: this.storyIndex,
      flag: false,
      Role: this.Role,
      CalledBy: this.Calledby
    };
    const dialogRef = this.dialog.open(ModalComponent, dialogConfig);
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        const object = {
          epicId: result.epicId,
          storyIndex: result.storyIndex,
          storyValue: result.story,
          flag: result.flag
        };
        this.messageEvent5.emit(object);
      }
      console.log('The dialog was closed');
    });
  }
}
@Component({
  selector: 'app-modal',
  templateUrl: 'modal.html',
  styleUrls: ['./card.component.css']
})
export class ModalComponent {
  data;
  clicked;
  constructor(
    private api: ApiCallsService, public dialogRef: MatDialogRef<ModalComponent>,
    @Inject(MAT_DIALOG_DATA) data) {
    this.data = data;
    console.log('Constructor called /data to the dialog ' + this.data.story + ' ' + this.data.type);
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
  saveModifiedUserDetails(data) {
    this.api.UpdateUSerStory(data).subscribe();
    this.dialogRef.close(data);
  }
  deleteUserStory(data) {
    data.flag = true;
    this.dialogRef.close(data);
    // console.log(data);
    this.api.removeUserStoryFromEpic(data.epicId, data.storyId, data.story).subscribe();
    this.api.deleteUserStory(data.storyId).subscribe();
  }
  deletecriteria(i) {
    this.data.ac.splice(i, 1);
  }
}
