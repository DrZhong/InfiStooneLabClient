import { finalize } from 'rxjs/operators';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { ReagentStockAuditDto, ReagentStockListDto, ReagentStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { STColumn } from '@delon/abc/st';

@Component({
  selector: 'app-reagent-stock-audit-dialog',
  templateUrl: './reagent-stock-audit-dialog.component.html',
  styles: [
  ]
})
export class ReagentStockAuditDialogComponent extends ModalComponentBase implements OnInit {

  constructor(
    private reagentStockServiceProxy: ReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }
  @Input() i: ReagentStockListDto

  ngOnInit(): void {
    this.refresh();
  }

  dataList: ReagentStockAuditDto[] = [];
  loading = false;
  refresh() {
    this.loading = true;
    this.reagentStockServiceProxy.getReagentStockAudit(this.i.id)
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
      type: 'tag',
      tag: {
        1: { text: '入库确认', color: 'error' },
        2: { text: '入库双人双锁', color: 'success' },
        3: { text: '出库确认', color: 'processing' },
        4: { text: '出库双人双锁', color: 'warning' },
      },
      className: 'text-truncate'
    },
    { title: '审核人', index: 'auditUserName', className: 'text-truncate' },
    { title: '审核时间', index: 'creationTime', type: 'date', className: 'text-truncate' },
  ];


}
