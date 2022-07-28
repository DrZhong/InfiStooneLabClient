import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExceptionRoutingModule } from './exception-routing.module';
import { Page404Component } from './page404/page404.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [Page404Component],
  imports: [
    CommonModule,
    SharedModule,
    ExceptionRoutingModule
  ]
})
export class ExceptionModule { }
