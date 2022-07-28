import { Component, Injector, Input, OnInit } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STChange, STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentStockListDto, ReagentStockServiceProxy, ReagentStockStatusEnum, SafeAttributes } from '@shared/service-proxies/service-proxies';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-select-master-stock',
  templateUrl: './select-master-stock.component.html',
  styles: [
  ]
})
export class SelectMasterStockComponent extends PagedListingComponentBase<ReagentStockListDto> implements OnInit {

  barCode: string | undefined;
  reagentCasNo: string | undefined;
  reagentNo: string | undefined;
  batchNo: string | undefined;
  supplierCompanyName: string | undefined;
  @Input()
  warehouseId: number | undefined;
  stockStatus: ReagentStockStatusEnum | undefined;
  filter: string | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    if (this.stockStatus == null) {
      this.stockStatus = undefined;
    }

    this.reagentStockServiceProxy
      .getAllMaster(
        this.barCode,
        this.reagentCasNo,
        this.reagentNo,
        this.batchNo,
        this.supplierCompanyName,
        this.warehouseId,
        ReagentStockStatusEnum.在库,
        this.filter,
        this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: ReagentStockListDto): void {

  }

  wareHouses: any[] = [];
  enumberEntityDto: any[] = [];
  canHuiShou = false;
  ngOnInit(): void {
    this.canHuiShou = this.isGranted('Pages.Reagent.Ruku.HuiShou');
    this.sorting = 'id desc';
    this.refresh();
  }
  show(reagentId: number) {
    this.modalHelper.createStatic(ShowReagentDetailComponent, {
      id: reagentId
    }, {
      size: 1000,
      modalOptions: {
        nzStyle: {
          top: '50px'
        }
      }
    })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }
  search() {
    this.pageNumber = 1;
    this.refresh();
  }

  scroll = {
    x: '1000px'
  }

  constructor(
    private modalRef: NzModalRef,
    private reagentStockServiceProxy: ReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }


  success(result?: any) {
    if (result) {
      this.modalRef.close(result);
    } else {
      this.close();
    }
  }

  close($event?: MouseEvent): void {
    this.modalRef.close();
  }
  select() {
    this.success(this.selectedReagentStockListDto);
  }

  selectedReagentStockListDto: ReagentStockListDto[] = [];

  changeOne($event: STChange) {
    if ($event.type == 'checkbox') {
      this.selectedReagentStockListDto = $event.checkbox as ReagentStockListDto[];
      return;
    }
    this.change($event);
  }

  columns: STColumn[] = [
    { title: 'ID', index: 'id', width: 40, type: 'checkbox', className: 'text-truncate' },
    {
      title: '条形码', width: 120, index: 'barCode', className: 'text-truncate'
    },

    {
      title: '存放位置', width: 150, index: 'warehouseName',
      format: (item: ReagentStockListDto) => {
        return `${item.warehouseName} ${item.reagentSafeAttribute == SafeAttributes.易制毒 ? '易制毒' : '易制爆'}`;
      },
      className: 'text-truncate'
    },
    {
      title: '试剂信息', width: 220, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },
    { title: '锁定状态', width: 100, index: 'lockedOrderId', className: 'text-truncate' },
    {
      title: '规格',
      format: (item: ReagentStockListDto) => {
        return `${item.capacity} ${item.capacityUnit}`;
      }
      , width: 100, index: 'capacity', className: 'text-truncate'
    },
    { title: '纯度', width: 100, index: 'reagentPurity', className: 'text-truncate' },
    { title: '价格', width: 100, index: 'price', type: 'currency' },

    { title: '批次号', width: 120, index: 'batchNo', className: 'text-truncate' },
    {
      title: '容量', width: 100, index: 'capacity', format: (item: ReagentStockListDto) => {
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
    { title: '终端确认', width: 120, index: 'clientConfirm', render: 'clientConfirm', className: 'text-truncate' },
    { title: '双人双锁', width: 120, index: 'doubleConfirm', render: 'doubleConfirm', className: 'text-truncate' },
    { title: '首次入库时间', index: 'firstStockInTime', type: 'date', width: 150 },
    { title: '最新入库时间', index: 'latestStockInTime', type: 'date', width: 150 },
  ];

}
