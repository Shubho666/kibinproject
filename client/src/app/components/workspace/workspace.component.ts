import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Route } from '@angular/compiler/src/core';
@Component({
  selector: 'app-workspace',
  templateUrl: './workspace.component.html',
  styleUrls: ['./workspace.component.css']
})
export class WorkspaceComponent implements OnInit {
  constructor(private router: Router) { }
  projectId;
  ngOnInit() {
    this.projectId = localStorage.getItem('ideazone');
    this.router.navigate(['ideazone/workspace/' + localStorage.getItem('ideazone_projectid')]);
  }
}
