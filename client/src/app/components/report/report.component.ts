import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label, MultiDataSet } from 'ng2-charts';
import { ActivatedRoute } from '@angular/router';
import { ReportService } from 'src/app/services/report.service';
import { count } from 'rxjs/operators';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
  projectid: string;
  projectData;
  projectLabels: string[] = [];
  projectCount: number[] = [];
  public barChartOptions: ChartOptions = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{}] },
  };
  public barChartLabels: Label[] = [];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public Colors = ['red', 'black', 'green', 'blue'];
  public doughnutChartLabels: Label[] = [];
  public doughnutChartData: MultiDataSet = [
    [350, 450, 100],
    [50, 150, 120],
    [250, 130, 70],
  ];
  public doughnutChartType: ChartType = 'doughnut';
  public barChartData: ChartDataSets[] = [
    { data: [] = [], label: 'Product Backlog' },
  ];
  constructor(private route: ActivatedRoute,
              private report: ReportService, public dialog: MatDialog) { }
  ngOnInit() {
    console.log();
    // localStorage.setItem('ideazone_projectid', this.projectid);
    this.getId();
    this.getData(this.projectid);
    console.log(this.barChartData[0].data);
    this.barChartData[0].data = this.projectCount;
   // console.log(this.barChartData);
  }
  // events
  public chartClicked({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }
  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }
  public randomize(): void {
    this.barChartType = this.barChartType === 'bar' ? 'line' : 'bar';
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(DialogOverviewExampleDialog, {
      width: '90%',
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
  public getId() {
    this.projectid = localStorage.getItem('ideazone_projectid');
    console.log('projectid', this.projectid);
  }
  public getData(id: string) {
    this.report.getProjectById(id).subscribe(x => {
     console.log(x);
      x.list.forEach(element => {
        console.log(element.date.substr(0,10));
    //    console.log(this.barChartLabels.find(element.date.substr(0,10)));
    console.log(this.barChartLabels.indexOf(element.date.substr(0,10)));
        if(this.barChartLabels.indexOf(element.date.substr(0,10))==-1)
        { 
          console.log("inside if");
          this.barChartLabels.push(element.date.substr(0,10));
        }
                
      });
      console.log(this.barChartLabels);
      x.list.forEach(element => {
        var flag=false;
        this.barChartData.forEach(checkedData=>{
          if(checkedData.label==element.label)
          {
           flag=true; 
          }
          
        })
        if(flag==false)
        {
        var name=element.label;
        var count:[]=[];
        var object={
          data:count,
          label:name
        }
        this.barChartData.push(object);
      }

        
      
      });
      console.log(this.barChartData);
      x.list.forEach(element => {
        this.barChartData.forEach(a=>{
          if(a.label==element.label)
          {
            a.data.push(element.userstoryid.length);
            
            
            
            
          }
        })

        
      });
      
        
    
     
    });
  }
}
@Component({
  selector: 'dialog-overview-example-dialog',
  templateUrl: 'details.html',
})
export class DialogOverviewExampleDialog {
  constructor(
    public dialogRef: MatDialogRef<DialogOverviewExampleDialog>,
    ) {}
  onNoClick(): void {
    this.dialogRef.close();
  }
}
