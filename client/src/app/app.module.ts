import Cookies from 'js-cookie';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AuthComponent } from './components/auth/auth.component';
import {
  Board3Component,
  CardOverviewDialog1,
  CreateColumn,
  DialogOverviewExample3Dialog,
  Activity,
  LinkComponent
  } from './components/board3/board3.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CardComponent, ModalComponent } from './components/card/card.component';
import { CardsRequestsComponent } from './components/cards-requests/cards-requests.component';
import { ConditionEventDirectiveModule } from 'ng2-events/lib/condition-directive';
import { DashboardComponent, ProjectDialog } from './components/dashboard/dashboard.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { EpicsCreationAreaComponent } from './components/epics-creation-area/epics-creation-area.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule } from '@angular/forms';
import { GanttComponent, GanttActivity } from './components/gantt/gantt.component';
import { HttpClientModule } from '@angular/common/http';
import { IdeazoneComponent, IdeazoneActivity } from './components/ideazone/ideazone.component';
import { JwtModule } from '@auth0/angular-jwt';
import { MAT_CHECKBOX_CLICK_ACTION, MatCheckboxModule } from '@angular/material/checkbox';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MAT_LABEL_GLOBAL_OPTIONS, MatInputModule, MatNativeDateModule,
   MAT_BOTTOM_SHEET_DEFAULT_OPTIONS, MatPaginatorModule} from '@angular/material';
import { MAT_RADIO_DEFAULT_OPTIONS, MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';
import { ChartsModule } from 'ng2-charts';

import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialogModule } from '@angular/material/dialog';
import {MatBottomSheetModule, MatBottomSheetRef} from '@angular/material/bottom-sheet';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import {MatTableModule} from '@angular/material/table';
import { MatSelectModule } from '@angular/material';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { NavbarComponent, BottomSheetActivity } from './components/navbar/navbar.component';
import { Ng2CompleterModule } from 'ng2-completer';
import { NgModule } from '@angular/core';
import { OnceEventModule } from 'ng2-events/lib/once';
import { PrivatespaceComponent } from './components/privatespace/privatespace.component';
import { ReactiveFormsModule } from '@angular/forms';
import { TextFieldModule } from '@angular/cdk/text-field';
import { AddMemberComponent } from './components/add-member/add-member.component';
import { ChatsystemComponent } from './components/chatsystem/chatsystem.component';
import { PrivatetokenComponent } from './components/privatetoken/privatetoken.component';
import { ReportComponent, DialogOverviewExampleDialog } from './components/report/report.component';
import { HomepageComponent } from './components/homepage/homepage.component';
import { MyActivityComponent } from './components/my-activity/my-activity.component';
import { AutofocusModule } from 'angular-autofocus-fix';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { WorkspaceComponent } from './components/workspace/workspace.component';

export function tokenGetter() {
  // console.log('hi there this function was called')
  return Cookies.get('jwt');
}
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    IdeazoneComponent,
    EpicsCreationAreaComponent,
    CardComponent,
    ModalComponent,
    PrivatespaceComponent,
    CardsRequestsComponent,
    Board3Component,
    DialogOverviewExample3Dialog,
    CardOverviewDialog1,
    CreateColumn,
    DashboardComponent,
    ProjectDialog,
    GanttComponent,
    AuthComponent,
    AddMemberComponent,
    ChatsystemComponent,
    BottomSheetActivity,
    PrivatetokenComponent,
    ReportComponent,
    HomepageComponent,
    Activity,
    PrivatetokenComponent,
    MyActivityComponent,
    BottomSheetActivity,
    LinkComponent,
    DialogOverviewExampleDialog,
    IdeazoneActivity,
    GanttActivity,
    WorkspaceComponent
  ],
  imports: [
    NgScrollbarModule,
    AutofocusModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
    MatButtonModule,
    MatGridListModule,
    MatDividerModule,
    MatCardModule,
    MatTabsModule,
    MatExpansionModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatCheckboxModule,
    HttpClientModule,
    OnceEventModule,
    ConditionEventDirectiveModule,
    MatDialogModule,
    MatSelectModule,
    MatRadioModule,
    DragDropModule,
    MatMenuModule,
    MatChipsModule,
    MatTooltipModule,
    HttpClientModule,
    AppRoutingModule,
    MatDialogModule,
    MatSnackBarModule,
    ChartsModule,
    MatBottomSheetModule,
    // GanttComponent,
    MatMenuModule,
    FormsModule,
    DragDropModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    MatSelectModule,
    Ng2CompleterModule,
    TextFieldModule,
    MatRadioModule,
    FlexLayoutModule,
    MatTableModule,
MatPaginatorModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatBottomSheetModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        // tslint:disable-next-line: max-line-length
        whitelistedDomains: ['172.23.234.69:8001', 'localhost:8002', 'localhost:8001', 'localhost:8003', 'localhost:5003', 'localhost:5002', 'localhost:5001', 'proflo.app.cgi-w3.stackroute.io']
        // blacklistedRoutes: ["example.com/examplebadroute/"]
      }
    })
  ],
  // tslint:disable-next-line: max-line-length
  entryComponents: [ModalComponent, DialogOverviewExample3Dialog, CardOverviewDialog1, CreateColumn, ProjectDialog, LinkComponent, Activity, BottomSheetActivity, DialogOverviewExampleDialog, IdeazoneActivity, GanttActivity],
  providers: [
    { provide: MatDialogRef, useValue: {} },
    { provide: MAT_DIALOG_DATA, useValue: [] },
    { provide: MAT_CHECKBOX_CLICK_ACTION, useValue: 'check' },
    { provide: MAT_LABEL_GLOBAL_OPTIONS, useValue: { float: 'always' }},
    {provide: MAT_BOTTOM_SHEET_DEFAULT_OPTIONS, useValue: {hasBackdrop: true}},
    {
      provide: MAT_RADIO_DEFAULT_OPTIONS,
      useValue: { color: 'primary' },
    },
    { provide: MatBottomSheetRef, useValue: {} }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
