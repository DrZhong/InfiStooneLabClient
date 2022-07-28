import { SharedModule } from './../../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReagentRoutingModule } from './reagent-routing.module';
import { MasterComponent } from './stockin/master/master.component';
import { CommonComponent } from './stockin/common/common.component';
import { MasterRecordComponent } from './record/master-record/master-record.component';
import { CommonRecordComponent } from './record/common-record/common-record.component';
import { MasterSearchComponent } from './search/master-search/master-search.component';
import { CommonSearchComponent } from './search/common-search/common-search.component';
import { VideoMonitorComponent } from './equipment/video-monitor/video-monitor.component';
import { TemperMonitorComponent } from './equipment/temper-monitor/temper-monitor.component';
import { MasterStockInComponent } from './stockin/master/master-stock-in/master-stock-in.component';
import { SelectReagentComponent } from './shared/select-reagent/select-reagent.component';
import { ShowReagentDetailComponent } from './shared/show-reagent-detail/show-reagent-detail.component';
import { MasterSearchDetailComponent } from './search/master-search/master-search-detail/master-search-detail.component';
import { DoubleConfirmComponent } from './stockin/double-confirm/double-confirm.component';
import { ClientConfirmComponent } from './stockin/client-confirm/client-confirm.component';
import { ReagentStockAuditDialogComponent } from './stockin/reagent-stock-audit-dialog/reagent-stock-audit-dialog.component';
import { CommonStockInComponent } from './stockin/common/common-stock-in/common-stock-in.component';
import { RecordHistoryComponent } from './stockin/common/record-history/record-history.component';
import { CommonSearchDetailComponent } from './search/common-search/common-search-detail/common-search-detail.component';
import { MyOutOrderComponent } from './outorder/my-out-order/my-out-order.component';
import { AllOutOrderComponent } from './outorder/all-out-order/all-out-order.component';
import { CreateOutStockComponent } from './outorder/my-out-order/create-out-stock/create-out-stock.component';
import { SelectMasterStockComponent } from './outorder/shared/select-master-stock/select-master-stock.component';
import { SelectCommonStockComponent } from './outorder/shared/select-common-stock/select-common-stock.component';
import { OrderDoubleConfirmComponent } from './outorder/order-double-confirm/order-double-confirm.component';
import { OrderClientConfirmComponent } from './outorder/order-client-confirm/order-client-confirm.component';
import { OrderItemAuditDialogComponentComponent } from './outorder/order-item-audit-dialog-component/order-item-audit-dialog-component.component';
import { QuicklyBindComponent } from './stockin/common/quickly-bind/quickly-bind.component';
import { MasterQuicklyBindComponent } from './stockin/master/master-quickly-bind/master-quickly-bind.component';


@NgModule({
  declarations: [
    MasterComponent,
    CommonComponent,
    MasterRecordComponent,
    CommonRecordComponent,
    MasterSearchComponent,
    CommonSearchComponent,
    VideoMonitorComponent,
    TemperMonitorComponent,
    MasterStockInComponent,
    SelectReagentComponent,
    ShowReagentDetailComponent,
    MasterSearchDetailComponent,
    DoubleConfirmComponent,
    ClientConfirmComponent,
    ReagentStockAuditDialogComponent,
    CommonStockInComponent,
    RecordHistoryComponent,
    CommonSearchDetailComponent,
    MyOutOrderComponent,
    AllOutOrderComponent,
    CreateOutStockComponent,
    SelectMasterStockComponent,
    SelectCommonStockComponent,
    OrderDoubleConfirmComponent,
    OrderClientConfirmComponent,
    OrderItemAuditDialogComponentComponent,
    QuicklyBindComponent,
    MasterQuicklyBindComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ReagentRoutingModule
  ],
  exports: [
    SelectReagentComponent
  ]
})
export class ReagentModule { }
