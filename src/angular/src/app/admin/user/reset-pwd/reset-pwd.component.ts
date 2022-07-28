import { UserServiceProxy, ResetPasswordDto, UserDto } from './../../../../shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, OnInit, Injector, Input } from '@angular/core';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-reset-pwd',
  templateUrl: './reset-pwd.component.html',
  styles: [
  ]
})
export class ResetPwdComponent extends ModalComponentBase {

  i: ResetPasswordDto = new ResetPasswordDto();

  @Input()
  record: UserDto;
  loading = false;
  constructor(
    private userServiceProxy: UserServiceProxy,
    injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    this.i['user'] = this.record.userName;
  }


  schema: SFSchema = {
    type: "object",
    properties: {
      user: {
        type: 'string',
        title: '用户',
        ui: {
          widget: 'text'
        }
      },
      adminPassword: {
        type: 'string',
        title: '管理员密码',
        maxLength: 64,
        ui: {
          placeholder: '请输入管理员密码',
          type: 'password'
        }
      },
      newPassword: {
        type: 'string',
        title: '重置的密码',
        maxLength: 64,
        ui: {
          placeholder: '请输入为此用户重置的密码'
        }
      }
    },
    required: ['adminPassword', 'newPassWord']
  };

  save(val: ResetPasswordDto) {

    this.loading = true;
    val.userId = this.record.id;
    this.userServiceProxy.resetPassword(val)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(w => {
        this.notify.success('重置成功');
        this.success(1);
      })
  }

}
