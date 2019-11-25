import { AfterViewInit, DoCheck, Component, ElementRef, ViewChild, ViewEncapsulation, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../../environments/environment';

import { HubConnectionBuilder, LogLevel, HttpTransportType } from '@aspnet/signalr';

import { LinkService } from '../../services/link.service';
import { TaskService } from '../../services/task.service';
import { AuthService } from 'src/app/services/auth.service';
import { DashboardService } from 'src/app/services/dashboard.service';
import { DatasharingService } from 'src/app/services/datasharing.service';
import { MatBottomSheet, MatBottomSheetRef } from '@angular/material/bottom-sheet';

import * as signalR from '@aspnet/signalr';

import 'dhtmlx-gantt';
import { element } from 'protractor';
import { distinct } from 'rxjs/internal/operators/distinct';

interface ILink {
  id: number;
  source: number;
  target: number;
  type: number;
}
export interface Food {
  value: string;
  viewValue: string;
}
interface IListOfTasks {
  id: number;
  start_date: Date;
  end_date: Date;
  text: string;
  progress: number;
  duration: number;
}
let connectionmaster;
let globalProId;
@Component({
  encapsulation: ViewEncapsulation.None,
  selector: 'gantt',
  styleUrls: ['./gantt.component.css'],
  templateUrl: './gantt.component.html'
})
export class GanttComponent implements AfterViewInit, OnInit, DoCheck {
  constructor(private linkService: LinkService, private taskService: TaskService, private route: ActivatedRoute,
              private bottomSheet: MatBottomSheet,private auth: AuthService,private dashBoardService: DashboardService) { }
  @ViewChild('gantt_here', { static: false }) ganttContainer: ElementRef;

  project_id;
  toggle = false;
  links = [];
  listOfTasks: any[] = [];
  foods: Food[];
  UserId;ProjectId;ProjectName;
  selectedValue: string;

  ngDoCheck() {
    const role = localStorage.getItem('Role');
    // console.log("The role is",role);
    if (role === 'member') {
      // console.log(role);
      gantt.config.readonly = true;
    }

  }
  openBottomSheet(): void {
    this.bottomSheet.open(GanttActivity);
  }

  toggleGrid() {
    gantt.config.show_grid = this.toggle;
    this.toggle = !this.toggle;
    gantt.render();
  }

  setScaleConfig(value) {
    console.log(value);

    switch (value) {
      case '1':
        console.log('hello', value);

        gantt.config.scale_unit = 'day';
        gantt.config.step = 1;
        gantt.config.date_scale = '%d %M';
        gantt.config.subscales = [];
        gantt.config.scale_height = 27;
        gantt.templates.date_scale = null;
        gantt.render();
        break;
      case '2':
        console.log('hello', value);
        const weekScaleTemplate = function(date) {
          const dateToStr = gantt.date.date_to_str('%d %M');
          const startDate = gantt.date.week_start(new Date(date));
          const endDate = gantt.date.add(gantt.date.add(startDate, 1, 'week'), -1, 'day');
          return dateToStr(startDate) + ' - ' + dateToStr(endDate);
        };

        gantt.config.scale_unit = 'week';
        gantt.config.step = 1;
        gantt.templates.date_scale = weekScaleTemplate;
        gantt.config.subscales = [
          { unit: 'day', step: 1, date: '%D' }
        ];
        gantt.config.scale_height = 50;
        gantt.render();
        break;
      case '3':
        console.log('hello', value);
        gantt.config.scale_unit = 'month';
        gantt.config.date_scale = '%F, %Y';
        gantt.config.subscales = [
          { unit: 'day', step: 1, date: '%j, %D' }
        ];
        gantt.config.scale_height = 50;
        gantt.templates.date_scale = null;
        gantt.render();
        break;
      case '4':
        console.log('hello', value);
        gantt.config.scale_unit = 'year';
        gantt.config.step = 1;
        gantt.config.date_scale = '%Y';
        gantt.config.min_column_width = 50;


        gantt.config.scale_height = 80;
        gantt.templates.date_scale = null;


        gantt.config.subscales = [
          { unit: 'month', step: 1, date: '%M' }
        ];
        gantt.render();
        break;
    }
  }
  ngOnInit() {
  
    const userdetails = this.auth.jwtToken();
     this.UserId = userdetails.decodedToken.id;
     this.dashBoardService.getProjectByOwner(this.UserId).subscribe(x => {
      this.project_id=localStorage.getItem("ideazone_projectid");
    const index = x.findIndex( y => y.id == this.project_id );  
    this.ProjectName = x[index].projectName; 
    });
   

    this.foods = [
      { value: '1', viewValue: 'Day' },
      { value: '2', viewValue: 'Week' },
      { value: '3', viewValue: 'Month' },
      { value: '4', viewValue: 'Year' }
    ];
    connectionmaster = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(environment.starturlconnection + '/notify', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    connectionmaster
      .start()
      .then( function() {
        console.log('Connected!', connectionmaster);
      })
      .catch(function(err) {
        return console.error(err.toString());
      });

    // setTimeout(() => {
    //   console.log(message);
    //   this.boardService.getFullList(this.boardid).subscribe(x => {
    //     this.allLists = x;
    //     console.log(this.allLists);
    //     this.connectedTo = [];
    //     for (const l in this.allLists) {
    //       this.connectedTo.push(this.allLists[l].name);
    //     }
    //   });
    // }, 1000);

  //   /

    connectionmaster.on("RabbitGanttUpdate", () => {
      this.listOfTasks = [];
      this.taskService.get(this.project_id).subscribe(tasks => {
        tasks.forEach(element => {
          this.listOfTasks.push({
            id: element.id,
            start_date: new Date(element.start_date),
            text: element.text,
            duration: element.duration,
            progress: element.progress
          });
        });

      });
    });

    setTimeout(() => {
      connectionmaster
        .invoke('JoinGroup', this.project_id)
        .then(res => console.log('Joined Group', this.project_id))
        .catch(err => {
          console.log('Error', err);
        });
    }, 1000);


   }

  ngAfterViewInit() {
   
    //this.project_id = this.route.snapshot.paramMap.get('id');
    this.project_id=localStorage.getItem("ideazone_projectid");
    globalProId = this.project_id;
    console.log(this.project_id);
    
    let connection: any;
    // SIGNALR CONNECTION
    connection = new HubConnectionBuilder()
      .configureLogging(LogLevel.Information)
      .withUrl(environment.signalRurl, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      }).build();

    connection.start().then(function() {
      console.log('Connected!');
    }).catch(function(err: Error) {
      return console.error(err.toString());
    });

    // SIGNALR FOR UPDATING TASKS
    connection.on('TaskUpdated', (newTask: IListOfTasks) => {

      const updatedTaskList = this.listOfTasks.map(task => task.id === newTask.id ? { ...newTask, start_date: new Date(newTask.start_date),
        end_date: new Date(newTask.end_date) } : task);
      this.listOfTasks = [...updatedTaskList];
      gantt.clearAll();
      gantt.render();
      gantt.parse({ data: this.listOfTasks, links: this.links });
    });

    // SIGNALR FOR ADDING TASKS
    connection.on('TaskAdded', (newTask: IListOfTasks) => {
      this.listOfTasks.push({
        id: newTask.id,
        start_date: new Date(newTask.start_date),
        text: newTask.text,
        duration: newTask.duration,
        progress: newTask.progress,
        // end_date:newTask.end_date
      });
      gantt.clearAll();
      //gantt.render();
      gantt.parse({ data: this.listOfTasks, links: this.links });
    });

    // SIGNALR FOR DELETING TASKS
    connection.on('TaskDeleted', (taskId: number) => {
      console.log('task deleted ', typeof (taskId), taskId);
      const filteredTaskList = this.listOfTasks.filter(task => task.id !== taskId);
      console.log(this.listOfTasks);
      console.log(filteredTaskList);
      console.log(this.links);
      this.listOfTasks = [...filteredTaskList];
      gantt.refreshData();
      gantt.clearAll();
    //  gantt.parse({ data: [], links });
    gantt.render();
      gantt.parse({ data: this.listOfTasks, links: this.links });
    });
    // SIGNALR FOR CREATING LINKS
    connection.on('LinkAdded', (newLink: ILink) => {
      console.log(newLink);
      this.links.push({
        id: newLink.id,
        source: newLink.source,
        target: newLink.target,
        type: newLink.type
      });
      console.log(this.links);
      gantt.clearAll();
      gantt.render();
      gantt.parse({ data: this.listOfTasks, links: this.links });
    });
    // SIGNALR FOR DELETING LINKS
    connection.on('LinkDeleted', (linkId: number) => {

      console.log(linkId);
      const filteredLinkList = this.links.filter(link => link.id !== linkId);
      this.links = [...filteredLinkList];
      gantt.refreshData();
      gantt.clearAll();
      gantt.render();
      gantt.parse({ data: this.listOfTasks, links: this.links });
    });


    setTimeout(() => {
      connection.invoke('JoinGroup', this.project_id)
        .then(() => console.log('Joined Group', this.project_id))
        .catch(err => { console.log('Error', err); });
    }, 1000);

    // INITIALIZING GANTT CHART

    gantt.init(this.ganttContainer.nativeElement);
    gantt.config.drag_progress = false;
    gantt.config.grid_width = 300;
    gantt.config.columns = [
      { name: 'text', label: 'User Story', width: '*', tree: true },
      { name: 'start_date', label: 'Start time', align: 'center' },
      { name: 'add', label: '', width: '44' }
    ];
    this.setScaleConfig('3');



    // gantt.config.xml_date = '%Y-%d-%m';
    // gantt.config.date_scale = '%j, %D';

    this.taskService.get(this.project_id).subscribe(tasks => {
      tasks.forEach(element => {
        this.listOfTasks.push({
          id: element.id,
          start_date: new Date(element.start_date),
          text: element.text,
          duration: element.duration,
          progress: element.progress
        });
      });
      this.linkService.get(this.project_id).subscribe(linkss => {
        linkss.forEach(element => {
          this.links.push({
            id: element.id,
            source: element.source,
            target: element.target,
            type: element.type
          });
        });
       // console.log(link);
       // const link = [...this.links];
        //const data = [...this.listOfTasks];
        // console.log(data);
        // console.log(link);

        gantt.clearAll();
        gantt.render();
        gantt.parse({ data:this.listOfTasks, links: linkss });
      });
    });


    // CRUD OPERATIONS ON GANTT CHART
    const dp = gantt.createDataProcessor({
      task: {
        update: (data) => {
          console.log(data);
          let k: string;
          let k1: string;
          k = data.start_date.substring(3, 5) + '-' + data.start_date.substring(0, 2) + data.start_date.substring(5, 19);
          k1 = data.end_date.substring(3, 5) + '-' + data.end_date.substring(0, 2) + data.end_date.substring(5, 19);
          data.start_date = Date.parse(k);
          data.end_date = Date.parse(k1);

          const us = {
            id: data.id,
            start_date: data.start_date,
            end_date: data.end_date,
            duration: data.duration,
            progress: data.progress,
            text: data.text,
            project_id: this.project_id
          };

          //this.project_id = this.route.snapshot.paramMap.get('id');
          this.project_id=localStorage.getItem("ideazone_projectid");
          this.taskService.update(us, this.project_id)
            .toPromise()
            .catch((error: any): Promise<any> => {
              console.log(error);
              return Promise.reject(error);
            });
          connection.invoke('UpdateTask', this.project_id, data);
          connectionmaster.invoke('SendUserStoryToAddOnList', this.project_id, 'usd');
        },
        create: (data) => {
          let k: string;
          let k1: string;
          k = data.start_date.substring(3, 5) + '-' + data.start_date.substring(0, 2) + data.start_date.substring(5, 19);
          k1 = data.end_date.substring(3, 5) + '-' + data.end_date.substring(0, 2) + data.end_date.substring(5, 19);
          data.start_date = Date.parse(k);
          data.end_date = Date.parse(k1);

          const us = {
            id: data.id,
            start_date: data.start_date,
            end_date: data.end_date,
            duration: data.duration,
            progress: data.progress,
            text: data.text,
            project_id: this.project_id
          };
          // this.project_id = this.route.snapshot.paramMap.get('id');
          this.project_id=localStorage.getItem("ideazone_projectid");
          console.log(this.project_id);
          this.taskService.insert(us, this.project_id)
            .toPromise()
            .catch((error: any): Promise<any> => {
              console.log(error);
              return Promise.reject(error);
            });
         connection.invoke('AddTask', this.project_id, data);
          connectionmaster.invoke('SendUserStoryToAddOnList', this.project_id, 'usd');
        },
        delete: (id) => {

          //this.project_id = this.route.snapshot.paramMap.get('id');
          this.project_id=localStorage.getItem("ideazone_projectid");
          this.taskService.remove(id, this.project_id)
            .toPromise()
            .catch((error: any): Promise<any> => {
              console.log(error);
              return Promise.reject(error);
            });
         connection.invoke('DeleteTask', this.project_id, id);
          connectionmaster.invoke('SendUserStoryToAddOnList', this.project_id, 'usd');
        }
      },
      link: {
        create: (data) => {
          console.log(data);

          const us = {
            id: data.id,
            project_id: this.project_id,
            source: data.source,
            target: data.target,
            type: data.type
          };
         // this.project_id = this.route.snapshot.paramMap.get('id');
         this.project_id=localStorage.getItem("ideazone_projectid");
          this.linkService.insert(us, this.project_id)
            .toPromise()
            .catch((error: any): Promise<any> => {
              console.log(error);
              return Promise.reject(error);
            });
         connection.invoke('AddLink', this.project_id, data);
        },

        delete: (id) => {
          //this.project_id = this.route.snapshot.paramMap.get('id');
          this.project_id=localStorage.getItem("ideazone_projectid");
          this.linkService.remove(id, this.project_id)
            .toPromise()
            .catch((error: any): Promise<any> => {
              console.log(error);
              return Promise.reject(error);
            });
         connection.invoke('DeleteLink', this.project_id, id);
        }
      }
    });
  }
}
@Component({
  selector: 'app-gantt-activity',
  templateUrl: 'bottom-sheet-overview-example-sheet.html',
})
export class GanttActivity implements OnInit {
  constructor(private bottomSheetRef: MatBottomSheetRef<GanttActivity>, private taskService: TaskService) { }
  activity; activityreverse;
  ngOnInit(): void {
    this.taskService.getActivity().subscribe(x => {
      console.log(globalProId, 'proid');
      this.activity = x;
      console.log(this.activity);
      // this.activity.filter((v , i, a)=>a.findIndex(t=>(t.description === v.description)) ===i);

      // this.activity = distinct(this.activity, y => y.description);

      const uniq = {};
      this.activity.filter(obj => !uniq[obj.description] && (uniq[obj.description] = true));
      console.log(this.activity);


      this.activityreverse = this.activity.reverse();


      this.activityreverse.forEach(ar => {
        ar.published = (new Date(ar.published)).toString();
      });
    });
  }
  openLink(event: MouseEvent): void {
    this.bottomSheetRef.dismiss();
    event.preventDefault();
  }
}
