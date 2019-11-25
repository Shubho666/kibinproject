import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private httpClient: HttpClient) { }

  getProjectById(id: string): Observable<any> {
    return this.httpClient.get(environment.starturlserver + '/Report/project/' + id);

  }

}
