import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { AccountRoutingModule } from './account-routing.module';


import { SharedModule } from '@shared/shared.module';

import { AccountComponent } from './account.component';
import { TenantChangeComponent } from './tenant/tenant-change.component';
import { TenantChangeModalComponent } from './tenant/tenant-change-modal.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountLanguagesComponent } from './layout/account-languages.component';

import { LoginService } from './login/login.service';;
import { ResetPasswordComponent } from './reset-password/reset-password.component'
  ;
import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component'
  ;

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        SharedModule,
        AccountRoutingModule
    ],
    declarations: [
        AccountComponent,
        TenantChangeComponent,
        TenantChangeModalComponent,
        LoginComponent,
        RegisterComponent,
        AccountLanguagesComponent,
        ResetPasswordComponent,
        ForgotpasswordComponent
    ],
    providers: [LoginService]
})
export class AccountModule { }
