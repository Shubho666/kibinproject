import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class IdeazoneDataSplitService {

  constructor() { }
  epicsR; epics; epicsB;
  epicsPR; epicsp;
  View = false;


  changeView(view) {
    this.View = view;

  }
  getView() {
    return this.View;
  }

  Workspacedatasplit(input) {
    this.epicsR = [];
    this.epics = [];
    this.epicsB = [];
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < input.length; i++) {
      if (input[i].status === 'workspace') {
        this.epics.push(input[i]);
      } else if (input[i].status === 'productbacklog') {
        this.epicsB.push(input[i]);
      } else if (input[i].status === 'requested') {
        this.epicsR.push(input[i]);
      }
    }
    // tslint:disable-next-line: prefer-for-of
    if (this.epicsB !== null) {
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < this.epicsB.length; i++) {
      this.epics.push(this.epicsB[i]);
    }
  }
    return this.epics;
  }

  WorkspacedatasplitR() {
    return this.epicsR;
  }

  Privatespacedatasplit(input) {
    this.epicsPR = [];
    this.epicsp = [];
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < input.length; i++) {
      if (input[i].status === 'privatespace') {
        this.epicsp.push(input[i]);
      } else {
        this.epicsPR.push(input[i]);
      }
    }
    return this.epicsp;
  }

  PrivaetespacedatasplitR() {
    return this.epicsPR;
  }

}


