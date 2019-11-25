import { Component, OnInit, DoCheck } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-chatsystem',
  templateUrl: './chatsystem.component.html',
  styleUrls: ['./chatsystem.component.css']
})
export class ChatsystemComponent implements OnInit,DoCheck {

  constructor(private route: ActivatedRoute) { }
  userid; allMessages;
  selectedcard=0;

  ngDoCheck(){
    let chatHistory = document.getElementById("messages");
    chatHistory.scrollTop = chatHistory.scrollHeight-chatHistory.clientHeight;

  }

  // id , group=[username1,username2] msg=[{ text:,sender:,time:}]
  ngOnInit() {
    this.userid = this.route.snapshot.paramMap.get('userid');

    this.allMessages = [
      {
        id: '1',
        group: ['user1', 'user2'],
        message: [
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send2',
            sender: 'user2'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send2',
            sender: 'user2'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send2',
            sender: 'user2'
          }
        ]
      },
      {
        id: '2',
        group: ['user1', 'user3'],
        message: [
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send3',
            sender: 'user3'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
         
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send3',
            sender: 'user3'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send3',
            sender: 'user3'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
         
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send3',
            sender: 'user3'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send3',
            sender: 'user3'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
         
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send3',
            sender: 'user3'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send3',
            sender: 'user3'
          },
          {
            text: 'msg from send1',
            sender: 'user1'
          },
         
          {
            text: 'msg from send1',
            sender: 'user1'
          },
          {
            text: 'msg from send3',
            sender: 'user3'
          }
        ]
      },
      // {
      //   id: '3',
      //   group: ['user3', 'user2'],
      //   message: [
      //     {
      //       text: 'msg from send3',
      //       sender: 'user3'
      //     },
      //     {
      //       text: 'msg from send2',
      //       sender: 'user2'
      //     },
      //     {
      //       text: 'msg from send3',
      //       sender: 'user3'
      //     },
      //     {
      //       text: 'msg from send3',
      //       sender: 'user3'
      //     },
      //     {
      //       text: 'msg from send2',
      //       sender: 'user2'
      //     },
      //     {
      //       text: 'msg from send3',
      //       sender: 'user3'
      //     },
      //     {
      //       text: 'msg from send2',
      //       sender: 'user2'
      //     }
      //   ]
      // }
    ];

  }

  selectedCardChange(cardName) {
    
    console.log(cardName);
    let i =0;
    this.allMessages.forEach(element => {
      if ((element.group[0] == cardName) || (element.group[1] == cardName)) {
        this.selectedcard=i;
      }
      i+=1;
      
    });
  }


}
