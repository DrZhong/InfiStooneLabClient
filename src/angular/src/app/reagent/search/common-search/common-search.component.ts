import { NormalReagentStockServiceProxy } from './../../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ClientStockDto, ReagentStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { MasterSearchDetailComponent } from '../master-search/master-search-detail/master-search-detail.component';
import { CommonSearchDetailComponent } from './common-search-detail/common-search-detail.component';

@Component({
  selector: 'app-common-search',
  templateUrl: './common-search.component.html',
  styles: [
  ]
})
export class CommonSearchComponent extends PagedListingComponentBase<ClientStockDto> implements OnInit {

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

    this.normalReagentStockServiceProxy
      .getNormalReagentStock(
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
    private normalReagentStockServiceProxy: NormalReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }





  columns: STColumn[] = [
    // { title: 'ID', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '????????????', width: 100, index: 'no',
      render: 'no',
      className: 'text-truncate'
    },

    {
      title: '????????????', width: 140, index: 'warehouseName',
      className: 'text-truncate'
    },
    {
      title: '????????????', width: 200, index: 'reagentCnName',
      format: (item: ClientStockDto) => {
        return `[${item.casNo}]${item.cnName}`;
      },
      className: 'text-truncate'
    },
    { title: '??????', width: 120, index: 'num', type: 'number' },
    { title: '????????????', width: 110, type: 'currency', index: 'minPrice' },
    { title: '????????????', width: 110, type: 'currency', index: 'manPrice' },
    {
      title: '??????', width: 100, index: 'reagentStatus', className: 'text-truncate',
      type: 'tag',
      tag: {
        0: { text: '??????', color: 'success' },
        1: { text: '??????', color: 'blue' },
        2: { text: '??????', color: 'red' }
      }
    },


    { title: '??????', width: 100, index: 'purity', className: 'text-truncate' },
    {
      title: '??????', width: 120, index: 'capacity', format: (item: ClientStockDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },
    { title: '????????????', width: 180, index: 'storageCondition', className: 'text-truncate' },
    {
      title: '??????',
      width: 80,

      buttons: [
        {
          text: '??????',
          type: 'modal',
          modal: {
            component: CommonSearchDetailComponent,
            size: 1200
          }
        }
      ],
    },

  ];


}
