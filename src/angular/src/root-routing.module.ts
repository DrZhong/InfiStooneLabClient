import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/app/home', pathMatch: 'full' },
  {
    path: 'account',
    loadChildren: () => import('account/account.module').then(m => m.AccountModule)
  },

  {
    path: 'exception',
    loadChildren: () => import('app/exception/exception.module').then(m => m.ExceptionModule)
  },
  //{ path: '**', redirectTo: 'exception/404' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [],
})
export class RootRoutingModule { }
