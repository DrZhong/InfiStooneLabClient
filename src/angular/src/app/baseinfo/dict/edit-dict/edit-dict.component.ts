import { DictServiceProxy, DictDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { finalize } from 'rxjs/operators';
import { SFSchema } from '@delon/form';
import { of } from 'rxjs';

@Component({
  selector: 'app-edit-dict',
  templateUrl: './edit-dict.component.html',
  styles: [
  ]
})
export class EditDictComponent extends ModalComponentBase {

  constructor(
    private dictServiceProxy: DictServiceProxy,
    injector: Injector) {
    super(injector);
  }

  @Input() record: DictDto;
  @Input() parent: DictDto[] = [];

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    console.log(this.parent);
  }

  loading = false;
  save(v: DictDto) {
    this.loading = true;
    if (this.record.id) {
      //update
      this.dictServiceProxy.update(v)
        .pipe(finalize(() => {
          this.loading = false;
        })).subscribe((x) => {
          abp.notify.success('修改成功');
          this.success(1);
        });
    } else {
      //create
      this.dictServiceProxy.create(v)
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

      parentId: {
        type: 'number',
        title: '父节点',
        ui: {
          widget: 'select',
          asyncData: () => {
            return of(this.parent.map(w => {
              return {
                label: w.name,
                value: w.id
              }
            }))
          },
          placeholder: '选择父节点，置空为根字典'
        },
      },
      name: {
        type: 'string',
        title: '字典名称'
      },
      value: {
        type: 'string',
        title: '字典值'
      },
      sort: {
        title: '顺序',
        type: 'number',
        default: 0
      }
    },
    required: ['name']
  };

}
