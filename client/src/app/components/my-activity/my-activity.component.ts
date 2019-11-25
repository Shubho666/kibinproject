import { Component, OnInit , ViewChild} from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table';
import { BoardService } from 'src/app/services/board.service';
import { ApiCallsService } from 'src/app/services/api-calls.service';
import { TaskService } from '../../services/task.service';
import { Time } from '@angular/common';

@Component({
  selector: 'app-my-activity',
  templateUrl: './my-activity.component.html',
  styleUrls: ['./my-activity.component.css']
})
export class MyActivityComponent implements OnInit {
  constructor(private boardService: BoardService, private ideaService: ApiCallsService,
              private taskservice: TaskService) { }
projectId;
UserId;
activityGant;
activityKanban;
activityIdea;
Ad: ActivityData[] = [];
Sorted: ActivityData[] = [];
displayedColumns: string[] = ['Date and Time', 'Activity Performed', 'Actvity Performed In'];
dataSource;

// http://localhost:8003/api/LoggerActivity/userid/projectid/5dccd6c6119563225c308679/
   // 5dd6bea97c7a6c5ba4e41778

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  ngOnInit() {

    this.Ad = [];
    this.Sorted = [];

    this.projectId = localStorage.getItem('ideazone_projectid');
    this.UserId = localStorage.getItem('ideazone_userid');

    this.taskservice.getSpecificByUser().subscribe(x => {
      this.activityGant = x;
      this.activityGant = this.activityGant.reverse();

      // tslint:disable-next-line: forin
      for (const i in this.activityGant) {

        this.Ad.push(
        {
          // tslint:disable-next-line: max-line-length
          Time: new Date(this.activityGant[i].published),
          time: (new Date(this.activityGant[i].published)).toString(),
          activity: this.activityGant[i].description,
          inside: 'Gantt'
        }
      );
        this.Sorted.push(
        {
          // tslint:disable-next-line: max-line-length
          Time: new Date(this.activityGant[i].published),
          time: (new Date(this.activityGant[i].published)).toString(),
          activity: this.activityGant[i].description,
          inside: 'Gantt'
        }
      );
      }

      this.boardService.getSpecificByUser(this.UserId, this.projectId).subscribe(x => {
      this.activityKanban = x;
      this.activityKanban = this.activityKanban.reverse();
      console.log(this.activityKanban);
      // tslint:disable-next-line: forin
      for (const i in this.activityKanban) {
        console.log(new Date(this.activityKanban[i].published));
        this.Ad.push(
        {
          // tslint:disable-next-line: max-line-length
          Time: new Date(this.activityKanban[i].published),
          time: (new Date(this.activityKanban[i].published)).toString(),
          activity: this.activityKanban[i].description,
          inside: 'Kanban'
        }
      );
        this.Sorted.push(
        {
          // tslint:disable-next-line: max-line-length
          Time: new Date(this.activityKanban[i].published),
          time: (new Date(this.activityKanban[i].published)).toString(),
          activity: this.activityKanban[i].description,
          inside: 'Kanban'
        }
      );
      }

      // tslint:disable-next-line: no-shadowed-variable
      this.ideaService.getSpecificByUser().subscribe(x => {
      this.activityIdea = x;
      this.activityIdea = this.activityIdea.reverse();
      // tslint:disable-next-line: forin
      for (const i in this.activityIdea) {
        this.Ad.push(
          {
            Time: new Date(this.activityIdea[i].published),
            time: (new Date(this.activityIdea[i].published)).toString(),
            activity: this.activityIdea[i].description,
            inside: 'IdeaZone'
          }
        );
        this.Sorted.push(
            {
              Time: new Date(this.activityIdea[i].published),
              time: (new Date(this.activityIdea[i].published)).toString(),
              activity: this.activityIdea[i].description,
              inside: 'IdeaZone'
            }
        );
        }
      this.Ad.sort((a, b) => -1 * (a.Time.getTime() - b.Time.getTime()));
      this.dataSource = new MatTableDataSource<ActivityData>(this.Ad);
      this.dataSource.paginator = this.paginator;
    });

  });
});

  }
  onclicksort(sortby) {
    console.log('Functon Called');
    if (sortby === 'PerformedIn') {
    this.dataSource = new MatTableDataSource<ActivityData>(this.Sorted);
    this.dataSource.paginator = this.paginator;
    } else {
      this.dataSource = new MatTableDataSource<ActivityData>(this.Ad);
      this.dataSource.paginator = this.paginator;
    }
  }
}

export interface ActivityData {
Time: Date;
time: string;
activity: string;
inside: string;
}
