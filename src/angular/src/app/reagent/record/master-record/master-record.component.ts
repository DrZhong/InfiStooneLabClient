import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { MasterStockInComponent } from '@app/reagent/stockin/master/master-stock-in/master-stock-in.component';
import { STComponent, STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentStockListDto, ReagentStockStatusEnum, ReagentStockServiceProxy, SafeAttributes, OperateTypeEnum, ReagentOperateRecordDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import dayjs from 'dayjs';


@Component({
  selector: 'app-master-record',
  templateUrl: './master-record.component.html',
  styles: [
  ]
})
export class MasterRecordComponent extends PagedListingComponentBase<ReagentOperateRecordDto> implements OnInit {
  showMore = false;
  reagentNo: string | undefined;
  barCode: string | undefined;
  createUserName: string | undefined;
  startDate: Date | undefined;
  endDate: Date | undefined;
  operateType: OperateTypeEnum | undefined;
  warehouseId: number | undefined;
  filter: string | undefined;
  date: any[] = null;
  casNo: string | undefined;

  disabledDate = (current: Date): boolean =>
    dayjs(current).diff(dayjs(), 'd') > 0;


  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    console.log(this.date);
    if (this.date) {
      if (this.date.length > 0) {
        this.startDate = this.date[0];
        this.endDate = this.date[1];
      } else {
        this.startDate = undefined;
        this.endDate = undefined;
      }
    } else {
      this.startDate = undefined;
      this.endDate = undefined;
    }
    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    if (this.operateType == null) {
      this.operateType = undefined;
    }

    this.reagentStockServiceProxy
      .getMasterReagentOperateRecord(
        this.reagentNo,
        this.barCode,
        this.createUserName,
        this.startDate,
        this.endDate,
        this.operateType,
        this.warehouseId,
        this.casNo,
        this.filter, this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: ReagentOperateRecordDto): void {
    // this.reagentStockServiceProxy.deleteMasterStock(entity.id)
    //   .subscribe(res => {
    //     this.refresh();
    //   });
  }

  wareHouses: any[] = [];
  enumberEntityDto: any[] = [];
  ngOnInit(): void {
    this.sorting = 'id desc';
    this.refresh();
  }
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
        // if (x)
        //   this.refresh();
      });
  }
  search() {
    this.pageNumber = 1;
    this.refresh();
  }

  scroll = {
    x: '1000px'
  }

  constructor(
    private reagentStockServiceProxy: ReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }





  columns: STColumn[] = [
    { title: 'ID', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '试剂编号', width: 100, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },

    {
      title: '存放位置', width: 140, index: 'warehouseName',
      format: (item: ReagentOperateRecordDto) => {
        return `${item.warehouseName} ${item.safeAttribute == SafeAttributes.易制毒 ? '易制毒' : '易制爆'}`;
      },
      className: 'text-truncate'
    },
    {
      title: '试剂名称', width: 120, index: 'reagentCnName',
      // format: (item: ReagentOperateRecordDto) => {
      //   return item.reagentCnName
      // },
      className: 'text-truncate'
    },
    {
      title: '条形码', width: 100, index: 'barCode', className: 'text-truncate'
    },


    //{ title: '纯度', width: 100, index: 'reagentPurity', className: 'text-truncate' },

    { title: '批次号', width: 120, index: 'batchNo', className: 'text-truncate' },
    {
      title: '规格', width: 100, index: 'capacity', format: (item: ReagentOperateRecordDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },

    {
      title: '操作类型', width: 90, index: 'operateType',
      type: 'tag',
      tag: {
        1: { text: '入库', color: 'success' },
        2: { text: '领用', color: 'processing' },
        3: { text: '归还', color: 'warning' },
        4: { text: '回收', color: 'error' },
      },
      className: 'text-truncate'
    },
    { title: '操作时重量/g', width: 110, type: 'number', index: 'weight' },
    { title: '操作人', width: 100, index: 'createUserName', className: 'text-truncate' },
    { title: '操作时间', index: 'creationTime', type: 'date', width: 150 }
  ];

  /**
   * 试剂回收
   */
  back(entity: ReagentStockListDto) {
    this.isTableLoading = true;
    this.reagentStockServiceProxy.backMasterStock(entity)
      .pipe(finalize(() => {
        this.isTableLoading = false;
      }))
      .subscribe(res => {
        this.notify.success('回收成功！');
        this.refresh();
      });
  }

  edit(item: ReagentStockListDto): void {
    // this.modalHelper.createStatic(EditReagentComponent, { id: item.id })
    //   .subscribe(x => {
    //     if (x)
    //       this.refresh();
    //   });
  }
  create() {
    this.modalHelper.createStatic(MasterStockInComponent, {}, {
      size: 1000,
      modalOptions: {
        nzClosable: false,
        nzKeyboard: false,
        nzStyle: {
          top: '20px',
        }
      }
    })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  };

}
