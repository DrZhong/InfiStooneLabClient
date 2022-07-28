import { ClientConfirmComponent } from './stockin/client-confirm/client-confirm.component';
import { DoubleConfirmComponent } from './stockin/double-confirm/double-confirm.component';
import { CommonSearchComponent } from './search/common-search/common-search.component';
import { MasterSearchComponent } from './search/master-search/master-search.component';
import { TemperMonitorComponent } from './equipment/temper-monitor/temper-monitor.component';
import { VideoMonitorComponent } from './equipment/video-monitor/video-monitor.component';
import { CommonRecordComponent } from './record/common-record/common-record.component';
import { MasterRecordComponent } from './record/master-record/master-record.component';
import { MasterComponent } from './stockin/master/master.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { CommonComponent } from './stockin/common/common.component';
import { AllOutOrderComponent } from './outorder/all-out-order/all-out-order.component';
import { MyOutOrderComponent } from './outorder/my-out-order/my-out-order.component';
import { OrderDoubleConfirmComponent } from './outorder/order-double-confirm/order-double-confirm.component';
import { OrderClientConfirmComponent } from './outorder/order-client-confirm/order-client-confirm.component';


const routes: Routes = [{
  path: '',
  children: [
    {
      path: 'master',
      component: MasterComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'common',
      component: CommonComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'master-record',
      component: MasterRecordComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'common-record',
      component: CommonRecordComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'master-search',
      component: MasterSearchComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'common-search',
      component: CommonSearchComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'video-monitor',
      component: VideoMonitorComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'temper-monitor',
      component: TemperMonitorComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'doubleConfirm',
      component: DoubleConfirmComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'clientConfirm',
      component: ClientConfirmComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'myoutorder',
      component: MyOutOrderComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'alloutorder',
      component: AllOutOrderComponent,
      canActivate: [AppRouteGuard]
    }, {
      path: 'orderdoubleConfirm',
      component: OrderDoubleConfirmComponent,
      canActivate: [AppRouteGuard]
    }, {
      path: 'orderclientConfirm',
      component: OrderClientConfirmComponent,
      canActivate: [AppRouteGuard]
    }
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReagentRoutingModule { }
