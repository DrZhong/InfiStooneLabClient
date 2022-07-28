import { GetAll, WareHouseServiceProxy } from './../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Input, Injector } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { CreateUserDto, UserServiceProxy, UserDto } from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import { SFSchema, SFSchemaEnumType } from '@delon/form';
import { map } from 'rxjs/operators';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-create-or-update-user',
  templateUrl: './create-or-update-user.component.html',
  styles: []
})
export class CreateOrUpdateUserComponent extends ModalComponentBase implements OnInit {

  i: CreateUserDto = new CreateUserDto();

  loading: boolean = false;
  constructor(
    private wareHouseServiceProxy: WareHouseServiceProxy,
    public userServiceProxy: UserServiceProxy,
    public appSessionService: AppSessionService,
    injector: Injector) {
    super(injector);
  }

  ngOnInit() {

  }

  save(v: CreateUserDto) {

    this.userServiceProxy.create(v)
      .pipe(finalize(() => {
        this.loading = false;
      })).subscribe((x) => {
        abp.notify.success('添加成功');
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
      password: {
        type: 'string',
        maxLength: 64,
        title: '密码',
        ui: { type: 'password' }
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
        title: '启用状态',
        type: 'boolean',
        default: true
      },
    },
    required: ['userName', 'name', 'emailAddress']
  };

}
