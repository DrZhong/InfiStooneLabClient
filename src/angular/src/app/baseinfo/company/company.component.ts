import { EditCompanyComponent } from './edit-company/edit-company.component';
import { CompanyDto, CompanyServiceProxy, CompanyType } from './../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { STColumn } from '@delon/abc/st';

@Component({
  selector: 'app-company',
  templateUrl: './company.component.html',
  styles: [
  ]
})
export class CompanyComponent extends PagedListingComponentBase<CompanyDto> implements OnInit {

  filter: string;
  companyType: CompanyType | undefined;
  contactName: string | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.companyType == null) {
      this.companyType = undefined;
    }

    this.companyServiceProxy
      .getAll(this.companyType, this.contactName, this.filter, this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: CompanyDto): void {
    this.companyServiceProxy.delete(entity.id)
      .subscribe(res => {
        this.refresh();
      });
  }


  scroll = {
    x: '1200px'
  }

  constructor(
    private companyServiceProxy: CompanyServiceProxy,
    injector: Injector) {
    super(injector);
  }



  columns: STColumn[] = [
    {
      title: '类型', index: 'companyType', width: 100, type: 'tag', tag: {
        0: { text: '生产商', color: 'red' },
        1: { text: '供应商', color: 'green' }
      }
    },
    { title: '厂家名称', index: 'name', className: 'text-truncate', width: 120 },
    { title: '拼音码', index: 'pinYin', className: 'text-truncate', width: 120 },
    { title: '联系人', index: 'contactName', className: 'text-truncate', width: 110 },
    { title: '联系电话', index: 'contactPhone', className: 'text-truncate', width: 120 },//
    { title: '状态', index: 'isActive', type: 'yn', width: 80 },
    { title: '创建者', index: 'createUserName', className: 'text-truncate', width: 140 },
    { title: '创建时间', index: 'creationTime', type: 'date', width: 150 },
    {
      title: '操作区',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          text: '删除',
          type: 'del',
          click: (record: any) => {
            this.delete(record)
          }
        },
        {
          text: '编辑',
          click: (record: any) => {
            this.edit(record)
          }
        }
      ],
    },
  ];

  edit(item: CompanyDto): void {
    this.modalHelper.createStatic(EditCompanyComponent, { record: item })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }
  create() {
    this.modalHelper.createStatic(EditCompanyComponent, { record: {} })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  };

}
