import { finalize } from 'rxjs/operators';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { UserServiceProxy, ChangePasswordDto } from './../../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector } from '@angular/core';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-change-pwd',
  templateUrl: './change-pwd.component.html',
  styles: []
})
export class ChangePwdComponent extends ModalComponentBase {

  i: ChangePasswordDto = new ChangePasswordDto();
  loading = false;
  constructor(
    private userServiceProxy: UserServiceProxy,
    injector: Injector) {
    super(injector);
  }

  ngOnInit() {
  }


  schema: SFSchema = {
    type: "object",
    properties: {
      currentPassword: {
        type: "string",
        title: '原密码',
        maxLength: 64,
        ui: {
          placeholder: '请输入原密码'
        }
      },
      newPassword: {
        type: 'string',
        title: '新密码',
        maxLength: 64,
        ui: {
          placeholder: '请输入新密码'
        }
      },
      newPassword1: {
        type: 'string',
        title: '核对新密码',
        maxLength: 64,
        ui: {
          placeholder: '请再次输入新密码'
        }
      }
    },
    required: ['currentPassword', 'newPassWord', 'newPassWord1']
  };

  save(val: ChangePasswordDto) {
    if (val.newPassword != val['newPassword1']) {
      abp.message.warn('您输入的两次新密码不一致！'); return;
    }
    this.loading = true;
    this.userServiceProxy.changePassword(val)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(w => {
        this.notify.success('修改成功');
        this.success(1);
      })
  }

}
