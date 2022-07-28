import {
  Component,
  Injector,
  ElementRef,
  ViewChild,
  OnInit,
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoginService } from './login.service';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormComponentBase } from '@shared/component-base/form-component-base';
import { AuthenticateModel } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
  animations: [appModuleAnimation()],
})
export class LoginComponent extends FormComponentBase<any> implements OnInit {
  submitting = false;

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _activatedRoute: ActivatedRoute,
    public loginService: LoginService,
    private _router: Router,
  ) {
    super(injector);
  }
  ddLoginUrl = "";

  returnUrl: string = "/app/home";
  callBackUrl = AppConsts.appBaseUrl + "/account/ddCallback";
  ngOnInit(): void {
    this._activatedRoute.queryParams.subscribe(p => {
      if (p['returnUrl']) {
        this.returnUrl = p['returnUrl'];
        this.callBackUrl = this.callBackUrl + "?returnUrl=" + this.returnUrl;
      }
      this.ddLoginUrl = "https://oapi.dingtalk.com/connect/qrconnect?appid=dingoa1hmruheil5xopxlg&response_type=code&scope=snsapi_login&state=STATE&redirect_uri=" + encodeURIComponent(this.callBackUrl);

    })

    this.validateForm = this.fb.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]],
      rememberMe: [true],
    });

    this.loginModel = new AuthenticateModel(); //this.loginService.authenticateModel;
    this.loginModel.rememberClient = true;
  }

  get multiTenancySideIsTeanant(): boolean {
    return this.appSession.tenantId > 0;
  }

  get isSelfRegistrationAllowed(): boolean {
    if (!this.appSession.tenantId) {
      return false;
    }
    return true;
  }


  loginModel: AuthenticateModel;
  protected submitExecute(finisheCallback: Function): void {
    this.submitting = true;
    this.loginService.authenticate(() => (this.submitting = false), this.returnUrl);
  }
  protected setFormValues(entity: any): void { }
  protected getFormValues(): void {
    this.loginService.authenticateModel = this.loginModel;
    this.loginService.rememberMe = this.loginModel.rememberClient;
  }
}
