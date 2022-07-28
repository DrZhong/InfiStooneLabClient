import { finalize } from 'rxjs/operators';

import { AccountServiceProxy, } from '@shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, OnInit, Injector } from '@angular/core';
import { FormComponentBase } from '@shared/component-base/form-component-base';
@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['../login/login.component.less'],
})
export class ForgotpasswordComponent extends FormComponentBase<any> implements OnInit {
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
    private accountServiceProxy: AccountServiceProxy,
    injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
  }
  submitting = false;
  entity: any = {};
  save() {

  }

}
