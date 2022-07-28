import {
  Component,
  ViewContainerRef,
  OnInit,
  ViewEncapsulation,
  Injector,
} from '@angular/core';
import { LoginService } from './login/login.service';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'layout-account',

  templateUrl: './account.component.html',
  styleUrls: ['./account.component.less'],
})
export class AccountComponent extends AppComponentBase {
  private viewContainerRef: ViewContainerRef;

  versionText: string;
  currentYear: number;

  links = [
    // {
    //   title: '登录',
    //   href: '/account/login',
    // },
    // {
    //   title: '忘记密码',
    //   href: '/account/forgotpassword',
    // }
    // ,
    // {
    //   title: '注册',
    //   href: '/account/register',
    // },
  ];

  public constructor(injector: Injector, private _loginService: LoginService) {
    super(injector);
    this.currentYear = new Date().getFullYear();
    this.versionText = ""
  }

  showTenantChange(): boolean {
    return abp.multiTenancy.isEnabled;
  }
}
