import { finalize } from 'rxjs/operators';
import { GerChartDataIntputDto, ReagentStockServiceProxy, NormalReagentStockServiceProxy, GerChartDataOutDto, ReagentDto } from './../../../shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/app-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { WarehouseType } from '@shared/service-proxies/service-proxies';
import { EChartsOption } from 'echarts';
import { NgxEchartsDirective } from 'ngx-echarts';
import dayjs from 'dayjs';
import { XlsxService } from '@delon/abc/xlsx';
import { SelectReagentComponent } from '@app/reagent/shared/select-reagent/select-reagent.component';
@Component({
  selector: 'app-statistical',
  templateUrl: './statistical.component.html',
  styles: [
  ]
})
export class StatisticalComponent extends AppComponentBase implements OnInit {

  constructor(
    private xlsx: XlsxService,
    private normalReagentStockServiceProxy: NormalReagentStockServiceProxy,
    private reagentStockServiceProxy: ReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }

  currentSelectedWarehouseType: WarehouseType = WarehouseType.试剂;
  ngOnInit(): void {
    this.currentSelectedWarehouseType = this.appSession.user.currentSelectedWarehouseType;
    this.that = this;
  }

  that: StatisticalComponent
  disabledDate = (current: Date): boolean =>
    dayjs(current).diff(dayjs(), 'd') > 0;

  chartOption: EChartsOption = {
    title: {
      text: '图表分析'
    },
    tooltip: {
      trigger: 'axis'
    },
    legend: {
      data: ['试剂入库', '试剂领用', '试剂归还', '试剂回收']
    },
    toolbox: {
      feature: {
        dataZoom: {
          yAxisIndex: 'none'
        },
        saveAsImage: {},
        restore: {},
        magicType: { type: ['line', 'bar'] },
        dataView: {
          show: true,
          title: '数据视图',
          readOnly: true
        },
        myToolsNum: {
          show: true,
          textAlign: 'right',
          title: '保存为excel',
          icon: 'path://M531.2 0h57.6v96h288c16 0 35.2 0 48 12.8 9.6 19.2 9.6 38.4 9.6 60.8v620.8c0 35.2 3.2 70.4-3.2 105.6-3.2 25.6-28.8 25.6-44.8 25.6H588.8V1024H528c-147.2-32-291.2-64-438.4-96V96l441.6-96zM361.6 313.6c-22.4 51.2-48 99.2-64 153.6-16-48-35.2-96-54.4-144-28.8 0-57.6 3.2-83.2 3.2 28.8 64 60.8 124.8 89.6 188.8-35.2 60.8-64 124.8-99.2 185.6 28.8 0 54.4 3.2 83.2 3.2 19.2-48 44.8-96 60.8-147.2 16 54.4 41.6 102.4 64 153.6 32 3.2 60.8 3.2 92.8 6.4-35.2-70.4-70.4-137.6-105.6-208 35.2-67.2 70.4-134.4 102.4-201.6-28.8 3.2-57.6 6.4-86.4 6.4z m371.2 32v89.6h134.4v-89.6h-134.4z m0-147.2V288h134.4v-89.6h-134.4z m0 284.8v89.6h134.4v-89.6h-134.4z m0 140.8v89.6h134.4V624h-134.4z m0 137.6v89.6h134.4v-89.6h-134.4z m-166.4-416v89.6h112v-89.6h-112z m0-147.2V288h112v-89.6h-112z m0 284.8v89.6h112v-89.6h-112z m0 140.8v89.6h112V624h-112z m0 137.6v89.6h112v-89.6h-112z',
          onclick: () => {
            this.excel();
          }
        }
      }
    },
    xAxis: {
      type: 'category',
      data: [],
    },
    yAxis: {
      type: 'value',
    },
    series: [
      {
        name: '试剂入库',
        data: [],
        type: 'line',
        markPoint: {
          data: [
            { type: 'max', name: 'Max' },
            { type: 'min', name: 'Min' }
          ]
        }
      },
      {
        name: '试剂领用',
        data: [],
        type: 'line',
        markPoint: {
          data: [
            { type: 'max', name: 'Max' },
            { type: 'min', name: 'Min' }
          ]
        }
      },
      {
        name: '试剂归还',
        data: [],
        type: 'line',
        markPoint: {
          data: [
            { type: 'max', name: 'Max' },
            { type: 'min', name: 'Min' }
          ]
        }
      },
      {
        name: '试剂回收',
        data: [],
        type: 'line',
        markPoint: {
          data: [
            { type: 'max', name: 'Max' },
            { type: 'min', name: 'Min' }
          ]
        }
      }
    ],
  };

  echartsInstance: any;
  chartDataLoading = false;
  chartLoadSuccess = false;
  onChartInit(ec: any) {
    this.echartsInstance = ec;
    this.chartLoadSuccess = true;
    this.refresh();
    // for (let index = 0; index < 24; index++) {
    //   this.chartOption.xAxis['data'].push(index);
    //   this.chartOption.series[0].data.push(Math.floor(Math.random() * 100));
    //   this.chartOption.series[1].data.push(Math.floor(Math.random() * 100));
    //   this.chartOption.series[2].data.push(Math.floor(Math.random() * 100));
    //   this.setDate();
    // }
  }

  excel() {
    if (this.dataList && this.dataList.time.length > 0) {
      const data = [['时间', '试剂入库', '试剂领用', '试剂归还', '试剂回收']];
      this.dataList.time.forEach((value, index) => {
        data.push([
          value,
          this.dataList.stockIn[index].toString(),
          this.dataList.stockOut[index].toString(),
          this.dataList.stockBack[index].toString(),
          this.dataList.stockRetrieve[index].toString(),
        ]);
      });

      //console.log(data);
      //return;
      this.xlsx.export({
        sheets: [
          {
            data,
            name: '统计数据'
          }
        ],
        filename: dayjs().format('YYYYMMDD_HHmmss') + '_' + this.appSession.user.name + '.xlsx'
      });
    }
  }

  refresh() {
    let body: GerChartDataIntputDto = new GerChartDataIntputDto();
    //body.startDate = this.sta
    body.dateCurrent = this.dateCurrent;
    body.groupBy = this.statisBy;
    if (this.customDate.length > 0) {
      body.startDate = this.customDate[0];
      body.endDate = this.customDate[1];
    } else {
      body.startDate = undefined;
      body.endDate = undefined;
    }
    //this.reagentType
    body.masterType
    body.safeAttributes = this.safeType;
    this.chartDataLoading = true;
    body.reagentId = this.selectedRegent.id;
    if (this.reagentType == 0) {
      this.masterSearch(body);
    } else {
      this.normalSearch(body);
    }

  }

  dataList: GerChartDataOutDto;

  masterSearch(body: GerChartDataIntputDto) {
    this.reagentStockServiceProxy.gerChartData(body)
      .pipe(finalize(() => {
        this.chartDataLoading = false;
      }))
      .subscribe(res => {
        this.dataList = res;
        // data: [ '试剂入库',  '试剂领用', '试剂归还', '试剂回收']
        this.chartOption.xAxis['data'] = res.time;
        this.chartOption.series[0].data = res.stockIn;
        this.chartOption.series[1].data = res.stockOut;
        this.chartOption.series[2].data = res.stockBack;
        this.chartOption.series[3].data = res.stockRetrieve;

        this.totalStockout = 0;
        this.totalStockin = 0;
        this.totalBack = 0;
        this.totalRetrieve = 0;
        res.stockIn.forEach(ele => {
          this.totalStockin += ele;
        });
        res.stockOut.forEach(ele => {
          this.totalStockout += ele;
        });
        res.stockBack.forEach(ele => {
          this.totalBack += ele;
        });
        res.stockRetrieve.forEach(ele => {
          this.totalRetrieve += ele;
        });

        this.setDate();
      });
  }


  selectedRegent: ReagentDto = new ReagentDto();
  clearReg() {
    this.selectedRegent = new ReagentDto();
    this.refresh();
  }
  selectReg() {
    let regentCatalog = [];
    if (this.reagentType == 0) {
      regentCatalog = [2];
    } else {
      regentCatalog = [0, 1];
    }
    this.modalHelper.createStatic(SelectReagentComponent, { reagentCatalog: regentCatalog })
      .subscribe((res: ReagentDto) => {
        //console.log(res);
        this.selectedRegent = res;
        this.refresh();
      });
  }

  normalSearch(body: GerChartDataIntputDto) {
    this.normalReagentStockServiceProxy.gerChartData(body)
      .pipe(finalize(() => {
        this.chartDataLoading = false;
      }))
      .subscribe(res => {
        this.dataList = res;
        // data: [ '试剂入库',  '试剂领用', '试剂归还', '试剂回收']
        this.chartOption.xAxis['data'] = res.time;
        this.chartOption.series[0].data = res.stockIn;
        this.chartOption.series[1].data = res.stockOut;
        this.chartOption.series[2].data = res.stockBack;
        this.chartOption.series[3].data = res.stockRetrieve;

        this.totalStockout = 0;
        this.totalStockin = 0;
        this.totalBack = 0;
        this.totalRetrieve = 0;
        res.stockIn.forEach(ele => {
          this.totalStockin += ele;
        });
        res.stockOut.forEach(ele => {
          this.totalStockout += ele;
        });
        res.stockBack.forEach(ele => {
          this.totalBack += ele;
        });
        res.stockRetrieve.forEach(ele => {
          this.totalRetrieve += ele;
        });

        this.setDate();
      });
  }

  setDate() {
    this.echartsInstance.setOption(this.chartOption)
  }

  /**
   * 快捷统计
   * A-今天，B-昨天，C-最近7天，D-最近30天
   */
  dateCurrent = 'A';
  customDate = [];
  /**
   * 统计分组
   * 0 -按时
   * 1-按日
   * 2-按周
   * 3-按月
   */
  statisBy = 0;
  /**
   * 试剂类型
   * 0-专管
   * 1-普通
   */
  reagentType = 0;

  /**
   * 安全类型
   * 0-全部，1-易制毒，2-易制爆，3-剧毒品，4-其它
   */
  safeType: number | undefined = undefined;


  totalStockin: number = 0;
  totalStockout = 0;
  totalBack = 0;
  totalRetrieve = 0;
  dateCurrentChange() {
    //console.log(this.dateCurrent);
    this.customDate = [];
    if (this.dateCurrent == 'A' || this.dateCurrent == 'B') {
      this.statisBy = 0;
    } else {
      this.statisBy = 1;
    }
    this.refresh();
  }
  statisByChange() {
    if (this.statisBy == 0 && this.dateCurrent > 'B') {
      this.dateCurrent = 'A';
    }
    this.refresh();

  }
  customDateChange(result: Date[]): void {
    this.dateCurrent = '';
    this.statisBy = 1;
    this.refresh();
  }


  safeTypeDisabled = false;
  /**
   * 统计类型
   */
  reagentTypeChange() {
    if (this.reagentType == 1) {
      //安全类型不可用
      this.safeType = undefined;
      this.safeTypeDisabled = true;
    } else {
      this.safeTypeDisabled = false;
    }
    this.refresh();
  }
  safeTypeChange() {
    this.refresh();
  }
}
