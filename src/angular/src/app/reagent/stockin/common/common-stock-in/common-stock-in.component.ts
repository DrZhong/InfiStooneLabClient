import { CreateNormalReagentStockDto, CreateNormalReagentStockItemDto, NormalReagentStockServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { SelectReagentComponent } from '@app/reagent/shared/select-reagent/select-reagent.component';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STColumn } from '@delon/abc/st';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { CommonServiceProxy, ReagentStockServiceProxy, ReagentLocationDto, CreateReagentStockDto, CompanyType, ReagentDto, ReagentStockListDto, SafeAttributes } from '@shared/service-proxies/service-proxies';
import { finalize, map } from 'rxjs/operators';
import { NgForm } from '@angular/forms';
import { SFSchema, SFComponent } from '@delon/form';

@Component({
  selector: 'app-common-stock-in',
  templateUrl: './common-stock-in.component.html',
  styles: [
  ]
})
export class CommonStockInComponent extends ModalComponentBase implements OnInit {

  constructor(
    private commonServiceProxy: CommonServiceProxy,
    private normalReagentStockServiceProxy: NormalReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }

  reagentName = '';
  locations: ReagentLocationDto[] = [];

  i: CreateNormalReagentStockDto = new CreateNormalReagentStockDto();
  saving = false;
  sCompanys: any[] = [];
  pCompanys: any[] = [];
  wsIinterval: any = undefined;
  ngOnInit(): void {
    // this.i.productionCompanyId = 3;
    // this.i.supplierCompanyId = 5;
    // this.commonServiceProxy
    //   .getAllCompany(true)
    //   .subscribe(res => {
    //     this.sCompanys = res.filter(w => w.companyType == CompanyType.供应商);
    //     this.pCompanys = res.filter(w => w.companyType == CompanyType.生产商);
    //   });

    //尝试连接打印机
    this.connectWs();
    this.wsIinterval = setInterval(() => {
      if (this.wsReady) return;
      this.connectWs();
    }, 30000);
  }
  wsReady = false;
  ws: WebSocket;//定义websocket
  //socket连接
  connectWs() {
    if (this.ws != null) { this.ws.close() };
    this.ws = new WebSocket("ws://127.0.0.1:18181/");
    let that = this;
    this.ws.onopen = (event) => {
      that.notify.success('打印机连接成功！');
      that.wsReady = true;
    };
    this.ws.onmessage = function (event: MessageEvent) {
      if (event.type == 'message') {
        var json = JSON.parse(event.data);
        if (json.success) {
          that.notify.success('正在打印...');
        } else {
          that.message.error(json.error);
        }
      }
    }
    this.ws.onerror = function (event) {
      //socket error信息
      //that.wsReady = false;
      that.notify.error('请检测打印软件是否启动！');

    }
    this.ws.onclose = function (event) {
      //socket 关闭后执行
      that.notify.info('打印软件已关闭！');
      that.wsReady = false;
    }
  }

  print(item: string) {
    if (!this.wsReady) {
      this.notify.error('请先打开打印机程序');
      return;
    }
    let val = {
      data: item
    };
    this.ws.send(JSON.stringify(val));
  }

  printBach() {
    if (!this.wsReady) {
      this.notify.error('请先打开打印机程序');
      return;
    }
    let d = this.i.items.filter(w => w.barCode);
    if (d.length == 0) {
      this.notify.error('请先点击 确认入库 后再打印标签');
      return;
    }
    //可以打印了
    d.forEach(ele => {
      for (let index = 0; index < ele.amount; index++) {
        this.print(ele.barCode);
      }
    });
  }


  close2() {
    if (this.i.items && this.i.items.length > 0) {
      this.message.confirm('你确定已经打印标签了嘛？关闭后将会无法批量打印此批次的标签！', '警告', res => {
        if (res) {
          this.beforeClose();
          this.success(1);
        }
      });
    } else {
      this.beforeClose();
      this.close();
    }
  }

  beforeClose() {
    clearInterval(this.wsIinterval);
    if (this.ws != null) { this.ws.close() };
  }


  save(value: CreateNormalReagentStockDto) {

    if (!value.productionDate && !value.expirationDate) {
      this.notify.warn('生产日期和过期日期至少填写一个！');
      return;
    }
    //判断
    if (this.i.items.filter(w => w.amount > 0).length == 0) {
      this.notify.warn('请至少填写一个存储位置的数量大于0');
      return;
    }

    this.saving = true;

    this.i.batchNo = value.batchNo;
    this.i.supplierCompanyId = value.supplierCompanyId;
    this.i.supplierCompanyId = value.productionCompanyId;
    this.i.productionDate = new Date(value.productionDate);
    this.i.expirationMonth = value.expirationMonth;
    this.i.expirationDate = value.expirationDate;
    this.i.price = value.price;
    console.log(this.i);
    this.normalReagentStockServiceProxy.createNormalStock(this.i)
      .pipe(finalize(() => {
        this.saving = false;
      }))
      .subscribe(res => {
        //this.dataList = res;
        for (const iterator of this.i.items) {
          if (iterator.amount > 0) {
            iterator.barCode = res.items.find(w => w.locationId == iterator.locationId).barCode;
          }
        }
        this.i.items = [...this.i.items];
        this.notify.success('绑定成功');
        //this.success(1);
      })
  }

  showLocationStock(s: CreateNormalReagentStockItemDto) {
    this.normalReagentStockServiceProxy.getLocationStock(s.locationId)
      .subscribe(w => {
        s['locationStock'] = w;
      });
  }

  warehouseId: number;
  productionCompanyId: number;
  supplierCompanyId: number;
  @ViewChild("sf") sf: SFComponent;
  selectReagent() {
    this.modalHelper.createStatic(SelectReagentComponent, { reagentCatalog: [0, 1] })
      .subscribe((res: ReagentDto) => {
        this.i.reagentId = res.id;
        this.i.items = [];
        //this.sf.setValue("/batchNo", "qqqq");
        this.reagentName = `[${res.no},${res.casNo}]${res.cnName}`;
        this.i.productionCompanyId = res.productionCompanyId;
        //this.sf.setValue("/productionCompanyId", res.productionCompanyId);
        //this.sf.setValue("/supplierCompanyId", res.supplierCompanyId);

        this.i.supplierCompanyId = res.supplierCompanyId;//supplierCompanyId
        this.i.price = res.price;
        //this.sf.setValue("/warehouseId", 0);
        this.sf.refreshSchema();
        this.warehouseId = 0;
        //this.i.locationId = null;
        this.locations = res.reagentLocations;
      });
  }
  num = 0;
  schema: SFSchema = {
    type: "object",
    ui: { grid: { xs: 24, md: 12 }, spanLabelFixed: 100 },
    properties: {
      reagentId: {
        type: 'number',
        title: '选择试剂',
        ui: {
          widget: 'custom',
          grid: { span: 24 }
        }
      },
      batchNo: {
        type: 'string',
        title: '入库批号'
      },
      price: {
        title: '价格',
        type: 'number',
        ui: {
          widgetWidth: 160
        }
      },
      supplierCompanyId: {
        type: 'number',
        title: '供应商',
        ui: {
          widget: 'select',
          asyncData: () => this.commonServiceProxy
            .getAllCompany(true).pipe(map(res => {
              return res.filter(w => w.companyType == CompanyType.供应商)
                .map(src => {
                  return {
                    label: src.name,
                    value: src.id
                  };
                })
            }))
        }
      },
      productionCompanyId: {
        title: '生产商',
        type: 'number',
        ui: {
          widget: 'select',
          asyncData: () => this.commonServiceProxy
            .getAllCompany(true).pipe(map(res => {
              return res.filter(w => w.companyType == CompanyType.生产商)
                .map(src => {
                  return {
                    label: src.name,
                    value: src.id
                  };
                })
            }))
        }
      },
      productionDate: {
        title: '生产日期',
        type: 'string',
        format: 'date',
        ui: {
          widgetWidth: 160
        }
      },
      expirationMonth: {
        title: '保质期(月)',
        type: 'number',
        ui: {
          unit: '月',
          widgetWidth: 160
        },
        default: 36
      },
      expirationDate: {
        title: '过期日期',
        type: 'string',
        format: 'date',
        ui: {
          widgetWidth: 160
        }
      },
      tip: {
        type: 'string',
        title: '',
        ui: {
          widget: 'custom'
        },
      },
      warehouseId: {
        title: '选择仓库',
        type: 'number',
        ui: {
          grid: { span: 24 },
          widget: 'select',
          placeholder: '选择仓库后，下方表格会显示可以存储的位置',
          asyncData: () => this.commonServiceProxy.getAllActiveWareHouse(true)
            .pipe(map(w => w.map(src => {
              return {
                label: src.name,
                value: src.id
              };
            }))),
          change: (m) => {
            console.log(m);
            this.i.items = this.locations.filter(w => w.locationWarehouseId == m)
              .map(m => {
                let va = new CreateNormalReagentStockItemDto();
                va.locationId = m.locationId,
                  va.amount = 0;
                va['locationName'] = m.locationName;
                return va;
              });
          }
        }
      }
    },
    required: ['batchNo', 'supplierCompanyId', 'productionCompanyId', 'reagentName']
  };


  scroll = {
    x: '1000px',
    y: '300px'
  }
}
