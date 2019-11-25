import {
  Component,
  OnInit,
  AfterViewInit,
  OnChanges,
  EventEmitter,
  ElementRef,
  DoCheck,
  ViewChild,
  HostListener
} from '@angular/core';
import { MessageService } from 'primeng/api';
import { GetMessageService } from '../../services/get-message.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpClient } from '@angular/common/http';
import {MatBottomSheet, MatBottomSheetRef} from '@angular/material/bottom-sheet';
import { ActivatedRoute, Router } from '@angular/router';
import { BoardService } from '../../services/board.service';
import { DashboardService } from '../../services/dashboard.service';
import { AuthService } from '../../services/auth.service';
import { DatePipe } from '@angular/common';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem
} from '@angular/cdk/drag-drop';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { Inject } from '@angular/core';
import { DatasharingService } from 'src/app/services/datasharing.service';
import { environment } from '../../../environments/environment';
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';
import { MatRadioChange } from '@angular/material';

import * as signalR from '@aspnet/signalr';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { emitKeypressEvents } from 'readline';
import { log } from 'util';
import { link } from 'fs';

export interface DialogData {
  newuserstory: string;
  userstorydescription: string;
  userstorypoints: number;
}

export interface DialogData1 {
  userid: string;
  boardId: string;
  acceptanceCriteria: string;
  task: string;
  subtask: string;
}
export interface DialogData2 {
  // userid: string;
  columnname: string;
  // projectid: string;
}

export interface memberData {
  status: boolean;
  name: string;
  img: string;
}

export interface LinkData {
  linkCardId: string;
  boardid: string;
}

let connection;
let BoardId;

@Component({
  selector: 'app-board3',
  templateUrl: './board3.component.html',
  styleUrls: ['./board3.component.css'],
  providers: [MessageService]
})
export class Board3Component implements OnInit, DoCheck {
  constructor(
    public dialog: MatDialog,
    private messageService: MessageService,
    private boardService: BoardService,
    private dashboardService: DashboardService,
    private route: ActivatedRoute,
    private getMessageService: GetMessageService,
    private httpClient: HttpClient,
    private el: ElementRef,
    private authService: AuthService,
    private _snackbar: MatSnackBar,
    private dss: DatasharingService,
    private router: Router,
    private _bottomSheet: MatBottomSheet
  ) { }
  boardid;
  movieposter =
    'https://upload.wikimedia.org/wikipedia/commons/7/77/Icon_New_File_256x256.png';
  cardposter =
    'https://carlisletheacarlisletheatre.org/images250_/envelope-png-1.png';
  userstory = [];
  alluserstories;
  connectedTo = [];
  allLists;
  allUserData;
  allUserDataArray = [];
  tempdis = true;
  disableAddUS = false;

  newuserstory: string;
  userstorydescription: string;
  userstorystatus: string;
  userstorypoints: number;
  flagIdeaZone = false;
  acceptanceCriteria;
  task;
  subtask;
  token;
  projectDetails;
  linkCardId;
  // disablePB=false;
  membersOfBoard: Array<memberData> = [];

  columnname;

  role;
  @HostListener('document:click', ['$event'])
  clickout(event) {
    // if(this.el.nativeElement.contains(event.target)) {
    //   this.text = "clicked inside";
    // } else {
    //   this.text = "clicked outside";
    // }
    this.tempdis = true;
  }
  @HostListener('document:keydown', ['$event'])
  clickout1(event) {
    if (event.ctrlKey == true) {
      if (event.keyCode == 76) {
        event.preventDefault();
        event.stopPropagation();
        this.openDialog2();
      } else {
        const indexToAddCard = event.which - 49;
        if (
          indexToAddCard < this.allLists.length &&
          event.which > 48 &&
          event.which < 58
        ) {
          event.preventDefault();
          event.stopPropagation();
          // this.allLists.length[indexToAddCar
          this.addCard(this.allLists[indexToAddCard]);
        }
      }
    }
  }

  ngDoCheck() {
    this.allLists.forEach(x => {
      x.userStory.forEach(y => {
        if (y.userStoryName === '') {
          const h = document.getElementById(y.userStoryId);
          if (h !== null) {
            h.focus();
          }
        }
      });
    });
  }
  ngOnInit() {
    this.getId();
    // console.log(this.boardid);
    connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.starturlconnection + '/notify', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    connection
      .start()
      .then(function() {
        console.log('Connected!', connection);
      })
      .catch(function(err) {
        return console.error(err.toString());
      });
    connection.on('ReceiveMessage', message => {
      // console.log(message);
      this.allLists.forEach(x => {
        if (x.name === message.name) {
          x.userStory.push(message.userStory);
        }
      });
      // console.log("received message", this.allLists);
    });



    connection.on('RabbitListUpdate', message => {
      setTimeout(() => {
        console.log(message);
        this.boardService.getFullList(this.boardid).subscribe(x => {
          this.allLists = x;
          // console.log(this.allLists);
          this.connectedTo = [];
          for (const l in this.allLists) {
            this.connectedTo.push(this.allLists[l].name);
          }
        });
      }, 1000);
    });


    // connection.on("RabbitListUpdate", message => {
    //   let allListsReceived = [...this.allLists];
    //   // this.boardService.getFullList(this.boardid).subscribe(x => {
    //   //   allListsReceived = x;
    //   //   // console.log(this.allLists);
    //   // });
    //   while (allListsReceived[0].userStory.length == this.allLists[0].userStory.length) {

    //     setTimeout(() => {
    //       this.boardService.getFullList(this.boardid).subscribe(x => {
    //         allListsReceived = x;

    //       });
    //     }, 500);}

    //     this.allLists = allListsReceived;


    // });

    connection.on('ReceiveForAddMembers', message => {
      this.dashboardService.getUserById(message.memberId).subscribe(x => {
        const obj = {
          status: message.status,
          name: x.username,
          img: message.img
        };
        this.membersOfBoard.push(obj);
      });
    });
    connection.on('ReceiveForRemoveMembers', message => {
      let i = 0;
      this.dashboardService.getByUserName(message.memberId).subscribe(x => {
        this.membersOfBoard.forEach(mem => {
          if (mem.name == x.username) {
            this.membersOfBoard.splice(i, 1);
          }
          i++;
        });
        if (this.token.id == message.memberId) {
          this.router.navigate(['/dashboard']);
        }
      });
    });

    connection.on('ReceiveForTransferMembers', message => {
      this.projectDetails.members = message.members;
      this.projectDetails.owner = message.owner;
      this.dashboardService.getUserById(message.memberId).subscribe(x => {
        this.membersOfBoard.forEach(mem => {
          if (mem.name == x.username) {
            mem.status = message.status;
          }
        });
      });
    });
    connection.on('ReceiveMessageEditedUs', message => {
      // console.log(message);
      // console.log(this.allLists);
      this.allLists.forEach(x => {
        if (x.name === message.name) {
          x.userStory.forEach(y => {
            if (y.userStoryId === message.userStory.userStoryId) {
              y.userStoryName = message.userStory.userStoryName;
            }
          });
        }
      });
    });

    connection.on('ReceiveMessageList', message => {
      this.allLists.forEach(x => {
        if (x.name === message.sourceName) {
          x.userStory = message.sourceList;
        } else if (x.name === message.destinationName) {
          x.userStory = message.destinationList;
        }
      });
    });
    connection.on('ReceiveColumnName', message => {
      // console.log(message);

      this.allLists.push(message);
      this.connectedTo.push(message.name);
    });

    connection.on('ReceiveMessageEditedListName', message => {
      // console.log(message);
      // console.log(this.allLists);
      this.connectedTo.push(message.listName);
      this.allLists.forEach(x => {
        if (x.id === message.id) {
          // console.log('matched');
          x.name = message.listName;
        }
      });
    });
    connection.on('ReceiveColumnToDelete', message => {
      const obj4 = [];
      // console.log("message",message);
      for (const i in this.allLists) {
        if (this.allLists[i].id !== message) {
          obj4.push(this.allLists[i]);
        }
      }
      this.allLists = obj4;
      // console.log("obj4", obj4, this.allLists);
    });

    connection.on('ReceiveUpdatedList', message => {
      this.allLists = message;
      console.log(this.allLists);
    });

    connection.on('ReceiveFromModalToBoard', message => {
      console.log(this.allLists);
      console.log(message);
      let listChanged;
      this.allLists.forEach(x => {
        if (x.id === message.listId) {
          x.userStory.forEach(y => {
            if (y.userStoryId === message.userStoryId) {
              y.userStoryName = message.userStoryName;
              listChanged = x;
            }
          });
        }
      });
      console.log(this.allLists);
      this.boardService.putList(listChanged).subscribe(x => {
        console.log('put in signalr');
      });
    });

    setTimeout(() => {
      connection
        .invoke('JoinGroup', this.boardid)
        .then(res => console.log('Joined Group', this.boardid))
        .catch(err => {
          console.log('Error', err);
        });
    }, 1000);

    this.boardService.getFullList(this.boardid).subscribe(x => {
      this.allLists = x;
      // console.log(this.allLists);
      for (const l in this.allLists) {
        this.connectedTo.push(this.allLists[l].name);
      }
    });

    this.token = this.authService.jwtToken().decodedToken;
    this.boardService.getAllUserData().subscribe(x => {
      this.allUserData = x;
      // console.log(this.allUserData);
      this.allUserData.forEach(y => {
        if (this.token.id != y.id && y.username != null) {
          this.allUserDataArray.push(y.username);
        }
        // console.log(this.allUserDataArray);
      });
    });


    this.boardService.getUSByProjectId(this.boardid).subscribe(x => {
      console.log(x);
      x.forEach(us => {
        if (us.linkedToId == us.uniqueId) {
          this.disableAddUS = true;
        }
      });
    });
    this.dashboardService.getProjectById(this.boardid).subscribe(x => {
      this.projectDetails = x;
      this.projectDetails.members.forEach(y => {
            this.dashboardService.getUserById(y).subscribe(z => {
              const obj1 = {
                status: true,
                name: z.username,
                img: z.avatar_url
              };
              this.membersOfBoard.push(obj1);
            });
          });
        });
    this.dashboardService.getProjectById(this.boardid).subscribe(p => {
        p.owner.forEach(o => {
          this.dashboardService.getUserById(o).subscribe(u => {
            const obj1 = {
              status: false,
              name: u.username,
              img: u.avatar_url
            };
            this.membersOfBoard.push(obj1);
          });
        });
      });

  }

  ngOnDestroy() {
    console.log('board destroyed in seconds');
    let i = 0;
    this.allLists.forEach(element => {
      console.log(element);
      element.index = i;
      i += 1;
      this.boardService.putList(element).subscribe(x => {
        console.log('success put', x);
      });
    });
  }

  getId() {
    // this.boardid = this.route.snapshot.params['boardid'];
    this.boardid = this.route.snapshot.paramMap.get('boardid');
    BoardId = this.boardid;
    // console.log(this.route.snapshot.routeConfig.path);

    // console.log(this.route.snapshot.paramMap.get('boardid'));
    // console.log("boardid" + this.boardid);
  }

  deleteColumn(l): void {
    let flag = false;
    if (l.userStory.length == 0) {
      if (confirm('Are you sure you want to delete this column?')) {
        this.boardService.deleteProjectFromList(l.id).subscribe(x => {
          connection.invoke('SendColumnToDelete', this.boardid, l.id);
        });
      }
    }
    let j = 0;
    l.userStory.forEach(x => {
      if (flag == false) {
        this.boardService.getUSById(x.userStoryId).subscribe(us => {
          if (us.linkedToId == us.uniqueId) {
            flag = true;
          }
          if (flag == false && j == 0) {
            if (confirm('Are you sure you want to delete this column?')) {
              j++;
              for (const i in l.userStory) {
                this.boardService
                  .deleteUserStory(l.userStory[i].userStoryId)
                  .subscribe();
              }
              this.boardService.deleteProjectFromList(l.id).subscribe(x => {
                connection.invoke('SendColumnToDelete', this.boardid, l.id);

                // this
              });
            }
          } else if (flag == true) {
            this._snackbar.open('Move main cards to delete this column', '', {duration: 2000});
          }
        });
      }
    });
  }

  deletecard(list, l): void {
    let flag = false;
    this.boardService.getUSById(list.userStoryId).subscribe(x => {
      if (x.uniqueId == x.linkedToId) {
        flag = true;
      } else {
        flag = false;
      }
      if (flag == false) {
        if (list.userStoryName != '') {
          if (confirm('Are you sure you want to delete this user story?')) {
            this.deleteCardCondition(list, l);
          }
        } else {
          this.deleteCardCondition(list, l);
        }
      } else {
        this._snackbar.open('This card cannot be deleted', '', {
          duration: 2000
        });
      }
    });
  }

  deleteCardCondition(list, l) {
    this.boardService.getUSById(list.userStoryId).subscribe(z => {
      const linkedid = z.linkedToId;

      this.boardService.deleteUserStory(list.userStoryId).subscribe(() => {
        let progress;
        this.boardService.getByLinkedId(linkedid).subscribe(a => {
          const linkedUserStories = a;
          progress = this.calculateProgress(linkedUserStories);
          let parentUserStory;
          this.boardService.getByUniqueId(linkedid).subscribe(b => {
            parentUserStory = b;
            parentUserStory.progress = progress;
            console.log(progress, 'delete');
            this.boardService.putUserStories(parentUserStory).subscribe(c => {
              let updatedAllLists;

              for (const i in this.allLists) {
                if (this.allLists[i].id === l.id) {
                  const obj2 = [];
                  for (const j in this.allLists[i].userStory) {
                    if (
                      this.allLists[i].userStory[j].userStoryId !==
                      list.userStoryId
                    ) {
                      obj2.push(this.allLists[i].userStory[j]);
                    }
                  }
                  this.allLists[i].userStory = obj2;
                  updatedAllLists = this.allLists[i];
                  break;
                }
              }
              const obj3 = {
                sourceName: l.name,
                destinationName: l.name,
                sourceList: l.userStory,
                destinationList: l.userStory
              };
              this.boardService.putList(updatedAllLists).subscribe(x => {
                connection.invoke('SendMessageListToGroup', this.boardid, obj3);
              });
            });
          });
        });
      });
    });
  }

  calculateProgress(linkedUserStories) {
    let noOfStories = 0;
    let innerProgress = 0,
      progress = 0,
      count = 0,
      noOfSub = 0,
      noOfTask = 0;
    linkedUserStories.forEach(y => {
      y.tasks.forEach(z => {
        z.subtask.forEach(sub => {
          if (sub.subtaskStatus == 'true') {
            count++;
          }
          noOfSub++;
        });
        noOfTask++;
      });
      if (noOfSub != 0) {
        innerProgress = count / noOfSub;
        noOfStories++;
      }
      count = 0;
      if (noOfSub != 0) {
        progress = progress + innerProgress;
      }
      noOfTask = 0;
      noOfSub = 0;
    });
    if (noOfStories != 0) {
      progress = (progress / noOfStories) * 100;
    }
    return progress;
  }


  addCard(list) {
    console.log(list);
    const us = {
      description: null,
      shortName: '',
      projectId: list.projectId,
      userId: null,
      status: list.name,
      startTime: new Date(),
      endTime: new Date(),
      linkedToId: '',
      assignedTo: [],
      acceptanceCriteria: [],
      tasks: [],
      points: 0,
      epicId: null,
      epicTitle: null
    };
    let projectSpecificUS;
    this.boardService.getUSByProjectId(this.boardid).subscribe(a => {
      projectSpecificUS = a;
      console.log(projectSpecificUS);
      let flag = true;
      projectSpecificUS.forEach(b => {
        if (flag == true) {
          if (b.uniqueId == b.linkedToId) {
            us.linkedToId = b.linkedToId;
            flag = false;
          }
        }
      });
      // let linkid=this.openDialogForLink();
      // us.linkedToId=linkid;
      this.boardService.postUserStories(us).subscribe(x => {
        const obj = {
          userStoryId: x.id,
          userStoryName: x.shortName
        };
        const list1 = JSON.parse(JSON.stringify(list));
        list1.userStory.push(obj);
        this.boardService.putList(list1).subscribe(y => {
          const msg = {
            name: list.name,
            UserStory: {
              UserStoryId: x.id,
              UserStoryName: x.shortName
            }
          };
          connection.invoke('SendMessageToGroup', this.boardid, msg);
        });
      });
    });
  }

  openDialogForLink(): string {
    const dialogRef = this.dialog.open(LinkComponent, {
      width: '250px',
      data: {linkCardId: this.linkCardId, boardid: this.boardid}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.linkCardId = result;
      return this.linkCardId;
    });
    return '';
  }
  delCardIfEmpty(userstory, l) {
    if (userstory.userStoryName === '') {
      this.deletecard(userstory, l);
      this.tempdis = true;
    }
  }

  delCardCondition(list, us) {
    // console.log("esc presed");
    let countUserStory = 0,
      i = 0;
    list.userStory.forEach(x => {
      countUserStory += 1;
    });

    list.userStory.forEach(x => {
      i += 1;
      if (
        x.userStoryId === us.userStoryId &&
        countUserStory === i &&
        us.userStoryName === ''
      ) {
        this.tempdis = true;
        this.deletecard(us, list);
      }
    });
  }

  addCardCondition(list, us) {
    let countUserStory = 0,
      i = 0;
    list.userStory.forEach(x => {
      countUserStory += 1;
    });
    list.userStory.forEach(x => {
      i += 1;
      if (x.userStoryId === us.userStoryId && countUserStory === i) {
        this.tempdis = false;
        this.addCard(list);
      }
    });
  }

  thisfunc() {
    console.log('this func happends');
  }

  onBlurUpdate(changedus, clickedlist, clickedus) {
    // console.log("this is flag");console.log(flag3);
    // if(flag3===true)
    // {this.tempdis=true;}
    // else {this.tempdis=false;}
    // console.log(clickedlist);
    // console.log(clickedus.userStoryId);
    clickedlist.userStory.forEach(x => {
      if (x.userStoryId === clickedus.userStoryId) {
        x.userStoryName = changedus;
      }
    });
    let clickedUserStoryFull;
    this.boardService.putList(clickedlist).subscribe(x => {
      console.log(x);
      console.log('list updated');
    });

    this.boardService.getUSById(clickedus.userStoryId).subscribe(x => {
      clickedUserStoryFull = x;
      console.log(clickedUserStoryFull);
      clickedUserStoryFull.shortName = changedus;
      // this.allLists.forEach(x=>{

      // })
      this.boardService.putUserStories(clickedUserStoryFull).subscribe(x => {
        console.log(clickedUserStoryFull);
        const obj = {
          name: clickedlist.name,
          UserStory: {
            UserStoryId: clickedUserStoryFull.id,
            UserStoryName: clickedUserStoryFull.shortName
          }
        };
        console.log(obj);
        connection.invoke('SendMessageToGroupForUsEdit', this.boardid, obj);
        connection.invoke('SendGanttToUpdate', this.boardid);
      });
    });
  }

  onBlurList(changedlname, clickedlist) {
    clickedlist.name = changedlname;
    this.boardService.putList(clickedlist).subscribe(x => console.log('put'));
    this.connectedTo.push(changedlname);
    // console.log(changedlname);
    // console.log(clickedlist);
    const obj = {
      id: clickedlist.id,
      listName: clickedlist.name
    };
    connection.invoke('SendMessageToGroupForListNameEdit', this.boardid, obj);
  }

  openDialog1(clickedus): void {
    const dialogRef = this.dialog.open(CardOverviewDialog1, {
      width: '40em',

      data: {
        userid: clickedus.userStoryId,
        boardId: this.boardid,
        acceptanceCriteria: this.acceptanceCriteria,
        task: this.task,
        subtask: this.subtask
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.userstory = result;
    });
  }
  openDialog2(): void {
    // console.log(list);
    const dialogRef = this.dialog.open(CreateColumn, {
      // width: "250px",
      data: {
        columnname: this.columnname
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('dialog was closer');
      // console.log(result, "this is dialog2");
      const addcolumn = {
        name: result.columnname,
        userStory: [],
        projectId: this.boardid,
        index: -1
      };
      this.connectedTo.push(result.columnname);
      this.boardService.postColumn(addcolumn).subscribe(x => {
        // console.log("column posted",this.allLists);
        const addcolumnWithId = {
          name: result.columnname,
          userStory: [],
          projectId: this.boardid,
          id: x.id,
          index: x.index
        };
        // this.allLists.push(addcolumnWithTempId);
        // console.log("all_list",this.allLists);
        // console.log("returned from post",x);
        connection.invoke('SendNewColumnName', this.boardid, addcolumnWithId);
      });
      // connection.invoke("SendMessageToGroup", this.boardid, msg);
    });
  }
  drop1(event: CdkDragDrop<any[]>) {
    moveItemInArray(this.allLists, event.previousIndex, event.currentIndex);
    console.log(event, 'events');
    console.log(this.allLists, 'alllists'); // working fine
    connection.invoke(
      'SendUpdatedPositionOfColumns',
      this.boardid,
      this.allLists
    );
  }

  drop(event: CdkDragDrop<any[]>) {
    // console.log(l_changed);
    // console.log(event);
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }

    const obj = {
      sourceName: event.previousContainer.id,
      destinationName: event.container.id,
      sourceList: event.previousContainer.data,
      destinationList: event.container.data
    };

    for (const x in this.allLists) {
      // console.log(this.allLists[x]);
      if (this.allLists[x].name === obj.sourceName) {
        this.boardService
          .putList(this.allLists[x])
          .subscribe(x => console.log('updated listdb')); // all list put
      } else if (this.allLists[x].name === obj.destinationName) {
        this.boardService.putList(this.allLists[x]).subscribe();
      }
    }
    connection.invoke('SendMessageListToGroup', this.boardid, obj);
  }
  addMembers(member) {
    let flagrole = false;
    this.projectDetails.owner.forEach(o => {
      if (this.projectDetails.owner == this.token.id) {
        flagrole = true;
      }
    });
    if (flagrole) {
      let flag = false;
      this.allUserDataArray.forEach(mem => {
        if (mem === member._searchStr) {
          flag = true;
        }
      });
      if (flag === false) {
        // alert("Username not found");
        this._snackbar.open('Username not found', '', {duration: 2000});
      } else {
        let flag2 = false;
        let fulldetails;
        this.dashboardService
          .getByUserName(member._searchStr)
          .subscribe(user => {
            fulldetails = user;
            console.log(fulldetails);
            this.projectDetails.members.forEach(z => {
              if (fulldetails.id == z) {
                flag2 = true;
              }
            });
            if (flag2 == false) {
              let details;
              // console.log("radio",this.role);
              this.dashboardService
                .getByUserName(member._searchStr)
                .subscribe(u => {
                  details = u;
                  this.boardService
                    .sendmail(details.id, this.boardid, this.role)
                    .subscribe();
                  // alert('Request has been sent');
                  this._snackbar.open('Request has been sent', '', {
                    duration: 2000
                  });
                });
            } else {
              // alert("Member already exists!!");
              this._snackbar.open('Member already exists', '', {duration: 2000});
            }
          });
      }
    } else {
      // alert("You cannot add members to this board!");
      this._snackbar.open('You cannot add members to this board', '', {duration: 2000});
    }
  }

  removeMember(member) {
    // console.log(member);
    let toRemoveMember;
    let flagrole = false, flag = true;
    this.projectDetails.owner.forEach(o => {
      if (o == this.token.id) {
        flagrole = true;
      }
    });
    this.dashboardService.getByUserName(member.name).subscribe(x => {
      toRemoveMember = x;
      // console.log(toRemoveMember);
      if (toRemoveMember.id == this.token.id) {
        flagrole = true;
      }
      if (this.projectDetails.owner.length == 1 && this.projectDetails.owner[0] == toRemoveMember.id) {
        flag = false;
        this._snackbar.open('Transfer ownership to remove yourself', '', {duration: 2000});
      }
      if (flagrole && flag) {
        if (confirm('Are you sure you want to remove the member?')) {
          let i = 0, j = 0, k = 0, l = 0;
          this.membersOfBoard.forEach(b => {
            if (b.name == toRemoveMember.username) {
              this.membersOfBoard.splice(k, 1);
            }
            k++;
          });
          console.log('members of board', this.membersOfBoard);
          toRemoveMember.projectDetails.forEach(pd => {
            if (pd.projectId == this.projectDetails.id) {
              toRemoveMember.projectDetails.splice(j, 1);
            }
            j++;
          });
          console.log('to remove member', toRemoveMember);
          this.dashboardService.putUser(toRemoveMember).subscribe(x => {
            console.log('put');
            const obj = {
              memberId: toRemoveMember.id,
              status: false,
              img: member.img
            };
            connection.invoke('SendForRemoveMembers', this.boardid, obj);
          });
          this.projectDetails.owner.forEach(o => {
            if (o == toRemoveMember.id) {
              this.projectDetails.owner.splice(i, 1);
            }
            i++;
          });
          this.projectDetails.members.forEach(mem => {
            if (mem == toRemoveMember.id) {
              this.projectDetails.members.splice(l, 1);
            }
            l++;
          });
          console.log('project details', this.projectDetails);
          this.dashboardService.putProject(this.projectDetails).subscribe(() => console.log('put project'));
        }
      } else if (flagrole == false) {
        this._snackbar.open('You cannot remove members of this board', '', {duration: 2000});
      }
    });
  }
  transfer(member) {
    let transferMem;
    let flag = false;
    this.projectDetails.owner.forEach(owner => {
      if (owner == this.token.id) {
        flag = true;
      }
    });
    if (flag) {
      this.dashboardService.getByUserName(member.name).subscribe(x => {
        transferMem = x;
        this.membersOfBoard.forEach(mem => {
          if (member == mem.name) {
            mem.status = false;
          }
        });
        const hide = (document.getElementById(member.name) as HTMLInputElement);
        hide.style.display = 'none';
        this._snackbar.open(member.name + ' is made an owner of this board', '', {duration: 2000});
        // console.log(transferMem,"transfermem");
        // console.log(this.projectDetails,"pro");
        transferMem.projectDetails.forEach(pro => {
          if (this.projectDetails.id == pro.projectId) {
            pro.role = 'Owner';
          }
        });
        console.log(transferMem);
        this.dashboardService.putUser(transferMem).subscribe();
        let i = 0;
        this.projectDetails.members.forEach(mem => {
          if (mem == transferMem.id) {
            this.projectDetails.members.splice(i, 1);
          }
          i++;
        });
        this.projectDetails.owner.push(transferMem.id);
        console.log(this.projectDetails);
        this.dashboardService.putProject(this.projectDetails).subscribe(() => {
          const obj = {
            memberId: transferMem.id,
            status: false,
            projectId: this.projectDetails.id,
            members: this.projectDetails.members,
            owner: this.projectDetails.owner
          };
          connection.invoke('SendForTransferMembers', this.boardid, obj);
        });
      });
    } else {
      this._snackbar.open('You cannot set owners of this board', '', {duration: 2000});
    }
  }
  openBottomSheet(): void {
    this._bottomSheet.open(Activity);
  }
}

@Component({
  selector: 'app-activity',
  templateUrl: 'activity.html',
})
export class Activity implements OnInit {
  constructor(private bottomSheetRef: MatBottomSheetRef<Activity>, private boardService: BoardService, private route: ActivatedRoute) {}
  activity;
  activityreverse;
  ngOnInit(): void {
    // const id=this.route.snapshot.paramMap.get('boardid');
    console.log(BoardId);
    this.boardService.getActivity(BoardId).subscribe(x => {
      this.activity = x;
      // console.log(this.activity);
      this.activityreverse = this.activity.reverse();
      this.activityreverse.forEach(ar=>{
        ar.published = (new Date(ar.published)).toString();
      });
      // (new Date(this.activityreverse[i].published)).toString()
      // console.log(this.activity_reverse);
    });
  }
  openLink(event: MouseEvent): void {
    this.bottomSheetRef.dismiss();
    event.preventDefault();
  }
}

@Component({
  selector: 'link',
  templateUrl: 'link.html',
})
export class LinkComponent {

  constructor(
    public dialogRef: MatDialogRef<LinkComponent>,
    @Inject(MAT_DIALOG_DATA) public data: LinkData,
    private boardService: BoardService) {}
    dataSource = [];
    ngOnInit() {
      console.log(this.data.boardid);
      this.boardService.getUSByProjectId(this.data.boardid).subscribe(x => {
        if (x.linkedToId == x.uniqueId) {
          const obj = {
            id: x.id,
            name: x.shortName
          };
          this.dataSource.push(obj);
        }

        // console.log(x);
      });
    }

  onNoClick(): void {
    this.dialogRef.close();
  }

}

@Component({
  selector: "createcolumn",
  templateUrl: 'createcolumn.html',
  styleUrls: ['./createcolumn.css']
})
// modal for adding new column (product backlog)
export class CreateColumn {
  usinput: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<CreateColumn>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData2,
    private fb: FormBuilder
  ) {
    this.usinput = fb.group({
      colname: new FormControl()
    });
  }

  onNoClick() {
    this.dialogRef.close();
  }
  sendData() {
    this.dialogRef.close(this.data);
  }
}

@Component({
  selector: "dialog-overview-example3-dialog",
  templateUrl: 'dialog-overview-example3-dialog.html'
})
// modal for adding new user story
export class DialogOverviewExample3Dialog {
  usinput: FormGroup;
  constructor(
    public dialogRef: MatDialogRef<DialogOverviewExample3Dialog>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private fb: FormBuilder
  ) {
    this.usinput = fb.group({
      usname: '',
      usdes: ''
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

@Component({
  selector: "card-overview-dialog",
  templateUrl: 'card-overview-dialog.html',

  styleUrls: ['./card-overview-dialog.css']
})
// modal for task and acceptance criteria
export class CardOverviewDialog1 implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<CardOverviewDialog1>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData1,
    private boardService: BoardService,
    private _snackbar: MatSnackBar,
    private dashboardService: DashboardService,
    private fb: FormBuilder
  ) {
    this.taskform = fb.group({
      newtask: ''
    });
    this.accform = fb.group({
      newac: ''
    });
    // this.subtaskform = this.createGroup();
  }
  accform: FormGroup;
  taskform: FormGroup;
  subtaskform: FormGroup;
  connection1;

  points = [1, 2, 3];
  clickedus;
  fullList;

  fetchedUserStory;

  startDateDisplay;
  endDateDisplay;

  allUserStoriesList = [];
  project;
  membersOfTheBoard = [];
  membersArray = [];
  assignedToArray = [];


  linkedToCard;
  createGroup() {
    const group = this.fb.group({});
    // console.log(this.clickedus.tasks);
    this.clickedus.tasks.forEach(control => {
      group.addControl(control.taskId, this.fb.control(''));
      console.log(control);
    });
    return group;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  ngOnInit() {
    // console.log(connection,"b3 connection");
    connection.on('ReceiveModalDetails', message => {
      this.clickedus.shortName = message.usname;
      this.clickedus.points = message.points;
      this.clickedus.acceptanceCriteria = message.acceptanceCriteria;
      this.clickedus.tasks = message.modalTasks;
      this.clickedus.assignedTo = message.assignedTo;
      this.linkedToCard.shortName = message.shortName;
      let j;
      for (const i in this.clickedus.tasks) {
        j = this.clickedus.tasks[i];
        console.log(this.clickedus.tasks[i]);
      }
      this.subtaskform.addControl(j.taskId, new FormControl(''));
    });

    this.boardService.getUSById(this.data.userid).subscribe(x => {
      this.clickedus = x;
      this.boardService
        .getUSByProjectId(this.clickedus.projectId)
        .subscribe(x => {
          x.forEach(y => {
            if (y.uniqueId == y.linkedToId) {
              this.allUserStoriesList.push(y);
            }
          });
          console.log(this.allUserStoriesList);
          this.dashboardService
            .getProjectById(this.clickedus.projectId)
            .subscribe(pro => {
              this.project = pro;
              // this.membersOfTheBoard.push(this.project.owner);
              this.project.owner.forEach(o => {
                this.membersOfTheBoard.push(o);
              });
              this.project.members.forEach(mem => {
                this.membersOfTheBoard.push(mem);
              });
              this.membersOfTheBoard.forEach(member => {
                this.dashboardService.getUserById(member).subscribe(mem => {
                  const obj = {
                    id: mem.id,
                    name: mem.username
                  };
                  // console.log(obj);
                  this.membersArray.push(obj);
                  this.boardService.getByUniqueId(this.clickedus.linkedToId).subscribe(x => {
                    this.linkedToCard = x;
                  });
                });
              });
            });
        });

      this.subtaskform = this.createGroup();
      this.startDateDisplay = this.clickedus.startTime.slice(0, 10);
      this.endDateDisplay = this.clickedus.endTime.slice(0, 10);
    });

    this.boardService.getFullList(this.data.boardId).subscribe(x => {
      this.fullList = x;
      console.log(this.fullList);
    });

    connection.on('ReceiveMessageDelete', message => {
      this.clickedus.shortName = message.usname;
      this.clickedus.points = message.points;
      this.clickedus.acceptanceCriteria = message.acceptanceCriteria;
      this.clickedus.tasks = message.modalTasks;
      this.clickedus.assignedTo = message.assignedTo;
      this.linkedToCard.shortName = message.shortName;
    });

    setTimeout(() => {
      connection
        .invoke('JoinGroup', this.clickedus.id)
        .then(res => console.log('Joined Group', this.clickedus.id))
        .catch(err => {
          console.log('Error', err);
        });
    }, 1000);
  }

  changeUsName(changedValue) {
    console.log(changedValue);
    this.clickedus.shortName = changedValue;
    this.boardService.putUserStories(this.clickedus).subscribe(x => {
      console.log('put');
      console.log(this.clickedus);
    });
    let listChanged;
    this.fullList.forEach(x => {
      x.userStory.forEach(y => {
        if (y.userStoryId === this.clickedus.id) {
          y.userStoryName = changedValue;
          listChanged = x;
        }
      });
    });
    console.log(listChanged);
    // this.boardService
    //   .putList(listChanged)
    //   .subscribe(x => {console.log("list put")
    const obj1 = {
      id: this.clickedus.id,
      usname: this.clickedus.shortName,
      points: this.clickedus.points,
      acceptanceCriteria: this.clickedus.acceptanceCriteria,
      modalTasks: this.clickedus.tasks,
      assignedTo: this.clickedus.assignedTo,
      shortName: this.linkedToCard.shortName
    };

    connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj1);

    const obj2 = {
      listId: listChanged.id,
      listName: listChanged.name,
      userStoryId: this.clickedus.id,
      userStoryName: changedValue
    };
    connection.invoke('SendFromModalToBoard', this.data.boardId, obj2);
    connection.invoke('SendGanttToUpdate', this.data.boardId);
    // });
  }

  addac() {
    const acReceived = (document.getElementById('ac') as HTMLInputElement)
      .value;

    this.boardService.getUSById(this.clickedus.id).subscribe(x => {
      this.clickedus = x;
      this.clickedus.acceptanceCriteria.push(acReceived);
      this.boardService.putUserStories(this.clickedus).subscribe(x => {
        // console.log("put ac");
        const obj1 = {
          id: this.clickedus.id,
          usname: this.clickedus.shortName,
          points: this.clickedus.points,
          acceptanceCriteria: this.clickedus.acceptanceCriteria,
          modalTasks: this.clickedus.tasks,
          assignedTo: this.clickedus.assignedTo,
          shortName: this.linkedToCard.shortName
        };
        // console.log("this is modal obj1");
        // console.log(obj1);

        connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj1);
      });
    });
  }

  deleteac(clickedac) {
    // console.log(this.clickedus.acceptanceCriteria);
    let i = 0;
    this.clickedus.acceptanceCriteria.forEach(x => {
      if (x === clickedac) {
        this.clickedus.acceptanceCriteria.splice(i, 1);
      }
      i++;
    });
    // console.log(this.clickedus.acceptanceCriteria);
    this.boardService.putUserStories(this.clickedus).subscribe(x => {
      const obj = {
        id: this.clickedus.id,
        usname: this.clickedus.shortName,
        points: this.clickedus.points,
        acceptanceCriteria: this.clickedus.acceptanceCriteria,
        modalTasks: this.clickedus.tasks,
        assignedTo: this.clickedus.assignedTo,
        shortName: this.linkedToCard.shortName
      };
      connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj);
    });
  }

  addtask() {
    // console.log(this.clickedus);
    const taskReceived = (document.getElementById('task') as HTMLInputElement)
      .value;
    const uuidv1 = require('uuid/v1');
    const x = uuidv1();
    console.log(x);
    this.subtaskform.addControl(x, new FormControl(''));
    const taskCreated = {
      taskId: x,
      taskName: taskReceived,
      assigneeId: [],
      subtask: []
    };

    console.log(this.clickedus);
    this.boardService.getUSById(this.clickedus.id).subscribe((x: any) => {
      this.clickedus = x;
      this.clickedus.tasks.push(taskCreated);
      this.boardService.putUserStories(this.clickedus).subscribe(() => {
        let progress;
        this.boardService.getByLinkedId(this.clickedus.linkedToId).subscribe(us => {
          const linkedToCards = us;
          progress = this.calculateProgress(linkedToCards);
          console.log(progress);
          this.clickedus.progress = progress;
          this.boardService.getByUniqueId(this.clickedus.linkedToId).subscribe(p => {
            p.progress = progress;
            this.boardService.putUserStories(p).subscribe(() => {
              this.boardService.putUserStories(this.clickedus).subscribe(() => {
                const obj = {
                  id: this.clickedus.id,
                  usname: this.clickedus.shortName,
                  points: this.clickedus.points,
                  acceptanceCriteria: this.clickedus.acceptanceCriteria,
                  modalTasks: this.clickedus.tasks,
                  assignedTo: this.clickedus.assignedTo,
                  shortName: this.linkedToCard.shortName
                };
                connection.invoke('SendModalDetailsToUsId', this.clickedus.id, obj);
                connection.invoke('SendGanttToUpdate', BoardId);
              });
            });
          });
        });
      });
    });
  }

  deletetask(task) {
    // console.log(this.clickedus.tasks);
    // console.log(task);
    let i = 0;
    // this.clickedus.tasks.forEach(x => {
    //   if (x.taskId === task.taskId) {
    //     this.clickedus.tasks.splice(i, 1);
    //   }
    //   i++;
    // });

    let progress;
    // console.log(this.clickedus,"outer");
    // console.log(this.clickedus.tasks);
    this.boardService.getUSById(this.clickedus.id).subscribe(us => {
      const fetchedUS = us;
      fetchedUS.tasks.forEach(t => {
        if (t.taskId == task.taskId) {
          fetchedUS.tasks.splice(i, 1);
        }
        i++;
      });
      this.clickedus = fetchedUS;
      this.boardService.putUserStories(this.clickedus).subscribe(x => {
        this.boardService.getByLinkedId(this.clickedus.linkedToId).subscribe(us => {
          const linkedToCards = us;
          progress = this.calculateProgress(linkedToCards);
          console.log(progress);
          this.clickedus.progress = progress;
          this.boardService.getByUniqueId(this.clickedus.linkedToId).subscribe(p => {
            p.progress = progress;
            this.boardService.putUserStories(p).subscribe(() => {
              console.log(this.clickedus, 'inner');
              this.boardService.putUserStories(this.clickedus).subscribe(() => {
                const obj = {
                  id: this.clickedus.id,
                  usname: this.clickedus.shortName,
                  points: this.clickedus.points,
                  acceptanceCriteria: this.clickedus.acceptanceCriteria,
                  modalTasks: this.clickedus.tasks,
                  assignedTo: this.clickedus.assignedTo,
                  shortName: this.linkedToCard.shortName
                };
                connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj);
                connection.invoke('SendGanttToUpdate', BoardId);
              });
            });
          });
        });
      });
    });
  }

  addsubtask(clickedtask) {
    // console.log(clickedtask);
    const subtaskReceived = (document.getElementById(
      clickedtask.taskId
    ) as HTMLInputElement).value;
    // console.log(subtaskReceived);
    const uuidv1 = require('uuid/v1');
    const x = uuidv1();
    // console.log(x);
    const subtaskCreated = {
      subtaskId: x,
      subtaskStatus: 'false',
      subtaskDescription: subtaskReceived
    };

    this.boardService.getUSById(this.clickedus.id).subscribe(a => {

      const fetchedUS = a;
      fetchedUS.tasks.forEach(t => {
        if (t.taskId == clickedtask.taskId) {
          t.subtask.push(subtaskCreated);
        }
      });
      console.log(fetchedUS);
      this.clickedus = fetchedUS;
      let progress;
      this.boardService.putUserStories(this.clickedus).subscribe(x => {
        this.boardService.getByLinkedId(this.clickedus.linkedToId).subscribe(us => {
          const linkedToCards = us;
          progress = this.calculateProgress(linkedToCards);
          console.log(progress);
          this.clickedus.progress = progress;
          this.boardService.getByUniqueId(this.clickedus.linkedToId).subscribe(p => {
            p.progress = progress;
            this.boardService.putUserStories(p).subscribe(() => {
              this.boardService.putUserStories(this.clickedus).subscribe(() => {
                const obj = {
                  id: this.clickedus.id,
                  usname: this.clickedus.shortName,
                  points: this.clickedus.points,
                  acceptanceCriteria: this.clickedus.acceptanceCriteria,
                  modalTasks: this.clickedus.tasks,
                  assignedTo: this.clickedus.assignedTo,
                  shortName: this.linkedToCard.shortName
                };
                connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj);
                connection.invoke('SendGanttToUpdate', BoardId);
              });
            });
          });
        });

      });
    });
  }

  deletesubtask(t, st) {
    let i = 0;

    let progress;


    this.boardService.getUSById(this.clickedus.id).subscribe(a => {
      const fetchedUS = a;
      console.log(fetchedUS);
      fetchedUS.tasks.forEach(x => {
        if (x.taskId == t.taskId) {
          x.subtask.forEach(y => {
            if (y.subtaskId == st.subtaskId) {
              x.subtask.splice(i, 1);
            }
            i++;
          });
        }
      });
      console.log(fetchedUS.tasks);
      this.clickedus = fetchedUS;

      this.boardService.putUserStories(this.clickedus).subscribe(x => {
        // console.log("x",x);
        this.boardService.getByLinkedId(this.clickedus.linkedToId).subscribe(us => {
          const linkedToCards = us;
          progress = this.calculateProgress(linkedToCards);
          console.log(progress);
          this.clickedus.progress = progress;
          this.boardService.getByUniqueId(this.clickedus.linkedToId).subscribe(p => {
            p.progress = progress;
            this.boardService.putUserStories(p).subscribe(() => {
              this.boardService.putUserStories(this.clickedus).subscribe(() => {
                const obj = {
                  id: this.clickedus.id,
                  usname: this.clickedus.shortName,
                  points: this.clickedus.points,
                  acceptanceCriteria: this.clickedus.acceptanceCriteria,
                  modalTasks: this.clickedus.tasks,
                  assignedTo: this.clickedus.assignedTo,
                  shortName: this.linkedToCard.shortName
                };
                connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj);
                connection.invoke('SendGanttToUpdate', BoardId);
              });
            });
          });
        });


      });
    });

    // console.log(this.clickedus.tasks,"outer");
  }
  onChange(event, st, t) {
    // console.log(t);
    // console.log("onChange event.checked " + event.checked);
    // console.log(typeof event.checked);
    const str: string = String(event.checked);
    console.log(str, 'change');
    this.boardService.getUSById(this.clickedus.id).subscribe(x => {
      this.fetchedUserStory = x;
      console.log(this.fetchedUserStory.tasks);
      this.fetchedUserStory.tasks.forEach(y => {
        if (y.taskId === t.taskId) {
          y.subtask.forEach(z => {
            if (z.subtaskId === st.subtaskId) {
              z.subtaskStatus = str;
            }
          });
        }
      });
      this.boardService.putUserStories(this.fetchedUserStory).subscribe(() => {
        let progress = 0;
        this.boardService.getByLinkedId(this.fetchedUserStory.linkedToId).subscribe(us => {
          const linkedUserStories = us;
          progress = this.calculateProgress(linkedUserStories);
          console.log(progress);
          this.fetchedUserStory.progress = progress;
          this.boardService.getByUniqueId(this.fetchedUserStory.linkedToId).subscribe(a => {
            const parent = a;
            parent.progress = progress;
            this.boardService.putUserStories(parent).subscribe(() => {
              this.boardService.putUserStories(this.fetchedUserStory).subscribe(() => {connection.invoke('SendGanttToUpdate', BoardId);
            });
            });
          });
        });
      });
    });
  }

  calculateProgress(linkedUserStories) {
    let noOfStories = 0;
    let innerProgress = 0,
      progress = 0,
      count = 0,
      noOfSub = 0,
      noOfTask = 0;
    linkedUserStories.forEach(y => {
      y.tasks.forEach(z => {
        z.subtask.forEach(sub => {
          if (sub.subtaskStatus == 'true') {
            count++;
          }
          noOfSub++;
        });
        // noOfTask++;
      });
      if (noOfSub != 0) {
        innerProgress = count / noOfSub;
        noOfStories++;
      }
      count = 0;
      if (noOfSub != 0) {
        progress = progress + innerProgress;
      }
      noOfTask = 0;
      noOfSub = 0;
    });
    if (noOfStories != 0) {progress = (progress / noOfStories); }
    return progress;
  }

  changePoints(event: MatRadioChange) {
    console.log(event.value);
    this.clickedus.points = event.value;
    console.log(this.clickedus);
    this.boardService.putUserStories(this.clickedus).subscribe(x => {
      console.log('Points changed');
      const obj = {
        id: this.clickedus.id,
        usname: this.clickedus.shortName,
        points: this.clickedus.points,
        acceptanceCriteria: this.clickedus.acceptanceCriteria,
        modalTasks: this.clickedus.tasks,
        assignedTo: this.clickedus.assignedTo,
        shortName: this.linkedToCard.shortName
      };
      connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj);
    });
  }

  changeStartDate(event) {
    if (event.value !== '') {
      const x = new Date(event.value);
      x.setHours(new Date().getHours());
      x.setMinutes(new Date().getMinutes());
      const date = new Date(x).toISOString();
      if (this.clickedus.linkedToId != this.clickedus.uniqueId) {
        this.boardService
          .getByUniqueId(this.clickedus.linkedToId)
          .subscribe(x => {
            if (date < x.startTime || date > x.endTime) {
              this._snackbar.open(
                'Start date doesnot lie between parent\'s dates',
                '',
                { duration: 2000 }
              );
            } else {
              if (date > this.clickedus.endTime) {
                this._snackbar.open(
                  'Start date cannot be greater than end date',
                  '',
                  { duration: 2000 }
                );
              } else {
                this.clickedus.startTime = date;
                this.startDateDisplay = this.clickedus.startTime;
                this.boardService
                  .putUserStories(this.clickedus)
                  .subscribe();
              }
            }
          });
      } else {
        if (date > this.clickedus.endTime) {
          this._snackbar.open('Start date cannot be greater than end date', '', {duration: 2000});
        } else {
          this.clickedus.startTime = date;
          this.startDateDisplay = this.clickedus.startTime;
          this.boardService.putUserStories(this.clickedus).subscribe();
        }
      }
    }
  }
  changeEndDate(event) {
    if (event.value !== '') {
      const x = new Date(event.value);
      x.setHours(new Date().getHours());
      x.setMinutes(new Date().getMinutes());
      const date = new Date(x).toISOString();
      if (this.clickedus.linkedToId != this.clickedus.uniqueId) {
        this.boardService
          .getByUniqueId(this.clickedus.linkedToId)
          .subscribe(x => {
            if (date < x.startTime || date > x.endTime) {
              this._snackbar.open(
                'Start date doesnot lie between parent\'s dates',
                '',
                { duration: 2000 }
              );
            } else {
              if (date < this.clickedus.startTime) {
                this._snackbar.open(
                  'End date cannot be less than start date',
                  '',
                  { duration: 2000 }
                );
              } else {
                this.clickedus.endTime = date;
                this.endDateDisplay = this.clickedus.endTime;
                this.boardService
                  .putUserStories(this.clickedus)
                  .subscribe();
              }
            }
          });
      } else {
        if (date < this.clickedus.startTime) {
          this._snackbar.open('End date cannot be less than start date', '', {duration: 2000});
        } else {
          this.clickedus.endTime = date;
          this.endDateDisplay = this.clickedus.endTime;
          this.boardService.putUserStories(this.clickedus).subscribe(() => connection.invoke('SendGanttToUpdate', BoardId));
        }
      }
    }
  }


  setDependency(event) {
    // console.log(event);
    const prevLinkedToId = this.clickedus.linkedToId;
    this.clickedus.linkedToId = event;
    let progress, progress1;
    this.boardService.putUserStories(this.clickedus).subscribe(() => {
      this.boardService.getByLinkedId(this.clickedus.linkedToId).subscribe(us => {
        const linkedToCards = us;
        progress = this.calculateProgress(linkedToCards);
        console.log('new', progress);
        this.clickedus.progress = progress;
        this.boardService.getByUniqueId(this.clickedus.linkedToId).subscribe(p => {
          const parent = p;
          this.linkedToCard = p;
          parent.progress = progress;
          this.boardService.putUserStories(parent).subscribe(() => {
            this.boardService.putUserStories(this.clickedus).subscribe(() => {
              const obj = {
                id: this.clickedus.id,
                usname: this.clickedus.shortName,
                points: this.clickedus.points,
                acceptanceCriteria: this.clickedus.acceptanceCriteria,
                modalTasks: this.clickedus.tasks,
                assignedTo: this.clickedus.assignedTo,
                shortName: this.linkedToCard.shortName
              };
              connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj);
            });
          });
        });
      });
      this.boardService.getByLinkedId(prevLinkedToId).subscribe(us => {
        const linkedToCards = us;
        progress1 = this.calculateProgress(linkedToCards);
        console.log('prev', progress1);
        this.boardService.getByUniqueId(prevLinkedToId).subscribe(x => {
          x.progress = progress1;
          this.boardService.putUserStories(x).subscribe();
        });
      });
    });
  }
  assign(event) {
    console.log(event);
    // this.assignedIdArray.push(event);
    console.log(this.clickedus);
    let flag = false;
    this.clickedus.assignedTo.forEach(x => {
      if (x.assignedToId == event.id) {
        flag = true;
      }
    });
    if (flag == false) {
      const obj = {
        assignedToId: event.id,
        assignedToName: event.name
      };
      this.clickedus.assignedTo.push(obj);
      this.boardService
        .putUserStories(this.clickedus)
        .subscribe(x => {
          const obj = {
            id: this.clickedus.id,
            usname: this.clickedus.shortName,
            points: this.clickedus.points,
            acceptanceCriteria: this.clickedus.acceptanceCriteria,
            modalTasks: this.clickedus.tasks,
            assignedTo: this.clickedus.assignedTo,
            shortName: this.linkedToCard.shortName
          };
          connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj);
        });
    } else {
      // alert("Member is already assigned to this task");
      this._snackbar.open('Member is already assigned to this task', '', {duration: 2000});
    }
  }
  removeAssignedTo(memberToRemove) {
    // console.log(memberToRemove);
    let i = 0;
    this.clickedus.assignedTo.forEach(x => {
      if (x.assignedToId == memberToRemove.assignedToId) {
        this.clickedus.assignedTo.splice(i, 1);
      }
      i++;
    });
    // console.log("bfbhf",this.clickedus);
    this.boardService.putUserStories(this.clickedus).subscribe(() => {
      const obj = {
        id: this.clickedus.id,
        usname: this.clickedus.shortName,
        points: this.clickedus.points,
        acceptanceCriteria: this.clickedus.acceptanceCriteria,
        modalTasks: this.clickedus.tasks,
        assignedTo: this.clickedus.assignedTo,
        shortName: this.linkedToCard.shortName
      };
      connection.invoke('SendMessageToGroupForDelete', this.clickedus.id, obj);
    });
  }
}
