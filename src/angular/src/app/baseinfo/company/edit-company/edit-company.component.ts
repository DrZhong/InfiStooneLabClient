import { CompanyDto } from './../../../../shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, OnInit, Input } from '@angular/core';
import { CompanyServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-edit-company',
  templateUrl: './edit-company.component.html',
  styles: [
  ]
})
export class EditCompanyComponent extends ModalComponentBase {

  constructor(
    private companyServiceProxy: CompanyServiceProxy,
    injector: Injector) {
    super(injector);
  }

  @Input() record: CompanyDto;

  loading = false;
  save(v: CompanyDto) {
    this.loading = true;
    if (this.record.id) {
      //update
      this.companyServiceProxy.update(v)
        .pipe(finalize(() => {
          this.loading = false;
        })).subscribe((x) => {
          abp.notify.success('修改成功');
          this.success(1);
        });
    } else {
      //create
      this.companyServiceProxy.create(v)
        .pipe(finalize(() => {
          this.loading = false;
        })).subscribe((x) => {
          abp.notify.success('添加成功');
          this.success(1);
        });
    }


  }

  schema: SFSchema = {
    type: "object",
    //ui: { grid: { xs: 24, md: 12 } },
    properties: {
      companyType: {
        type: 'number',
        title: '类型',
        enum: [
          { label: '生产商', value: 0 },
          { label: '供应商', value: 1 }
        ],
        ui: {
          widget: 'radio',
          styleType: 'button',
          buttonStyle: 'solid',
        },
        default: 0,
      },
      name: {
        type: 'string',
        title: '厂家名称',
        maxLength: 64
      },
      pinYin: {
        title: '厂家拼音码',
        type: "string",
      },
      contactName: {
        title: '联系人',
        type: "string",
      },
      contactPhone: {
        title: '联系电话',
        type: "string",
      },
      address: {
        title: '地址',
        type: "string",
      },
      isActive: {
        title: '状态',
        default: true,
        type: 'boolean'
      },
    },
    required: ['name']
  };

}
