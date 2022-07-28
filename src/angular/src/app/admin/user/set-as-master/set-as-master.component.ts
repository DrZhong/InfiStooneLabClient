
import { Component, Injector, Input, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { ResetPasswordDto, UserDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-set-as-master',
  templateUrl: './set-as-master.component.html',
  styles: [
  ]
})
export class SetAsMasterComponent extends ModalComponentBase {


  @Input()
  record: UserDto;
  loading = false;
  constructor(
    private userServiceProxy: UserServiceProxy,
    injector: Injector) {
    super(injector);
  }

  ngOnInit() {
    console.log(this.record);
    //this.i['user'] = this.record.userName;
  }


  schema: SFSchema = {
    type: "object",
    properties: {
      userName: {
        type: 'string',
        title: '手机号',
        ui: {
          widget: 'text'
        }
      },
      name: {
        type: 'string',
        title: '姓名',
        ui: {
          widget: 'text'
        }
      },
      isActive: {
        type: 'boolean',
        title: '状态',
        ui: {
          widget: 'text'
        }
      },
      roleNames: {
        title: '职能',
        type: 'string',
        ui: {
          widget: 'text'
        }
      },
      isMaster: {
        title: '是否为专管',
        type: 'boolean'
      }
    },
    required: ['isMaster']
  };

  save(val: UserDto) {

    this.loading = true;
    if (val.isMaster) {
      this.userServiceProxy.setAsMaster(this.record)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(w => {
          this.notify.success('设置成功');
          this.success(1);
        });
    } else {
      this.userServiceProxy.cancelMaster(this.record)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(w => {
          this.notify.success('设置成功');
          this.success(1);
        });
    }

  }

}
