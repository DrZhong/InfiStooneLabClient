import { finalize } from 'rxjs/operators';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { ClientStockDto, ReagentStockDto, ReagentStockServiceProxy, SafeAttributes } from '@shared/service-proxies/service-proxies';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { STColumn } from '@delon/abc/st';

@Component({
  selector: 'app-master-search-detail',
  templateUrl: './master-search-detail.component.html',
  styles: [
  ]
})
export class MasterSearchDetailComponent extends PagedListingComponentBase<ReagentStockDto>   {


  close() {
    this.modalRef.close();
  }

  regentNo: string;
  stockStatus: number | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    if (this.stockStatus == null) {
      this.stockStatus = undefined;
    }

    this.reagentStockServiceProxy.getMasterReagentStockDetailByNo(this.record.no, this.stockStatus, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe(res => {
        this.dataList = res.items,
          this.totalItems = res.totalCount;
      });
  }
  protected delete(entity: ReagentStockDto): void {
    throw new Error('Method not implemented.');
  }

  @Input() record: ClientStockDto;
  constructor(
    injector: Injector,
    private reagentStockServiceProxy: ReagentStockServiceProxy,
    private modalRef: NzModalRef) {
    super(injector);
  }

  search() {

    this.pageNumber = 1;
    this.refresh();
  }


  columns: STColumn[] = [
    { title: 'ID', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '条形码', width: 120, index: 'barCode', className: 'text-truncate'
    },

    {
      title: '存放位置', width: 150, index: 'warehouseName',
      format: (item: ReagentStockDto) => {
        return `${item.warehouseName} ${item.reagentSafeAttribute == SafeAttributes.易制毒 ? '易制毒' : '易制爆'}`;
      },
      className: 'text-truncate'
    },
    {
      title: '试剂信息', width: 220, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },
    { title: '重量/g', width: 110, index: 'weight', className: 'text-truncate' },
    {
      title: '规格',
      format: (item: ReagentStockDto) => {
        return `${item.capacity} ${item.capacityUnit}`;
      }
      , width: 100, index: 'capacity', className: 'text-truncate'
    },
    { title: '纯度', width: 100, index: 'reagentPurity', className: 'text-truncate' },

    { title: '批次号', width: 120, index: 'batchNo', className: 'text-truncate' },
    { title: '保质期', width: 140, type: 'date', index: 'expirationDate', className: 'text-truncate' },
    {
      title: '容量', width: 100, index: 'capacity', format: (item: ReagentStockDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },

    { title: '供应商', width: 120, index: 'supplierCompanyName', className: 'text-truncate' },
    {
      title: '状态', width: 100, index: 'stockStatus',
      type: 'tag',
      tag: {
        0: { text: '待入库', color: 'error' },
        1: { text: '在库', color: 'success' },
        2: { text: '离库', color: 'processing' },
        3: { text: '已用完', color: 'warning' },
      },
      className: 'text-truncate'
    },
    { title: '入库人', width: 100, index: 'latestStockInUserName', className: 'text-truncate' },
    { title: '首次入库时间', index: 'firstStockInTime', type: 'date', width: 150 },
    { title: '最新入库时间', index: 'latestStockInTime', type: 'date', width: 150 },
    { title: '最后一次出库人', width: 120, index: 'latestStockOutUserName', className: 'text-truncate' },
    { title: '最后一次出库日期', index: 'latestStockOutTime', type: 'date', width: 150 },
    { title: '创建人', index: 'CreateUserName', type: 'date', width: 100 },
  ];


  scroll = {
    x: '1000px'
  }


}
