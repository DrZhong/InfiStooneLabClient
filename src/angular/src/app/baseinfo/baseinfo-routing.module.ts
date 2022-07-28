import { StatisticalComponent } from './statistical/statistical.component';
import { LocationComponent } from './location/location.component';
import { DictComponent } from './dict/dict.component';
import { CompanyComponent } from './company/company.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { ReagentComponent } from './reagent/reagent.component';

const routes: Routes = [{
  path: '',
  children: [
    {
      path: 'company',
      component: CompanyComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'dict',
      component: DictComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'location',
      component: LocationComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'reagent',
      component: ReagentComponent,
      canActivate: [AppRouteGuard]
    },
    {
      path: 'statistical',
      component: StatisticalComponent,
      canActivate: [AppRouteGuard]
    }
  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BaseinfoRoutingModule { }
