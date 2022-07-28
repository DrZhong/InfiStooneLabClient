import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { NormalReagentStockListDto, NormalReagentStockServiceProxy, OperateTypeEnum, ReagentOperateRecordDto, ReagentStockListDto } from '@shared/service-proxies/service-proxies';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STColumn } from '@delon/abc/st';
import dayjs from 'dayjs';
import { finalize } from 'rxjs/operators';
import { MasterStockInComponent } from '../../master/master-stock-in/master-stock-in.component';

@Component({
  selector: 'app-record-history',
  templateUrl: './record-history.component.html',
  styles: [
  ]
})
export class RecordHistoryComponent extends PagedListingComponentBase<ReagentOperateRecordDto> implements OnInit {


  @Input() i: NormalReagentStockListDto;

  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {


    this.normalReagentStockServiceProxy
      .getOperateRecordByNormalReagentId(this.i.id + '', this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: ReagentOperateRecordDto): void {

  }
  close() {

  }

  ngOnInit(): void {
    this.sorting = 'id desc';
    this.refresh();
  }

  scroll = {
    x: '1000px'
  }

  constructor(
    private normalReagentStockServiceProxy: NormalReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }





  columns: STColumn[] = [
    { title: 'ID', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '试剂编号', width: 100, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },

    {
      title: '存放位置', width: 200, index: 'warehouseName',
      format: (item: ReagentOperateRecordDto) => {
        return `${item.warehouseName} ${item.locationName}`;
      },
      className: 'text-truncate'
    },
    {
      title: '试剂名称', width: 120, index: 'reagentCnName',
      className: 'text-truncate'
    },
    {
      title: '条形码', width: 100, index: 'barCode', className: 'text-truncate'
    },
    {
      title: '操作数量', width: 100, type: 'number', index: 'operateAmount', className: 'text-truncate'
    },

    //{ title: '纯度', width: 100, index: 'reagentPurity', className: 'text-truncate' },

    { title: '批次号', width: 120, index: 'batchNo', className: 'text-truncate' },
    {
      title: '容量', width: 100, index: 'capacity', format: (item: ReagentOperateRecordDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },

    {
      title: '操作类型', width: 100, index: 'operateType',
      type: 'tag',
      tag: {
        1: { text: '入库', color: 'success' },
        2: { text: '领用', color: 'processing' },
        3: { text: '归还', color: 'warning' },
        4: { text: '回收', color: 'error' },
      },
      className: 'text-truncate'
    },
    { title: '操作人', width: 100, index: 'createUserName', className: 'text-truncate' },
    { title: '操作时间', index: 'creationTime', type: 'date', width: 150 }
  ];



}
