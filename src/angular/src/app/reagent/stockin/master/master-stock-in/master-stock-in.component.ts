import { SelectReagentComponent } from './../../../shared/select-reagent/select-reagent.component';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { CommonServiceProxy, CompanyType, CreateReagentStockDto, LocationStockDto, ReagentDto, ReagentLocationDto, ReagentStockListDto, ReagentStockServiceProxy, SafeAttributes } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { STColumn } from '@delon/abc/st';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import dayjs from 'dayjs';
import { differenceInCalendarDays, setHours } from 'date-fns';

@Component({
  selector: 'app-master-stock-in',
  templateUrl: './master-stock-in.component.html',
  styles: [
  ]
})
export class MasterStockInComponent extends ModalComponentBase implements OnInit {

  constructor(
    private commonServiceProxy: CommonServiceProxy,
    private reagentStockServiceProxy: ReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }


  locationStock: LocationStockDto;
  locationChange(e: any) {
    console.log(this.i.locationId);
    this.reagentStockServiceProxy.getLocationStock(this.i.locationId)
      .subscribe(q => {
        this.locationStock = q;
      });
  }

  reagentName = '';
  locations: ReagentLocationDto[] = [];
  locationMsg = "111";
  i: CreateReagentStockDto = new CreateReagentStockDto();
  saving = false;
  sCompanys: any[] = [];
  pCompanys: any[] = [];
  wsIinterval: any = undefined;
  ngOnInit(): void {
    this.i.expirationMonth = 36;
    this.commonServiceProxy
      .getAllCompany(true)
      .subscribe(res => {
        this.sCompanys = res.filter(w => w.companyType == CompanyType.供应商);
        this.pCompanys = res.filter(w => w.companyType == CompanyType.生产商);
      });

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
      //socket 获取后端传递到前端的信息
      //that.ws.send('sonmething');
      //console.log(event.data);
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
      that.notify.error('请检测打印软件是否启动！');

    }
    this.ws.onclose = function (event) {
      //socket 关闭后执行
      that.notify.info('打印软件已关闭！');
      that.wsReady = false;
    }
  }

  today = new Date();
  disabledDate = (current: Date): boolean =>
    // Can not select days before today and today
    differenceInCalendarDays(current, this.today) > 0;

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
    if (this.dataList.length == 0) {
      this.notify.error('请先 绑定标签 后在点击 打印标签');
      return;
    }
    //可以打印了
    this.dataList.forEach(ele => {
      this.print(ele.barCode);
    });
  }


  close2() {
    if (this.dataList.length > 0) {
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

  codes: string[] = [];
  getCode() {
    this.saving = true;
    this.reagentStockServiceProxy.getCodes(this.num)
      .pipe(finalize(() => {
        this.saving = false;
      }))
      .subscribe(res => {
        this.codes = res;
        //this.notify.success('绑定成功')
        //this.success(1);
      });
  }
  clearCode() {
    this.codes = [];
  }

  save() {
    this.i.codes = this.codes;
    this.saving = true;

    this.reagentStockServiceProxy.createMasterStock(this.i)
      .pipe(finalize(() => {
        this.saving = false;
      }))
      .subscribe(res => {
        this.dataList = res;
        this.notify.success('绑定成功')
        //this.success(1);
      })
  }

  productionCompanyId: number;
  supplierCompanyId: number;
  selectReagent() {
    this.modalHelper.createStatic(SelectReagentComponent, { reagentCatalog: [2] })
      .subscribe((res: ReagentDto) => {
        //console.log(res);
        this.i.reagentId = res.id;
        this.reagentName = `[${res.no},${res.casNo}]${res.cnName}`;
        this.i.productionCompanyId = res.productionCompanyId;
        this.i.supplierCompanyId = res.supplierCompanyId;
        this.i.price = res.price;
        this.i.locationId = null;
        this.locations = res.reagentLocations;
        this.locationStock = undefined;
      });
  }
  num = 0;

  dataList: ReagentStockListDto[] = [];
  columns: STColumn[] = [
    {
      title: '删除', index: 'id', width: 60, buttons: [
        {
          type: 'del',
          text: '删除',
          click: (item: any) => {
            this.reagentStockServiceProxy.deleteMasterStock(item.id)
              .subscribe(res => {
                let index = this.dataList.findIndex(q => q.id == item.id);
                if (index > -1) {
                  this.dataList.splice(index, 1);
                  this.dataList = [...this.dataList];
                }
              });
          }
        }
      ]
    },
    { title: 'ID', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '条形码', width: 120, index: 'barCode', className: 'text-truncate'
    },

    {
      title: '存放位置', width: 150, index: 'warehouseName',
      format: (item: ReagentStockListDto) => {
        return `${item.locationName} ${item.reagentSafeAttribute == SafeAttributes.易制毒 ? '易制毒' : '易制爆'}`;
      },
      className: 'text-truncate'
    },
    {
      title: '试剂信息', width: 220, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },
    {
      title: '规格',
      format: (item: ReagentStockListDto) => {
        return `${item.capacity} ${item.capacityUnit}`;
      }
      , width: 100, index: 'capacity', className: 'text-truncate'
    },
    { title: '纯度', width: 100, index: 'reagentPurity', className: 'text-truncate' },
    { title: '价格', width: 110, type: 'currency', index: 'price' },
    { title: '批次号', width: 120, index: 'batchNo', className: 'text-truncate' },
    {
      title: '容量', width: 100, index: 'capacity', format: (item: ReagentStockListDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },
    { title: '终端确认', width: 100, index: 'clientConfirm', type: 'yn', className: 'text-truncate' },
    { title: '双人双锁', width: 100, index: 'doubleConfirm', type: 'yn', className: 'text-truncate' },
    { title: '供应商', width: 120, index: 'supplierCompanyName', className: 'text-truncate' },


  ];
  show(reagentId: number) {
    this.modalHelper.createStatic(ShowReagentDetailComponent, {
      id: reagentId
    }, {
      size: 1000,
      modalOptions: {
        nzStyle: {
          top: '50px'
        }
      }
    })
      .subscribe(x => {

      });
  }

  scroll = {
    x: '1000px',
    y: '300px'
  }
}
