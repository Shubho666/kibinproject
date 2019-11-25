import { AuthService } from '../../services/auth.service';
import { BoardService } from '../../services/board.service';
import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  DoCheck,
  AfterViewInit,
  OnChanges
} from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { DatasharingService } from 'src/app/services/datasharing.service';
import {DashboardService} from '../../services/dashboard.service';
import {MatBottomSheet, MatBottomSheetRef} from '@angular/material/bottom-sheet';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnDestroy, OnInit, DoCheck {
  mobileQuery: MediaQueryList;
  // tslint:disable-next-line: variable-name
  private _mobileQueryListener: () => void;
  panelOpenState: false;
  projects: any;
  constructor(changeDetectorRef: ChangeDetectorRef,
              media: MediaMatcher,
              private dss: DatasharingService,
              private router: Router,
              private boardService: BoardService,
              private authService: AuthService,
              private dashBoardService: DashboardService,
              private bottomSheet: MatBottomSheet) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    // tslint:disable-next-line: deprecation
    this.mobileQuery.addListener(this._mobileQueryListener);
  }
  isLoggedIn: boolean;
  user = [];
  token;
  LoginView;
  DashboardView;
  URL;
  IdzoneProjectId;
  userDetails;
  ngOnInit() {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.authService.authSubject.subscribe(x => this.user = x);
    this.token = this.authService.jwtToken().decodedToken.id;
    this.dashBoardService.getUserById(this.token).subscribe(x => {
      this.userDetails = x; });
    this.dashBoardService.getProjectByOwner(this.token).subscribe(x => {this.projects = x; });
  }
  ngDoCheck() {
    this.URL = window.location.href;
    this.DashboardView = this.URL.includes('dashboard');
    this.LoginView = this.URL.includes('login');
    this.IdzoneProjectId = localStorage.getItem('ideazone_projectid');
    // console.log((this.DashboardView));
  }
  ngOnDestroy(): void {
    // tslint:disable-next-line: deprecation
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }
  values() {
    this.boardService.getValue().subscribe(x => console.log(x));
  }
  logout() {
    this.authService.logout();
    this.isLoggedIn = false;
    this.router.navigate(['/login']);
  }
  openBottomSheet(): void {
    this.bottomSheet.open(BottomSheetActivity);
  }
  loadProjects() {
  this.dashBoardService.getProjectByOwner(this.token).subscribe(x => {this.projects = x; });
  }
  routeTo(openproject) {
    console.log(openproject);
    console.log(this.token);
    if (openproject.owner.includes(this.token)) {
      console.log('role of user', openproject.owner);
      localStorage.setItem('Role', 'owner');
    } else {
      localStorage.setItem('Role', 'member');
    }
    // this.router.navigate(['ideazone/workspace/' + openproject.id]);
    this.router.navigate (['ideazone/Workspace/' + this.IdzoneProjectId] );
    // this.idzoneapi.setProjectId(open_project.id);
    localStorage.setItem('ideazone_projectid', openproject.id);
    this.dss.setidflag(true);
    this.panelOpenState = false;
  }
}
@Component({
  selector: 'app-bottom-sheet-activity',
  templateUrl: 'bottom-sheet.html',
})
// tslint:disable-next-line: component-class-suffix
export class BottomSheetActivity {
  constructor(private bottomSheetRef: MatBottomSheetRef<BottomSheetActivity>) {}
  openLink(event: MouseEvent): void {
    this.bottomSheetRef.dismiss();
    event.preventDefault();
  }
}
