import { Component, OnInit } from '@angular/core';
import {AuthService} from '../../services/auth.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-privatetoken',
  templateUrl: './privatetoken.component.html',
  styleUrls: ['./privatetoken.component.css']
})
export class PrivatetokenComponent implements OnInit {

  constructor(private authService:AuthService,
      private fb:FormBuilder) {
        this.tokenForm=fb.group({
          appName:"",
          expiryDate:""
        });
       }
  tokenForm:FormGroup;
  allTokenInfo;
  privatetoken;
  displayedColumns: string[] = ['appName','expiryDate','symbol'];

  ngOnInit() {
    this.authService.getAllTokens().subscribe(x=>{this.allTokenInfo=x;
    console.log(this.allTokenInfo)});
  }
  getPrivateToken(){
    let appname=(document.getElementById("appname") as HTMLInputElement).value;
    let expiryDate=(document.getElementById("expirydate") as HTMLInputElement).value;
    // expiryDate = expiryDate.slice(0, 10);
    this.authService.privateToken(appname,expiryDate).subscribe(x=>{this.privatetoken=x;
    console.log(this.privatetoken);
    //this.allTokenInfo.push(this.privatetoken);
    },
    err=>{this.privatetoken=err.error.text;
      console.log(this.privatetoken);
      //this.allTokenInfo.push(this.privatetoken);
    });
  }


  revoke(t){
    console.log(t);
    let i=0;
    this.allTokenInfo.forEach(x=>{
      if(x.id==t.id){
        this.allTokenInfo.splice(i,1);
        console.log("spliced"+this.allTokenInfo);
      }
      i++;
    })
    this.authService.deleteToken(t.id).subscribe();
  }

}
