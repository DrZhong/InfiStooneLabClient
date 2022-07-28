import { UserDto, PagedResultRequestFilterDto } from './../../../../shared/service-proxies/service-proxies';
import { UserServiceProxy, CommonServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, OnInit, Input, Injector, ViewChild } from '@angular/core';
import { PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { STChange, STColumn } from '@delon/abc/st';

@Component({
  selector: 'app-select-user',
  templateUrl: './select-user.component.html',
  styles: []
})
export class SelectUserComponent extends ModalComponentBase implements OnInit {
  constructor(
    public userServiceProxy: UserServiceProxy,
    private commonServiceProxy: CommonServiceProxy,
    injector: Injector) {
    super(injector);
  }
  @Input() filter: string = "";
  pageIndex = 1;
  paseSize = 10;
  isTableLoading = false;
  total = 0;
  datas: any[] = [];
  ngOnInit() {
    this.loadData();

  }
  ckeckedValue: UserDto[] = [];
  checkboxChange(list: UserDto[]) {
    this.ckeckedValue = list;
  }

  save() {

    this.success(this.ckeckedValue);

  }

  show($even: STChange) {
    if ($even.type == 'pi' || $even.type == 'ps') {
      this.pageIndex = $even.pi;
      this.paseSize = $even.ps;
      this.loadData();
    }
    if ($even.type == 'checkbox') {
      this.ckeckedValue = $even.checkbox as UserDto[];
    }
  }

  loadData() {
    const pageReq = new PagedRequestDto();
    pageReq.maxResultCount = this.paseSize;
    pageReq.skipCount = (this.pageIndex - 1) * this.paseSize;


    this.isTableLoading = true;


    let body: PagedResultRequestFilterDto = new PagedResultRequestFilterDto();
    body.filter = this.filter;
    body.maxResultCount = pageReq.maxResultCount;
    body.skipCount = pageReq.skipCount;
    this.commonServiceProxy.searchUsers(body)
      .pipe(finalize(() => {
        this.isTableLoading = false;
      }))
      .subscribe(res => {

        this.datas = res.items;
        this.total = res.totalCount;

      });
  }



  columns: STColumn[] = [
    {
      title: '编号',
      index: 'value',
      type: 'checkbox'
    },
    {
      title: '手机号',
      index: 'userName'
    },
    {
      title: '姓名',
      index: 'name'
    },
    {
      title: '邮箱',
      index: 'emailAddress'
    }
  ];
}
