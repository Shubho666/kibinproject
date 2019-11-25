import { Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { BoardService } from '../../services/board.service';
import { environment } from '../../../environments/environment';
import { Subscription } from 'rxjs';
import {Router} from '@angular/router';
import { DOCUMENT } from "@angular/common";
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {
  isUserAuthenticated = false;
  subscription: Subscription;
  userName: string;

  constructor(@Inject(DOCUMENT) private document: Document,private httpClient: HttpClient, private authService: AuthService, private boardService: BoardService, private router:Router) { }

  ngOnInit() {
    if(this.authService.isLoggedIn()){
      console.log('inside if');
      this.document.location.href=environment.starturlclient+'/dashboard';
    }
  }


  login() {
    this.authService.login();
  }

  

}
