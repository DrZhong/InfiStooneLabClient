import { Component, Injector, OnInit } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { MasterStockInComponent } from '@app/reagent/stockin/master/master-stock-in/master-stock-in.component';
import { STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentOperateRecordDto, OperateTypeEnum, ReagentStockServiceProxy, SafeAttributes, ReagentStockListDto, StockoutOrderServiceProxy, OutOrderTypeEnum, OutOrderStatusEnum, OutOrderDto } from '@shared/service-proxies/service-proxies';
import dayjs from 'dayjs';
import { finalize } from 'rxjs/operators';
import { OrderItemAuditDialogComponentComponent } from '../order-item-audit-dialog-component/order-item-audit-dialog-component.component';
import { CreateOutStockComponent } from './create-out-stock/create-out-stock.component';

@Component({
  selector: 'app-my-out-order',
  templateUrl: './my-out-order.component.html',
  styles: [
  ]
})
export class MyOutOrderComponent extends PagedListingComponentBase<OutOrderDto> implements OnInit {

  outOrderStatus: OutOrderStatusEnum | undefined;
  warehouseId: number | undefined;
  outOrderType: OutOrderTypeEnum | undefined;
  applyUserName: string | undefined;
  filter: string | undefined;
  inCludeItems: boolean = true;


  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    if (this.outOrderStatus == null) {
      this.outOrderStatus = undefined;
    }
    if (this.outOrderType == null) {
      this.outOrderType = undefined;
    }
    this.stockoutOrderServiceProxy
      .getMyOrder(
        this.outOrderStatus,
        this.warehouseId,
        this.outOrderType,
        this.applyUserName,
        this.inCludeItems,
        this.filter,
        this.sorting,
        request.skipCount,
        request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: OutOrderDto): void {
    this.stockoutOrderServiceProxy.cancelOrder(entity.id)
      .subscribe(res => {
        this.refresh();
      });
  }

  wareHouses: any[] = [];
  enumberEntityDto: any[] = [];
  ngOnInit(): void {
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
        // if (x)
        //   this.refresh();
      });
  }
  search() {
    this.pageNumber = 1;
    this.refresh();
  }

  scroll = {
    x: '1000px'
  }
  scroll2 = {
    x: '800px'
  }
  showMore = false;
  constructor(
    private stockoutOrderServiceProxy: StockoutOrderServiceProxy,
    injector: Injector) {
    super(injector);
  }


  shwoAudit(item: any) {
    this.modalHelper.createStatic(OrderItemAuditDialogComponentComponent, { i: item })
      .subscribe(x => {

      });
  }



  columns: STColumn[] = [
    { title: '出库单号', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '所属仓库', width: 100, index: 'warehouseName', className: 'text-truncate'
    },
    {
      title: '订单状态', width: 80, index: 'outOrderStatus',
      type: 'tag',
      tag: {
        0: { text: '待出库', color: 'default' },
        1: { text: '出库完毕', color: 'success' },
        2: { text: '已取消', color: 'error' },
      },
      className: 'text-truncate'
    },
    {
      title: '订单类型', width: 80, index: 'outOrderType',
      type: 'tag',
      tag: {
        1: { text: '专管试剂', color: 'error' },
        2: { text: '普通试剂', color: 'success' },
      },
      className: 'text-truncate'
    },
    { title: '申请人', width: 100, index: 'applyUserName', className: 'text-truncate' },
    { title: '创建时间', index: 'creationTime', className: 'text-truncate', type: 'date', width: 140 },
    {
      title: '操作',
      width: 80,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          text: '取消出库单',
          pop: '你确定要取消出库单吗？',
          type: 'del',
          iif: (item: OutOrderDto) => item.outOrderStatus == OutOrderStatusEnum.待出库,
          click: (record: any) => {
            this.delete(record)
          }
        },
      ],
    },
  ];

  masterItemColumn: STColumn[] = [
    { title: '条码', index: 'reagentStockBarCode', width: 60, className: 'text-truncate' },
    {
      title: '试剂名称', width: 100, index: 'reagentCnName', className: 'text-truncate'
    },
    {
      title: 'CAS号', width: 100, index: 'reagentCasNo', className: 'text-truncate'
    },
    {
      title: '价格', width: 100, index: 'price', type: 'currency'
    },
    {
      title: '批次号', width: 100, index: 'reagentStockBatchNo', className: 'text-truncate'
    },
    {
      title: '位置', width: 100, index: 'locationName', className: 'text-truncate'
    },
    {
      title: '终端确认', width: 100, index: 'clientConfirm', render: 'clientConfirm', className: 'text-truncate'
    },

    { title: '双人双锁', width: 100, index: 'doubleConfirm', render: 'doubleConfirm', className: 'text-truncate' },
    { title: '出库时间', index: 'stockOutTime', width: 140, type: 'date', className: 'text-truncate' },
  ];

  commonItemColumn: STColumn[] = [
    { title: '位置', index: 'locationName', width: 100, className: 'text-truncate' },
    { title: '出库数量', index: 'stockoutAccount', width: 90, className: 'text-truncate' },
    { title: '试剂编号', index: 'reagentNo', width: 80, className: 'text-truncate' },
    { title: 'cas号', index: 'reagentCasNo', width: 80, className: 'text-truncate' },
    {
      title: '价格', width: 100, index: 'price', type: 'currency'
    },
    { title: '试剂名称', index: 'reagentCnName', width: 120, className: 'text-truncate' },//
    { title: '出库时间', index: 'stockOutTime', width: 140, type: 'date', className: 'text-truncate' },
  ];



  create() {
    this.modalHelper.createStatic(CreateOutStockComponent, {}, {
      size: 1000,
      modalOptions: {
        nzClosable: false,
        nzKeyboard: false,
        nzStyle: {
          top: '20px',
        }
      }
    })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  };

}
