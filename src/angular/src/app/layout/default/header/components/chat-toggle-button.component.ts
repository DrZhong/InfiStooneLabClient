import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: '[chat-toggle-button]',
  template: `
   <nz-badge [nzCount]="unreadChatMessageCount">
   <i nz-icon style="font-size:16px" nzType="message" nzTheme="outline"></i>
    </nz-badge>
    
  `,
  styles: [
  ]
})
export class ChatToggleButtonComponent implements OnInit {
  unreadChatMessageCount = 0;
  chatConnected = false;
  constructor() { }

  ngOnInit(): void {
    this.registerToEvents();
  }
  @HostListener('click', ['$event.target']) onClick(btn) {
    abp.event.trigger('app.message.topbar.click');
  }

  registerToEvents() {
    abp.event.on('app.chat.unreadMessageCountChanged', messageCount => {
      this.unreadChatMessageCount = messageCount;
    });

    abp.event.on('app.chat.connected', () => {
      this.chatConnected = true;
    });
  }
}
