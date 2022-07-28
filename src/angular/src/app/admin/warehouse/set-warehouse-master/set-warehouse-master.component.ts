import { Component, Injector, Input, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { WareHouseServiceProxy, WareHouseDto, SetMasterDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-set-warehouse-master',
  templateUrl: './set-warehouse-master.component.html',
  styles: [
  ]
})
export class SetWarehouseMasterComponent extends ModalComponentBase {

  constructor(
    private wareHouseServiceProxy: WareHouseServiceProxy,
    injector: Injector) {
    super(injector);
  }
  ngOnInit(): void {

  }


  @Input() record: WareHouseDto;

  loading = false;
  save(v: WareHouseDto) {
    this.loading = true;
    let body: SetMasterDto = new SetMasterDto();
    body.userPhone = v.masterUserName;
    body.warehouseId = v.id;
    this.wareHouseServiceProxy.setMaster(body)
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
      warehouseType: {
        type: 'number',
        title: '类型',
        enum: [
          { label: '试剂仓库', value: 1 },
          { label: '耗材仓库', value: 2 },
          { label: '办公仓库', value: 3 },
        ],
        readOnly: true,
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
        readOnly: true
      },
      masterUserName: {
        type: 'string',
        title: '管理员手机号'
      },
    },
    required: ['masterUserName']
  };

}
