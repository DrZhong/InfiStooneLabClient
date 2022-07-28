import { EditLocationComponent } from './edit-location/edit-location.component';
import { EnumberEntityDto, LocationDto, LocationServiceProxy, StorageAttrEnum, CommonServiceProxy, WareHouseDto } from './../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { STColumn } from '@delon/abc/st';
import { zip } from 'rxjs';
@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styles: [
  ]
})
export class LocationComponent extends PagedListingComponentBase<LocationDto> implements OnInit {

  warehouseId: number | undefined;
  filter: string | undefined;
  storageAttr: StorageAttrEnum | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    if (this.storageAttr == null) {
      this.storageAttr = undefined;
    }

    this.locationServiceProxy
      .getAll(this.warehouseId, this.storageAttr, this.filter, this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: LocationDto): void {
    this.locationServiceProxy.delete(entity.id)
      .subscribe(res => {
        this.refresh();
      });
  }

  wareHouses: WareHouseDto[] = [];
  enumberEntityDto: EnumberEntityDto[] = [];
  ngOnInit(): void {

    this.commonServiceProxy.getStorageAttrList().subscribe((ress) => {

      this.enumberEntityDto = ress;
    });
    this.refresh();
  }

  scroll = {
    x: '1200px'
  }

  constructor(
    private commonServiceProxy: CommonServiceProxy,
    private locationServiceProxy: LocationServiceProxy,
    injector: Injector) {
    super(injector);
  }



  columns: STColumn[] = [

    { title: '库位名称', index: 'name', className: 'text-truncate' },
    { title: '所属仓库', index: 'warehouseName', className: 'text-truncate' },
    { title: '状态', type: 'yn', index: 'isActive', width: 120 },//countLimit
    { title: '库位试剂数量限制', index: 'countLimit', width: 200 },//
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

  edit(item: LocationDto): void {
    this.modalHelper.createStatic(EditLocationComponent, { record: item })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }
  create() {
    this.modalHelper.createStatic(EditLocationComponent, { record: {} })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  };

}
