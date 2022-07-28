import { WareHouseDto } from './../../../../shared/service-proxies/service-proxies';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { WareHouseServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { SFSchema } from '@delon/form';

@Component({
  selector: 'app-edit-warehouse',
  templateUrl: './edit-warehouse.component.html',
  styles: [
  ]
})
export class EditWarehouseComponent extends ModalComponentBase {

  constructor(
    private wareHouseServiceProxy: WareHouseServiceProxy,
    injector: Injector) {
    super(injector);
  }
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    if (this.record.id) {
      this.schema.properties.warehouseType.readOnly = true;
    }
  }


  @Input() record: WareHouseDto;

  loading = false;
  save(v: WareHouseDto) {
    this.loading = true;
    if (this.record.id) {
      //update
      this.wareHouseServiceProxy.update(v)
        .pipe(finalize(() => {
          this.loading = false;
        })).subscribe((x) => {
          abp.notify.success('修改成功');
          this.success(1);
        });
    } else {
      //create
      this.wareHouseServiceProxy.create(v)
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
      warehouseType: {
        type: 'number',
        title: '类型',
        enum: [
          { label: '试剂仓库', value: 1 },
          { label: '耗材仓库', value: 2 },
          { label: '办公仓库', value: 3 },
        ],
        readOnly: false,
        ui: {
          widget: 'radio',
          styleType: 'button',
          buttonStyle: 'solid',
        },
        default: 1,
      },
      name: {
        type: 'string',
        title: '仓库名称',
        maxLength: 64
      },
      code: {
        title: '仓库编码',
        type: "string",
      },
      zhuanGuanNotifySetting: {
        type: 'number',
        title: '专管试剂提醒策略',
        enum: [
          { label: '不提醒', value: 0 },
          { label: '提醒一次', value: 1 },
          { label: '周期提醒', value: 2 },
        ],
        readOnly: false,
        ui: {
          widget: 'radio',
          styleType: 'button',
          buttonStyle: 'solid',
        },
        default: 0,
      },
      notifySettingIntervalHour: {
        type: 'number',
        title: '提醒间隔（小时）'
      },
      phone: {
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
