import {
  Component,
  Injector,
  AfterViewInit,
  OnInit,
} from '@angular/core';
import { Router } from '@angular/router';
import { Validators } from '@angular/forms';

import {
  AccountServiceProxy,
  RegisterInput,
  RegisterOutput,
} from '@shared/service-proxies/service-proxies';


import { FormComponentBase } from '@shared/component-base/form-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';

import { LoginService } from '../login/login.service';
import { finalize } from 'rxjs/operators';

@Component({
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.less'],
  animations: [appModuleAnimation()],
})
export class RegisterComponent extends FormComponentBase<RegisterInput> implements OnInit {


  model: RegisterInput;

  constructor(
    injector: Injector,
    private _accountService: AccountServiceProxy,
    private _router: Router,
    private readonly _loginService: LoginService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    if (!this.appSession.tenant) {
      this.back();
      return;
    }

    this.model = new RegisterInput();

    this.model.surname = '.';
    // this.validateForm = this.formBuilder.group({
    //   name: ['', [Validators.required]],
    //   surname: ['', [Validators.required]],
    //   emailAddress: ['', [Validators.required, Validators.email]],
    //   userName: ['', [Validators.required]],
    //   password: ['', [Validators.required]]
    // });
  }

  back(): void {
    this._router.navigate(['/']);
  }

  submitting = false;

  protected submitExecute(finisheCallback: Function): void {
    this.submitting = true;
    this._accountService
      .register(this.model)
      .pipe(finalize(() => {
        finisheCallback();
        this.submitting = false;
      }))
      .subscribe((result: RegisterOutput) => {
        if (!result.canLogin) {
          this.notify.success('注册成功，请耐心等待管理员审核后才可以登录到系统！');
          this._router.navigateByUrl('/account/login');
          return;
        }

        this.saving = true;

        // Autheticate
        this._loginService.authenticateModel.userNameOrEmailAddress = this.model.userName;
        this._loginService.authenticateModel.password = this.model.password;
        this._loginService.authenticate(() => {
          this.saving = false;
        }, '/app/home');
      });
  }
  protected setFormValues(entity: RegisterInput): void {

  }
  protected getFormValues(): void {
    // this.model.name = this.getControlVal('name');
    // this.model.surname = this.getControlVal('surname');
    // this.model.emailAddress = this.getControlVal('emailAddress');
    // this.model.userName = this.getControlVal('userName');
    // this.model.password = this.getControlVal('password');
  }
}
