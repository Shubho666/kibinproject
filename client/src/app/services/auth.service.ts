import { Injectable, Inject } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { tap } from 'rxjs/operators';
import { DOCUMENT } from '@angular/common';
import * as moment from 'moment';
import Cookies from 'js-cookie';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    @Inject(DOCUMENT) private document: Document,
    private httpClient: HttpClient,
    private router: Router
  ) { }
  // tslint:disable-next-line: variable-name
  private _isUserAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isUserAuthenticated: Observable<
    boolean
  > = this._isUserAuthenticatedSubject.asObservable();

  // updateUserAuthenticationStatus(){
  //   return this.httpClient.get<boolean>(`${environment.}/home/isAuthenticated`, {withCredentials: true}).pipe(tap(isAuthenticated => {
  //     this._isUserAuthenticatedSubject.next(isAuthenticated);
  //   }));
  // }
  user: any = [];
  public authSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    this.user
  );
  cookiename = 'jwt';

  helper = new JwtHelperService();
  userIdLoggedIn;

  setUserAsNotAuthenticated() {
    this._isUserAuthenticatedSubject.next(false);
  }
  private JwtExists() {
    if (Cookies.get(this.cookiename) != null) {
      return true;
    } else {
      return false;
    }
  }
  jwtToken() {
    const token = Cookies.get(this.cookiename);
    const obj = {
      decodedToken: this.helper.decodeToken(token),
      expirationDate: this.helper.getTokenExpirationDate(token),
      isExpired: this.helper.isTokenExpired(token)
    };
    return obj;
  }

  loggedInUser() {
    return this.jwtToken().decodedToken.id;
  }

  isLoggedIn() {
    if (this.JwtExists()) {
      const expDate = this.jwtToken().isExpired;
      // tslint:disable-next-line: triple-equals
      if (expDate == true) {
        return false;
      } else {
        return true;
      }
    } else {
      {
        return false;
      }
    }
  }

  login() {
    if (this.JwtExists() === true) {
      const tokenObj = this.jwtToken();
      console.log(tokenObj);
      this.user = tokenObj;
      this.authSubject.next(this.user);
      console.log(this.authSubject);
      return;
    } else {
      this.document.location.href = environment.starturlserver + '/auth/login';
    }
  }

  // sendmail(){
  //   this.document.location.href = 'http://localhost:8001/auth/sendmail';
  // }

  // sendmail() {
  //   console.log("sendmail function");

  //   return this.httpClient.post('http://localhost:8001/auth/sendmail', "s");
  // }
  logout() {
    Cookies.remove(this.cookiename);
    return;
    // this.document.location.href = "http://localhost:4200/login";
  }

  privateToken(appname, expiryDate): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/token/accessToken?appname=' + appname + '&expiryDate=' + expiryDate);
  }

  getAllTokens(): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/Token/userId/' + this.loggedInUser());
  }

  deleteToken(tokenid): Observable<any> {
    return this.httpClient.delete(environment.starturlserver + '/Token/' + tokenid);
  }
}
