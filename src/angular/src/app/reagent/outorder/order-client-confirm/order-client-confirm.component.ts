import { Component, Injector, OnInit } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { AuditOutOrderMasterItemDto, OutOrderMasterItemStatues, StockoutOrderServiceProxy, SafeAttributes, OrderConfirmEdInputDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { OrderItemAuditDialogComponentComponent } from '../order-item-audit-dialog-component/order-item-audit-dialog-component.component';

@Component({
  selector: 'app-order-client-confirm',
  templateUrl: './order-client-confirm.component.html',
  styles: [
  ]
})
export class OrderClientConfirmComponent extends PagedListingComponentBase<AuditOutOrderMasterItemDto> implements OnInit {

  userName: string | undefined;
  filter: string | undefined;
  warehouseId: number | undefined;
  audited: OutOrderMasterItemStatues | undefined = OutOrderMasterItemStatues.待审核;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    this.stockoutOrderServiceProxy
      .getClientConfirm(this.userName,
        this.warehouseId,
        this.audited,
        this.filter, this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: AuditOutOrderMasterItemDto): void {

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
    private stockoutOrderServiceProxy: StockoutOrderServiceProxy,
    injector: Injector) {
    super(injector);
  }



  columns: STColumn[] = [
    { title: '所属订单', index: 'outOrderId', width: 100, className: 'text-truncate' },
    {
      title: '条形码', width: 120, index: 'reagentStockBarCode', className: 'text-truncate'
    },

    {
      title: '存放位置', width: 150, index: 'warehouseName',
      format: (item: AuditOutOrderMasterItemDto) => {
        return `${item.outOrderWarehouseName} ${item.reagentStockSafeAttribute == SafeAttributes.易制毒 ? '易制毒' : '易制爆'}`;
      },
      className: 'text-truncate'
    },
    {
      title: '试剂信息', width: 220, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },
    {
      title: '规格',
      format: (item: AuditOutOrderMasterItemDto) => {
        return `${item.reagentStockCapacity} ${item.reagentStockCapacityUnit}`;
      }
      , width: 100, index: 'capacity', className: 'text-truncate'
    },
    { title: '纯度', width: 100, index: 'reagentPurity', className: 'text-truncate' },

    { title: '批次号', width: 120, index: 'reagentStockBatchNo', className: 'text-truncate' },
    {
      title: '容量', width: 100, index: 'capacity', format: (item: AuditOutOrderMasterItemDto) => {
        return `${item.reagentStockCapacity}${item.reagentStockCapacityUnit}`;
      }, className: 'text-truncate'
    },

    { title: '终端确认', width: 120, index: 'clientConfirm', render: 'clientConfirm', className: 'text-truncate' },
    { title: '双人双锁', width: 120, index: 'doubleConfirm', render: 'doubleConfirm', className: 'text-truncate' },
    {
      title: '审核',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          text: '不通过',
          pop: '你确定要审核不通过吗？',
          type: 'del',
          iif: (item: AuditOutOrderMasterItemDto) => item.clientConfirmed == OutOrderMasterItemStatues.待审核,
          click: (record: any) => {
            this.audit(record, OutOrderMasterItemStatues.审核不通过)
          }
        },
        {
          text: '通过',
          pop: '你确定要审核通过吗？',
          type: 'del',
          iif: (item: AuditOutOrderMasterItemDto) => item.clientConfirmed == OutOrderMasterItemStatues.待审核,
          click: (record: any) => {
            this.audit(record, OutOrderMasterItemStatues.审核通过)
          }
        },
      ],
    },
  ];


  shwoAudit(item: AuditOutOrderMasterItemDto) {
    this.modalHelper.createStatic(OrderItemAuditDialogComponentComponent, { i: item })
      .subscribe(x => {

      });
  }


  protected audit(entity: AuditOutOrderMasterItemDto, status: OutOrderMasterItemStatues): void {
    let body = new OrderConfirmEdInputDto();
    body.orderItemId = entity.id;
    body.auditResult = status;
    this.stockoutOrderServiceProxy.orderClientConfirmEd(body)
      .subscribe(res => {
        this.refresh();
      });
  }



}
