import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { map, finalize } from 'rxjs/operators';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { SFSchema, SFComponent } from '@delon/form';
import { ClientStockDto, CommonServiceProxy, CreateOutOrderInputDto, CreateOutOrderItem, OutOrderTypeEnum, ReagentStockListDto, StockoutOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { SelectMasterStockComponent } from '../../shared/select-master-stock/select-master-stock.component';
import { SelectCommonStockComponent } from '../../shared/select-common-stock/select-common-stock.component';

@Component({
  selector: 'app-create-out-stock',
  templateUrl: './create-out-stock.component.html',
  styles: [
  ]
})
export class CreateOutStockComponent extends ModalComponentBase implements OnInit {
  constructor(
    private stockoutOrderServiceProxy: StockoutOrderServiceProxy,
    injector: Injector, private commonServiceProxy: CommonServiceProxy) {
    super(injector);
  }
  i: CreateOutOrderInputDto = new CreateOutOrderInputDto();;
  ngOnInit(): void {
  }

  loading = false;
  save(v: CreateOutOrderInputDto) {
    if (v.outOrderType == OutOrderTypeEnum.专管试剂) {
      if (this.masterItems.length == 0) {
        this.notify.warn("至少选择一个专管试剂")
        return;
      }
      v.items = this.masterItems.map(w => {
        let d = new CreateOutOrderItem();
        d.reagentStockId = w.id;
        d.stockoutAccount = 1;
        return d;
      });
    } else {
      //普通试剂
      if (this.commonItems.length == 0) {
        this.notify.warn("至少选择一个普通试剂")
        return;
      }
      v.items = this.commonItems.map(w => {
        let d = new CreateOutOrderItem();
        d.reagentId = w.id;
        d.locationId = w.locationId;
        d.stockoutAccount = w['stockoutAccount'];
        return d;
      });;
    }
    this.loading = true;
    this.stockoutOrderServiceProxy.createOutOrder(v)
      .pipe(finalize(() => {
        this.loading = false;
      })).subscribe(w => {
        this.notify.success('创建成功');
        this.success(1);
      })

  }

  /**
   * 选择专管试剂
   * @param v 
   */
  selectMaster(v: CreateOutOrderInputDto) {
    if (v.outOrderType != OutOrderTypeEnum.专管试剂) {
      this.notify.warn("请先选择 专管试剂！")
      return;
    }
    if (!v.warehouseId) {
      this.notify.warn("请先选择仓库！")
      return;
    };

    this.modalHelper.createStatic(SelectMasterStockComponent, {
      warehouseId: v.warehouseId
    })
      .subscribe((x: ReagentStockListDto[]) => {
        if (x && x.length > 0) {
          x.forEach(ele => {
            if (this.masterItems.findIndex(w => w.reagentStockId == ele.id) > -1) {
              //说明已经存在
            } else {
              //说明不存在  //reagentStockId
              ele['reagentStockId'] = ele.id;
              this.masterItems.push(ele);
            }
          });
          this.masterItems = [...this.masterItems];
        }
      });
  }

  /**
   * 选择普通试剂
   * @param v 
   */
  selectCommon(v: CreateOutOrderInputDto) {
    if (v.outOrderType != OutOrderTypeEnum.普通试剂) {
      this.notify.warn("请先选择 普通试剂")
      return;
    }
    if (!v.warehouseId) {
      this.notify.warn("请先选择仓库！")
      return;
    }
    this.modalHelper.createStatic(SelectCommonStockComponent, { warehouseId: v.warehouseId })
      .subscribe((x: ClientStockDto[]) => {
        if (x) {
          console.log(x);
          x.forEach(ele => {
            if (this.commonItems.findIndex(w => w.reagentId == ele.id && w.locationId == ele.locationId) > -1) {
              //说明已经存在
            } else {
              //说明不存在  //reagentStockId
              ele['reagentId'] = ele.id;
              this.commonItems.push(ele);
            }
          });
          console.log(this.commonItems);
          this.commonItems = [...this.commonItems];
        }
      });
  }

  removeMaster(index: number) {
    this.masterItems.splice(index, 1);
    this.masterItems = [...this.masterItems];
  }

  removeCommon(index: number) {
    this.commonItems.splice(index, 1);
    this.commonItems = [...this.commonItems];
  }


  masterItems: any[] = [];
  commonItems: any[] = [];

  selecteOutOrderType: OutOrderTypeEnum = OutOrderTypeEnum.专管试剂;

  @ViewChild("sf") sf: SFComponent;
  schema: SFSchema = {
    type: "object",
    properties: {
      outOrderType: {
        type: 'number',
        title: '类型',
        enum: [
          { label: '专管试剂', value: 1 },
          { label: '普通试剂', value: 2 }
        ],
        ui: {
          widget: 'radio',
          styleType: 'button',
          buttonStyle: 'solid',
          change: (v1: OutOrderTypeEnum) => {
            //清空仓库 
            this.sf.setValue('/warehouseId', null);
            this.masterItems = [];
            this.commonItems = [];
            this.selecteOutOrderType = v1;
          }
        },
        default: 1,
      },
      warehouseId: {
        title: '所属仓库',
        type: "number",
        ui: {
          widget: 'select',
          asyncData: () => this.commonServiceProxy.getAllActiveWareHouse(true)
            .pipe(map(m => m.map(res => {
              return {
                label: res.name, value: res.id
              };
            })))
        }
      },
      applyUserName: {
        title: '申请人账号',
        type: "string",

        description: '置空则默认申请人为当登录者'
      }
    },
    required: ['outOrderType', 'warehouseId']
  };

}
