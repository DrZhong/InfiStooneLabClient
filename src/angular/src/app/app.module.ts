import { ElementRef, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '@app/app-routing.module';
import { AppComponent } from '@app/app.component';

import { LocalizationService } from 'abp-ng2-module';
import { LayoutModule } from '@app/layout/layout.module';
import { HomeComponent } from '@app/home/home.component';
import { SharedModule } from '@shared/shared.module';
import { NoticationComponent } from './notication/notication.component';
import { NoticationDetailComponent } from './notication/notication-detail/notication-detail.component';
import { SelectCurrentWarehouseComponent } from './shared/select-current-warehouse/select-current-warehouse.component';

@NgModule({
    imports: [
        // BrowserModule,
        // BrowserAnimationsModule,
        // HttpClientModule,
        AppRoutingModule,
        LayoutModule,
        SharedModule
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        NoticationComponent,
        NoticationDetailComponent,
        SelectCurrentWarehouseComponent,
    ],
    providers: [LocalizationService]
})
export class AppModule { }
