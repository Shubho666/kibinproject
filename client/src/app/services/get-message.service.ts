import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GetMessageService {

  constructor(private httpClient:HttpClient) { }
   
  public postMessage(msg):Observable<any>{
    console.log(msg);
    return this.httpClient.post("https://localhost:5001/api/Message",msg);
  }

}
