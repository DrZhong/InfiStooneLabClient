import { Component, Injector, Input, OnInit } from '@angular/core';
import { STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ClientStockDto, NormalReagentStockServiceProxy, NormalReagentStockListDto } from '@shared/service-proxies/service-proxies';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-common-search-detail',
  templateUrl: './common-search-detail.component.html',
  styles: [
  ]
})
export class CommonSearchDetailComponent extends PagedListingComponentBase<NormalReagentStockListDto>   {


  close() {
    this.modalRef.close();
  }

  regentNo: string;
  stockStatus: number | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    if (this.stockStatus == null) {
      this.stockStatus = undefined;
    }

    this.normalReagentStockServiceProxy.getNormalReagentStockDetailByNo(this.record.no, this.stockStatus, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe(res => {
        this.dataList = res.items,
          this.totalItems = res.totalCount;
      });
  }
  protected delete(entity: NormalReagentStockListDto): void {
    throw new Error('Method not implemented.');
  }

  @Input() record: ClientStockDto;
  constructor(
    injector: Injector,
    private normalReagentStockServiceProxy: NormalReagentStockServiceProxy,
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
      title: '存放位置', width: 220, index: 'warehouseName',
      format: (item: NormalReagentStockListDto) => {
        return `${item.warehouseName} [${item.locationName}]`;
      },
      className: 'text-truncate'
    },
    {
      title: '试剂信息', width: 220, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },
    { title: '批次数量', width: 120, index: 'amount', className: 'text-truncate' },
    { title: '实际在库数量', width: 120, index: 'realAmount', className: 'text-truncate' },
    {
      title: '规格',
      format: (item: NormalReagentStockListDto) => {
        return `${item.capacity} ${item.capacityUnit}`;
      }
      , width: 100, index: 'capacity', className: 'text-truncate'
    },
    { title: '纯度', width: 100, index: 'reagentPurity', className: 'text-truncate' },
    { title: '生产日期', index: 'productionDate', type: 'date', dateFormat: 'yyyy-MM-dd', width: 120 },
    { title: '保质期', index: 'expirationDate', type: 'date', dateFormat: 'yyyy-MM-dd', width: 120 },
    { title: '批次号', width: 120, index: 'batchNo', className: 'text-truncate' },
    // {
    //   title: '容量', width: 100, index: 'capacity', format: (item: ReagentStockListDto) => {
    //     return `${item.capacity}${item.capacityUnit}`;
    //   }, className: 'text-truncate'
    // },

    { title: '供应商', width: 120, index: 'supplierCompanyName', className: 'text-truncate' },
    { title: '首次入库时间', width: 140, type: 'date', index: 'stockInTime', className: 'text-truncate' },
    { title: '最后一次入库人', width: 120, index: 'latestStockInUserName', className: 'text-truncate' },
    { title: '最新出库时间', width: 140, type: 'date', index: 'latestStockOutTime', className: 'text-truncate' },
    { title: '最新入库人', width: 120, index: 'latestStockOutUserName', className: 'text-truncate' }
  ];


  scroll = {
    x: '1000px'
  }


}
