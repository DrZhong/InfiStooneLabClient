import { finalize } from 'rxjs/operators';
import { Component, Injector, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';

@Component({
  selector: 'app-send-email',
  templateUrl: './send-email.component.html',
  styles: [
  ]
})
export class SendEmailComponent extends ModalComponentBase implements OnInit {

  i: any;
  constructor(
    public userServiceProxy: UserServiceProxy,
    injector: Injector) {
    super(injector);
  }

  loading = false;
  ngOnInit() {

  }

  save(body: any) {

    this.loading = true;

  }

  schema: SFSchema = {
    type: "object",
    properties: {
      name: {
        type: 'string',
        title: '邮件主题',
        maxLength: 128
      },
      value: {
        type: 'string',
        title: '邮件内容',
        ui: {
          widget: 'ueditor'
        }
      },
    },
    required: ['value', 'name']
  };

}
