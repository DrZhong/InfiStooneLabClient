import { Component, Injector, Input, OnInit } from '@angular/core';
import { STColumn } from '@delon/abc/st';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { AuditOutOrderMasterItemDto, OutOrderMasterItemAuditDto, StockoutOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-order-item-audit-dialog-component',
  templateUrl: './order-item-audit-dialog-component.component.html',
  styles: [
  ]
})
export class OrderItemAuditDialogComponentComponent extends ModalComponentBase implements OnInit {

  constructor(
    private stockoutOrderServiceProxy: StockoutOrderServiceProxy,
    injector: Injector) {
    super(injector);
  }
  @Input() i: AuditOutOrderMasterItemDto

  ngOnInit(): void {
    this.refresh();
  }

  dataList: OutOrderMasterItemAuditDto[] = [];
  loading = false;
  refresh() {
    this.loading = true;
    this.stockoutOrderServiceProxy.getOutOrderMasterItemAudit(this.i.id)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(res => {
        this.dataList = res;
      });
  }


  columns: STColumn[] = [
    {
      title: '状态', index: 'reagentStockAuditType',
      type: 'badge',
      badge: {
        1: { text: '入库确认', color: 'error' },
        2: { text: '入库双人双锁', color: 'success' },
        3: { text: '出库确认', color: 'processing' },
        4: { text: '出库双人双锁', color: 'warning' },
      },
      className: 'text-truncate'
    },
    {
      title: '状态', index: 'auditResult',
      type: 'tag',
      tag: {
        0: { text: '待审核', color: 'processing' },
        1: { text: '审核通过', color: 'success' },
        2: { text: '审核不通过', color: 'error' },
      },
      className: 'text-truncate'
    },
    { title: '审核人', index: 'auditUserName', className: 'text-truncate' },
    { title: '审核时间', index: 'creationTime', type: 'date', className: 'text-truncate' },
  ];


}
