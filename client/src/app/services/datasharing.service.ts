import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class DatasharingService {
  constructor() { }
  Role;
  idflag = false;
  getRole() {
    return this.Role;
  }
  setRole(role) {
    this.Role = role;
  }
  getidflag() {
return this.idflag;
  }
  setidflag(value) {
this.idflag = value;
console.log('idflag ' + this.idflag);
  }
}












