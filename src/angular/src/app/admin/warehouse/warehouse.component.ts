import { EditWarehouseComponent } from './edit-warehouse/edit-warehouse.component';
import { GetAll, WareHouseDto, WareHouseServiceProxy, WarehouseType } from './../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { STColumn } from '@delon/abc/st';
import { finalize } from 'rxjs/operators';
import { SetWarehouseMasterComponent } from './set-warehouse-master/set-warehouse-master.component';

@Component({
  selector: 'app-warehouse',
  templateUrl: './warehouse.component.html',
  styles: [
  ]
})
export class WarehouseComponent extends PagedListingComponentBase<WareHouseDto> implements OnInit {

  warehouseType: WarehouseType | undefined | null;

  list: WareHouseDto[] = [];
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {


    this.wareHouseServiceProxy
      .getAll(new GetAll())
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.list = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: WareHouseDto): void {
    this.wareHouseServiceProxy.delete(entity.id)
      .subscribe(res => {
        this.refresh();
      });
  }


  scroll = {
    x: '1000px'
  }

  constructor(
    private wareHouseServiceProxy: WareHouseServiceProxy,
    injector: Injector) {
    super(injector);
  }

  search() {
    if (this.warehouseType) {
      this.list = this.dataList.filter(w => w.warehouseType == this.warehouseType);
    } else {
      this.list = this.dataList;
    }

  }

  columns: STColumn[] = [
    {
      title: '仓库类型', index: 'warehouseType', type: 'tag', tag: {
        1: { text: '试剂仓库', color: 'red' },
        2: { text: '耗材仓库', color: 'orange' },
        3: { text: '办公仓库', color: 'green' }
      },
      filter: {
        menus: [
          { text: '试剂仓库', value: 1 },
          { text: '耗材仓库', value: 2 },
          { text: '办公仓库', value: 3 }
        ],
        fn: (f: any, record: WareHouseDto) => {
          console.log(f, record);
          return record.warehouseType == f.value;
        },
        multiple: true
      },
      width: 120
    },
    { title: '仓库名称', index: 'name', className: 'text-truncate', width: 120 },
    { title: '仓库编码', index: 'code', className: 'text-truncate', width: 100 },
    {
      title: '专管试剂提醒策略',
      index: 'zhuanGuanNotifySetting',
      render: 'custom',
      className: 'text-truncate', width: 140
    },
    { title: '管理员姓名', index: 'masterUserName', className: 'text-truncate', width: 120 },
    { title: '管理员手机号', index: 'masterUserUserName', className: 'text-truncate', width: 120 },
    { title: '联系电话', index: 'phone', className: 'text-truncate', width: 120 },
    { title: '仓库地址', index: 'address', className: 'text-truncate', width: 140 },//
    { title: '状态', index: 'isActive', type: 'yn', width: 80 },
    { title: '创建时间', index: 'creationTime', type: 'date', width: 150 },
    {
      title: '操作区',
      width: 180,
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
          text: '仓库管理员',
          click: (record: any) => {
            this.setMaster(record)
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

  setMaster(item: WareHouseDto) {
    //SetWarehouseMasterComponent
    this.modalHelper.createStatic(SetWarehouseMasterComponent, { record: item })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }

  edit(item: WareHouseDto): void {
    this.modalHelper.createStatic(EditWarehouseComponent, { record: item })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }
  create() {
    this.modalHelper.createStatic(EditWarehouseComponent, { record: {} })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  };

}
