import { UserDto, UserServiceProxy } from './../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Input, Injector } from '@angular/core';
import { finalize, map } from 'rxjs/operators';
import { SFSchema } from '@delon/form';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styles: [
  ]
})
export class EditUserComponent extends ModalComponentBase implements OnInit {

  @Input() i: UserDto;

  loading: boolean = false;
  constructor(
    public userServiceProxy: UserServiceProxy,
    injector: Injector) {
    super(injector);
  }

  ngOnInit() {

  }

  save(v: UserDto) {

    this.userServiceProxy.update(v)
      .pipe(finalize(() => {
        this.loading = false;
      })).subscribe((x) => {
        abp.notify.success('修改成功');
        this.success(1);
      });

  }

  schema: SFSchema = {
    type: "object",
    //ui: { grid: { xs: 24, md: 12 } },
    properties: {
      userName: {
        type: "string",
        title: '手机号',
        maxLength: 64
      },
      name: {
        type: 'string',
        title: '名字',
        maxLength: 64
      },
      emailAddress: {
        type: 'string',
        title: '邮箱',
        description: '邮箱用来向用户发送一些站内消息和提醒'
      },
      roleNames: {
        title: '职能',
        type: "string",
        ui: {
          placeholder: '选择一个或多个职能',
          widget: 'select', mode: 'tags',
          asyncData: () => {
            return this.userServiceProxy.getRoles().pipe(map(ele => {
              return ele.items.map(w => {
                return {
                  label: w.displayName,
                  value: w.normalizedName
                };
              });
            }));
          },
        }
      },
      isActive: {
        title: '在职状态',
        type: 'boolean',
        ui: {
          checkedChildren: '在职',
          unCheckedChildren: '离职',
        }
      },
    },
    required: ['userName', 'name', 'emailAddress']
  };

}
