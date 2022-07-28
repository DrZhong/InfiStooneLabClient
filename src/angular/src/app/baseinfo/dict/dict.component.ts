import { EditDictComponent } from './edit-dict/edit-dict.component';
import { DictDto, DictServiceProxy } from './../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { STColumn } from '@delon/abc/st';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-dict',
  templateUrl: './dict.component.html',
  styles: [
  ]
})
export class DictComponent extends PagedListingComponentBase<DictDto> implements OnInit {
  expandValue = false;
  search() {
    let that = this;
    this.dataList.forEach(ele => {
      ele['expand'] = that.expandValue;
    });
    this.dataList = [...this.dataList];
  }

  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    let that = this;
    this.dictServiceProxy
      .getAll(this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {

        result.items.forEach(ele => {
          ele['expand'] = that.expandValue;
        });
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: DictDto): void {
    this.dictServiceProxy.delete(entity.id)
      .subscribe(res => {
        this.refresh();
      });
  }


  scroll = {
    x: '1200px'
  }

  constructor(
    private dictServiceProxy: DictServiceProxy,
    injector: Injector) {
    super(injector);
  }



  columns: STColumn[] = [
    { title: '名称', index: 'name', className: 'text-truncate' },
    { title: '值', index: 'value', className: 'text-truncate' },
    { title: '顺序', index: 'sort', className: 'text-truncate', width: 120 },
    {
      title: '操作区',
      width: 200,
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

  edit(item: DictDto): void {
    this.modalHelper.createStatic(EditDictComponent, { record: item, parent: this.dataList })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }
  create() {
    this.modalHelper.createStatic(EditDictComponent, { record: {}, parent: this.dataList })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  };

}
