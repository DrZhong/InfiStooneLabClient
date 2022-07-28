import { Router, ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';

import { AccountServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { FormComponentBase } from '@shared/component-base/form-component-base';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['../login/login.component.less'],
})
export class ResetPasswordComponent extends FormComponentBase<any> implements OnInit {
  protected submitExecute(finisheCallback: Function): void {
    throw new Error('Method not implemented.');
  }
  protected setFormValues(entity: any): void {
    throw new Error('Method not implemented.');
  }
  protected getFormValues(): void {
    throw new Error('Method not implemented.');
  }

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private accountServiceProxy: AccountServiceProxy,
    injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {

    this.activatedRoute.queryParams.subscribe(data => {
      this.entity.userId = data['userid'];
      this.entity.resetCode = data['code'];
      this.init();
    })
    // ['userid'];
    // this.entity.resetCode = this.activatedRoute.queryParams['code'];
  }

  userName = '';
  init() {

  }

  submitting = false;
  entity: any = {}
  save() {
    this.submitting = true;

  }

}
