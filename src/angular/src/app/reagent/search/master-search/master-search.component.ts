import { Component, Injector, OnInit } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { MasterStockInComponent } from '@app/reagent/stockin/master/master-stock-in/master-stock-in.component';
import { STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentOperateRecordDto, OperateTypeEnum, ReagentStockServiceProxy, SafeAttributes, ReagentStockListDto, ClientStockDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { MasterSearchDetailComponent } from './master-search-detail/master-search-detail.component';

@Component({
  selector: 'app-master-search',
  templateUrl: './master-search.component.html',
  styles: [
  ]
})
export class MasterSearchComponent extends PagedListingComponentBase<ClientStockDto> implements OnInit {

  no: string | undefined;
  casNo: string | undefined;
  purity: string | undefined;
  storageCondition: string | undefined;
  warehouseId: number | undefined;
  stockStatus: number | undefined;
  filter: string | undefined;
  stockShouldMoreZero: boolean | undefined = false;

  showMore = false;

  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    if (this.stockStatus == null) {
      this.stockStatus = undefined;
    }

    this.reagentStockServiceProxy
      .getMasterReagentStock(
        this.no,
        this.casNo,
        this.purity,
        this.storageCondition,
        this.warehouseId,
        this.stockStatus,
        false,
        this.stockShouldMoreZero,
        this.filter,
        this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: ClientStockDto): void {
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
    // { title: 'ID', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '试剂编号', width: 100, index: 'no',
      render: 'no',
      className: 'text-truncate'
    },

    {
      title: '存放仓库', width: 140, index: 'warehouseName',
      className: 'text-truncate'
    },
    {
      title: '试剂信息', width: 200, index: 'reagentCnName',
      format: (item: ClientStockDto) => {
        return `[${item.casNo}]${item.cnName}`;
      },
      className: 'text-truncate'
    },
    { title: '库存', width: 120, index: 'num', type: 'number' },
    { title: '库存总重量/g', width: 120, type: 'number', index: 'totalWeight' },
    { title: '最小价格', width: 110, type: 'currency', index: 'minPrice' },
    { title: '最大价格', width: 110, type: 'currency', index: 'maxPrice' },
    {
      title: '状态', width: 100, index: 'reagentStatus', className: 'text-truncate',
      type: 'tag',
      tag: {
        0: { text: '液体', color: 'success' },
        1: { text: '固体', color: 'blue' },
        2: { text: '气体', color: 'red' }
      }
    },


    { title: '纯度', width: 100, index: 'purity', className: 'text-truncate' },
    {
      title: '规格', width: 120, index: 'capacity', format: (item: ClientStockDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },
    { title: '存储条件', width: 180, index: 'storageCondition', className: 'text-truncate' },
    {
      title: '操作',
      width: 80,

      buttons: [
        {
          text: '详情',
          type: 'modal',
          modal: {
            component: MasterSearchDetailComponent,
            size: 1200
          }
        }
      ],
    },

  ];


}
