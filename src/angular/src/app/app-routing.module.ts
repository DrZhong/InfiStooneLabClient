import { BaseinfoModule } from './baseinfo/baseinfo.module';
import { NoticationDetailComponent } from './notication/notication-detail/notication-detail.component';
import { NoticationComponent } from './notication/notication.component';

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from '@app/home/home.component';
import { AppComponent } from '@app/app.component';
const routes: Routes = [
  {
    path: 'app',
    component: AppComponent,
    canActivate: [AppRouteGuard],
    canActivateChild: [AppRouteGuard],
    children: [
      {
        path: 'home',
        component: HomeComponent,
        data: { title: '首页' },
        canActivate: [AppRouteGuard],
      },
      {
        path: 'notication',
        //component: NoticationComponent, //NoticationComponent,
        canActivate: [AppRouteGuard],
        children: [
          {
            path: '',
            component: NoticationComponent, //NoticationComponent,
          },
          {
            path: 'detail/:id',
            component: NoticationDetailComponent, //NoticationComponent,
          }
        ]
      },
      {
        path: 'admin',
        loadChildren: () => import('app/admin/admin.module').then(m => m.AdminModule)
      },
      {
        path: 'baseinfo',
        loadChildren: () => import('app/baseinfo/baseinfo.module').then(m => m.BaseinfoModule)
      },
      {
        path: 'reagent',//reagent
        loadChildren: () => import('app/reagent/reagent.module').then(m => m.ReagentModule)
      },
      {
        path: '**',
        redirectTo: 'home',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
