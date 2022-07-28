import { finalize } from 'rxjs/operators';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Input, OnInit, Injector } from '@angular/core';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-user-base-info',
  templateUrl: './user-base-info.component.html',
  styles: [
  ]
})
export class UserBaseInfoComponent extends ModalComponentBase implements OnInit {

  @Input() record;
  i: any;
  loading = false;
  constructor(
    public userServiceProxy: UserServiceProxy,
    injector: Injector) {
    super(injector);
  }

  schema: SFSchema = {
    type: "object",
    properties: {
      birthDate: {
        type: "string",
        format: 'date',
        title: '出生年月'
      },
      height: {
        type: 'integer',
        title: '身高(CM)'
      },
      weight: {
        type: 'integer',
        title: '体重'
      },
      hasIllnessHistory: {
        type: 'boolean',
        title: '是否有既往病史'
      },
      illnessHistoryRemark: {
        type: 'string',
        title: '既往病史备注?'
      },
      hasFamilyDisease: {
        title: '是否有家族病史?',
        type: 'boolean'
      },
      familyDiseaseRemark: {
        title: '家族病史备注',
        type: 'string'
      },
    },
    required: ['birthDate', 'height', 'weight']
  };
  ngOnInit(): void {
    // this.userServiceProxy.getUserBaseInfo(this.record.id)
    //   .subscribe(w => {
    //     this.i = w;
    //   }, err => {
    //     this.close();
    //   })
  }

  save(value) {

    // this.loading = true;
    // this.userServiceProxy.updateUserBaseInfo(value)
    //   .pipe(finalize(() => {
    //     this.loading = false;
    //   }))
    //   .subscribe(q => {
    //     this.notify.success('保存成功！')
    //     this.success(1);
    //   })
  }

}
