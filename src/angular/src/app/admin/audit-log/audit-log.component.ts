import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { SFSchema } from '@delon/form';
import dayjs from 'dayjs';
import { AuditLogServiceProxy, AuditLogListDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';

import { finalize } from 'rxjs/operators';
import { AuditDetailComponent } from './audit-detail/audit-detail.component';
import { STColumn, STComponent } from '@delon/abc/st';
//import { UserInfoComponent } from './user-info/user-info.component';

@Component({
  selector: 'app-audit-log',
  templateUrl: './audit-log.component.html',
  styles: []
})
export class AuditLogComponent extends PagedListingComponentBase<AuditLogListDto> implements OnInit {

  startDate: Date;//= moment().startOf('day');
  endDate: Date;// = moment().endOf('day');
  userId: number;
  userName: string;
  hasException: boolean;
  methodName: string | undefined;
  // searchEntity = { 
  //   //startDate: [this.startDate.format(), this.endDate.format()]
  // };

  dateRange = [dayjs().startOf('day').toDate(), dayjs().endOf('day').toDate()];
  onDataChange($e) {
    // console.log(this.dateRange);
    // this.searchEntity.startDate = this.dateRange[0];
    // this.searchEntity.endDate = this.dateRange[1];
  }


  constructor(
    public auditLogServiceProxy: AuditLogServiceProxy,
    injector: Injector) {
    super(injector);
  }
  reset(p) { }

  search() {
    this.pageNumber = 1;
    this.refresh();
  }

  protected delete(entity: AuditLogListDto): void {
    throw new Error("Method not implemented.");
  }

  hasExceptionList = [
    { name: '全部', value: '' },
    { name: '有错误', value: true }
  ];


  protected fetchData(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void {

    if (this.dateRange.length > 0) {
      this.startDate = this.dateRange[0];////this.searchEntity.startDate;
      this.endDate = this.dateRange[1];;
    } else {
      this.startDate = undefined;////this.searchEntity.startDate;
      this.endDate = undefined;;
    }

    this.auditLogServiceProxy.getAuditLogs(this.startDate, this.endDate, this.userName, this.userId, '', this.methodName, '', this.hasException, undefined, undefined, '', request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe(res => {

        this.dataList = res.items;
        this.totalItems = res.totalCount;

      });
  }


  showBrowserInfo: boolean = false;
  @ViewChild("st") st: STComponent;
  showBrowserInfoChange(e) {
    this.st.resetColumns();
  }

  columns: STColumn[] = [
    {
      title: '查看',
      buttons: [
        {
          text: '查看',
          //component: AuditDetailComponent,
          click: (record: any) => {
            this.modalHelper.createStatic(AuditDetailComponent, { record: record }).subscribe(w => {

            })
          }
          //this.edit(record)

        }
      ],
    },
    {
      title: '状态',
      render: 'custom'
    },
    { title: '访问时间', type: 'date', index: 'executionTime' },
    {
      title: '用户', index: 'userName', render: 'userName',
    },
    { title: '服务名', index: 'serviceName' },
    { title: '方法名', index: 'methodName' },
    { title: '耗时', index: 'executionDuration' },
    { title: '访问者IP', index: 'clientIpAddress' },
    { title: '浏览器', index: 'browserInfo', iif: () => this.showBrowserInfo }
  ];

  openUserInfo(record: any) {
    // this.modalHelper.createStatic(UserInfoComponent, { i: record })
    //   .subscribe(() => {

    //   })
  }


  schema: SFSchema = {
    properties: {
      startDate: {
        type: 'string',
        'format': 'date-time',
        title: '日期',
        ui: { widget: 'date', mode: 'range', showTime: true }
      }
    }
  };


  scoll = { x: '1800px' };

}
