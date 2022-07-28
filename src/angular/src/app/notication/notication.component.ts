import { STColumn } from '@delon/abc/st';
import { finalize } from 'rxjs/operators';
import { Component, OnInit, Injector } from '@angular/core';
import dayjs from 'dayjs';
import { Router } from '@angular/router';
import { NotificationServiceProxy, UserNotificationDto, UserNotificationState } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
@Component({
  selector: 'app-notication',
  templateUrl: './notication.component.html',
  styles: [
  ]
})
export class NoticationComponent extends PagedListingComponentBase<UserNotificationDto> implements OnInit {


  dateRange = [dayjs().startOf('day').add(-1, 'months').toDate(), dayjs().endOf('day').toDate()];
  state: UserNotificationState | undefined;
  startDate: Date | null | undefined;
  endDate: Date | null | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    if (this.dateRange.length > 0) {
      this.startDate = this.dateRange[0];////this.searchEntity.startDate;
      this.endDate = this.dateRange[1];;
    } else {
      this.startDate = undefined;////this.searchEntity.startDate;
      this.endDate = undefined;;
    }
    this.notificationServiceProxy.getUserNotifications(this.state, this.startDate, this.endDate, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe(res => {
        res.items.forEach(ele => {
          ele['title'] = abp.utils.truncateStringWithPostfix(ele.notification.data.properties['Message'], 50, '...');
        });

        this.dataList = res.items;
        this.totalItems = res.totalCount;

      });
  }
  protected delete(entity: UserNotificationDto): void {
    this.notificationServiceProxy.deleteNotification(entity.id).subscribe(res => {
      this.notify.success('删除成功');
      this.refresh();
    });

  }

  constructor(
    injector: Injector,
    private router: Router,
    private notificationServiceProxy: NotificationServiceProxy) {
    super(injector);
  }
  setAllAsRead() {
    this.notificationServiceProxy.setAllNotificationsAsRead()
      .subscribe(res => {
        abp.event.trigger('abp.notifications.received');
        this.refresh();
      })
  }


  columns: STColumn[] = [
    {
      title: '查看',
      width: 120,
      buttons: [
        {
          text: '设为已读',
          iif: (item: UserNotificationDto) => item.state == 0,
          click: (item: any) => {
            this.notificationServiceProxy.setNotificationAsRead(item).subscribe(res => {
              this.notify.success('设置成功');
            })
          }
        }
      ],
    },
    { title: '消息内容', index: 'title', type: 'link', click: (item) => { return `/app/notication/detail/${item.id}` } },
    { title: '时间', width: 170, index: 'notification.creationTime', type: 'date' },
    {
      title: '等级', width: 100, index: 'notification.severity', type: 'tag', tag: {
        0: { text: 'Info', color: 'blue', },
        1: { text: 'Success', color: 'success' },
        2: { text: 'Warn', color: 'orange' },
        3: { text: 'Error', color: 'red' },
        4: { text: 'Fatal', color: 'purple' }
      }
    },
    {
      title: '状态', width: 100, index: 'state', type: 'badge', badge: {
        0: { text: '未读', color: 'error' },
        1: { text: '已读', color: 'default' }
      }
    },

  ];

  scoll = { x: '1800px' };


}
