import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AccountComponent } from './account.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';


@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        component: AccountComponent,

        children: [
          { path: 'login', component: LoginComponent },
          { path: 'register', component: RegisterComponent },
          { path: 'resetpwd', component: ResetPasswordComponent },
          { path: 'forgotpassword', component: ForgotpasswordComponent }
        ],
      },
    ]),
  ],
  exports: [RouterModule],
})
export class AccountRoutingModule { }
