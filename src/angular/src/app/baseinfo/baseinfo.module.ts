import { SharedModule } from './../../shared/shared.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BaseinfoRoutingModule } from './baseinfo-routing.module';
import { CompanyComponent } from './company/company.component';
import { EditCompanyComponent } from './company/edit-company/edit-company.component';
import { DictComponent } from './dict/dict.component';
import { EditDictComponent } from './dict/edit-dict/edit-dict.component';
import { LocationComponent } from './location/location.component';
import { EditLocationComponent } from './location/edit-location/edit-location.component';
import { ReagentComponent } from './reagent/reagent.component';
import { EditReagentComponent } from './reagent/edit-reagent/edit-reagent.component';
import { StatisticalComponent } from './statistical/statistical.component';

import { NgxEchartsModule } from 'ngx-echarts';
import { ReagentModule } from '@app/reagent/reagent.module';
import { DeletedReagentComponent } from './reagent/deleted-reagent/deleted-reagent.component';
import { ImportByExcelComponent } from './reagent/import-by-excel/import-by-excel.component';
@NgModule({
  declarations: [
    CompanyComponent,
    EditCompanyComponent,
    DictComponent,
    EditDictComponent,
    LocationComponent,
    EditLocationComponent,
    ReagentComponent,
    EditReagentComponent,
    StatisticalComponent,
    DeletedReagentComponent,
    ImportByExcelComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BaseinfoRoutingModule,
    ReagentModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts'),
    }),
  ]
})
export class BaseinfoModule { }
