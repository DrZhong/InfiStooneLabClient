import { UserFingerDto, UserServiceProxy, UserFingerListDto } from './../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { UserDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { UserDetailComponent } from '../user/user-detail/user-detail.component';

@Component({
  selector: 'app-user-finger',
  templateUrl: './user-finger.component.html',
  styles: [
  ]
})
export class UserFingerComponent extends PagedListingComponentBase<UserFingerListDto> implements OnInit {

  filter: string;

  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {


    this.userServiceProxy
      .getAllUserFinger(this.filter, this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: UserFingerListDto): void {
    this.userServiceProxy.deleteUserFinger(entity.id)
      .subscribe(res => {
        this.notify.success('删除成功，请重启客户端并重新初始化指纹配置后才能生效');
        this.refresh();
      })
  }



  constructor(
    private userServiceProxy: UserServiceProxy,
    injector: Injector) {
    super(injector);
  }



  columns: STColumn[] = [
    { title: '序号', index: 'id', className: 'text-truncate', width: 80 },
    { title: '账号/手机号', index: 'userUserName', className: 'text-truncate', width: 120 },
    { title: '姓名', index: 'userName', className: 'text-truncate', width: 100 },
    { title: '录入时间', index: 'creationTime', type: 'date', width: 150 },
    {
      title: '操作区',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          text: '删除',
          type: 'del',
          click: (item) => {
            this.delete(item);
          }
        },
      ],
    },
  ];


}
