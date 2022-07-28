import { Router } from '@angular/router';
import { NotificationServiceProxy, GuidEntityDto } from './../../../../../shared/service-proxies/service-proxies';

import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, NgZone } from '@angular/core';
import dayjs from 'dayjs';
/**
 * 菜单通知
 */
@Component({
  selector: 'header-notify',
  template: `
  <notice-icon
    [data]="data"
    [count]="count"
    [loading]="loading"
    (select)="select($event)" 
    (clear)="clear($event)" 
    ></notice-icon>
  `,
})
export class HeaderNotifyComponent extends AppComponentBase {

  constructor(
    private ngZone: NgZone,
    injector: Injector,
    private router: Router,
    private notificationServiceProxy: NotificationServiceProxy) {
    super(injector);
  }
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    //this.initData();
    this.loadData();
    abp.event.on('abp.notifications.received', userNotification => {
      console.log(userNotification);
      switch (userNotification?.notification.notificationName) {
        case 'Abp.Vote':
        case 'MeetingChanged':
          return;
        case 'Abp.Refresh':
          window.location.reload();
          break;
        case 'App.Test':
        case 'NotifyConsts.ReplyAuditReSearch'://回复评论
        case 'NotifyConsts.AuditReSearch':
        case 'App.ReagentStockNotifyJob':
          let msg = abp.utils.truncateStringWithPostfix(userNotification.notification.data.properties['Message'], 30, '...');
          this.notify.info(msg, '收到信消息');
          this.destopNotify(msg, true);
          break;
        case 'App.Notification':
          let msg1 = abp.utils.truncateStringWithPostfix(userNotification.notification.data.properties['Message'], 30, '...');
          let title = `收到来自【${userNotification.notification.data.properties['userFullName']}】的消息`;
          this.notify.info(msg1, title);
          this.destopNotify(msg1, true);
          break
        default:
          break;
      }
      this.loadData();
    });
  }

  destopNotify(msg: string, requireInteraction: boolean = false) {
    // Push.create('南京晶捷生物公共服务平台消息', {
    //   body: msg,
    //   icon: abp.appPath + 'assets/images/logos/logo-64.png',
    //   timeout: 1000 * 10 * 5,
    //   requireInteraction: requireInteraction,
    //   onClick: function () {
    //     window.focus();
    //     this.close();
    //   }
    // });
  }

  data: any[] = [];
  initData() {
    this.data = [
      {
        title: '通知',
        list: [],
        emptyText: '你已查看所有通知',
        emptyImage:
          'https://gw.alipayobjects.com/zos/rmsportal/wAhyIChODzsoKIOBHcBk.svg',
        clearText: '查看全部',
      },
      {
        title: '消息',
        list: [],
        emptyText: '您已读完所有消息',
        emptyImage:
          'https://gw.alipayobjects.com/zos/rmsportal/sAuJeJzSKbUmHfBQRzmZ.svg',
        clearText: '查看全部',
      },
      {
        title: '待办',
        list: [],
        emptyText: '你已完成所有待办',
        emptyImage:
          'https://gw.alipayobjects.com/zos/rmsportal/HsIsxMZiWKrNUavQUXqx.svg',
        clearText: '查看全部',
      },
    ];
  }


  count = 0;
  loading = false;

  loadData() {
    this.ngZone.run(() => {
      this.notificationServiceProxy.getUserNotifications(undefined, undefined, undefined, 0, 10)
        .subscribe(res => {
          this.initData();
          this.count = res.unreadCount;

          abp.event.trigger('notifychange', res.unreadCount);
          //this.data
          res.items.forEach(ele => {

            let item = {
              id: ele.id,
              title: abp.utils.truncateStringWithPostfix(ele.notification.data.properties['Message'], 50, '...'),
              datetime: dayjs(ele.notification.creationTime).format('YYYY-MM-DD HH:mm:ss'),
              type: '通知',
              exrea: '通知',
              link: '',
              read: ele.state == 1,
            };
            switch (ele.notification.notificationName) {
              case 'NotifyConsts.ReplyAuditReSearch'://回复评论
              case 'NotifyConsts.AuditReSearch':
                item.type = 'ReplyAuditReSearch';
                item.link = ele.notification.data.properties['Link'];
              default:
                break;
            }
            this.data[0].list.push(item);
          });
          // console.log(this.data);
        });
    });

  }

  clear(type: string): void {
    this.router.navigateByUrl('/app/notication');
  }

  select(res: any): void {
    if (res.item.link) {
      //
      this.router.navigateByUrl(res.item.link);
    } else {
      this.router.navigateByUrl(`/app/notication/detail/${res.item.id}`);
    }
    // if (res.item.type == "ReplyAuditReSearch") {
    //   //
    //   this.router.navigateByUrl(res.item.link);
    // }
    if (!res.item.read) {
      let input = new GuidEntityDto();
      input.id = res.item.id;
      this.notificationServiceProxy.setNotificationAsRead(input)
        .subscribe(res => {
          this.loadData();
        });
    }
  }
}
