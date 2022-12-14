import { NormalReagentStockServiceProxy } from './../../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STComponent, STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentStockStatusEnum, NormalReagentStockListDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { CommonStockInComponent } from './common-stock-in/common-stock-in.component';
import { RecordHistoryComponent } from './record-history/record-history.component';
import { QuicklyBindComponent } from './quickly-bind/quickly-bind.component';

@Component({
  selector: 'app-common',
  templateUrl: './common.component.html',
  styles: [
  ]
})
export class CommonComponent extends PagedListingComponentBase<NormalReagentStockListDto> implements OnInit {

  barCode: string | undefined;
  reagentCasNo: string | undefined;
  reagentNo: string | undefined;
  batchNo: string | undefined;
  supplierCompanyName: string | undefined;
  warehouseId: number | undefined;
  stockStatus: ReagentStockStatusEnum | undefined;
  filter: string | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    this.normalReagentStockServiceProxy
      .getAllNormal(this.barCode,
        this.reagentCasNo,
        this.reagentNo,
        this.batchNo,
        this.supplierCompanyName,
        this.warehouseId,
        this.stockStatus,
        this.filter, this.sorting, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: NormalReagentStockListDto): void {
    this.normalReagentStockServiceProxy.delete(entity.id)
      .subscribe(res => {
        this.refresh();
      });
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
        if (x)
          this.refresh();
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


  showProductionCompany = false;
  showSupplierCompany = false;
  showCnAliasName = false;
  showEnName = false;
  showSafeAttribute = false;
  showStorageCondition = false;
  @ViewChild("st") st: STComponent;
  checkChange(e) {
    this.st.resetColumns();
  }


  showMore = false;

  columns: STColumn[] = [
    { title: 'ID', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '?????????', width: 120, index: 'barCode', className: 'text-truncate'
    },

    {
      title: '????????????', width: 220, index: 'warehouseName',
      format: (item: NormalReagentStockListDto) => {
        return `${item.warehouseName} [${item.locationName}]`;
      },
      className: 'text-truncate'
    },
    {
      title: '????????????', width: 220, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },
    { title: '????????????', width: 120, index: 'amount', className: 'text-truncate' },
    { title: '??????????????????', width: 120, index: 'realAmount', className: 'text-truncate' },
    {
      title: '??????',
      format: (item: NormalReagentStockListDto) => {
        return `${item.capacity} ${item.capacityUnit}`;
      }
      , width: 100, index: 'capacity', className: 'text-truncate'
    },
    { title: '??????', width: 100, index: 'reagentPurity', className: 'text-truncate' },
    { title: '??????', type: 'currency', width: 120, index: 'price' },
    { title: '?????????', width: 120, index: 'batchNo', className: 'text-truncate' },
    // {
    //   title: '??????', width: 100, index: 'capacity', format: (item: ReagentStockListDto) => {
    //     return `${item.capacity}${item.capacityUnit}`;
    //   }, className: 'text-truncate'
    // },
    { title: '????????????', index: 'productionDate', type: 'date', dateFormat: 'yyyy-MM-dd', width: 120 },
    { title: '?????????', index: 'expirationDate', type: 'date', dateFormat: 'yyyy-MM-dd', width: 120 },
    { title: '?????????', width: 120, index: 'supplierCompanyName', className: 'text-truncate' },
    { title: '??????????????????', width: 140, type: 'date', index: 'stockInTime', className: 'text-truncate' },
    { title: '?????????????????????', width: 120, index: 'latestStockInUserName', className: 'text-truncate' },
    { title: '??????????????????', width: 140, type: 'date', index: 'latestStockOutTime', className: 'text-truncate' },
    { title: '???????????????', width: 120, index: 'latestStockOutUserName', className: 'text-truncate' },
    {
      title: '?????????',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          tooltip: '??????',
          icon: 'delete',
          type: 'del',
          iif: (item: NormalReagentStockListDto) => item.realAmount == 0,
          click: (record: any) => {
            this.delete(record)
          }
        },
        {
          tooltip: '????????????',
          icon: 'history',
          click: (record: any) => {
            this.history(record)
          }
        }
        ,
        {
          icon: 'printer',
          tooltip: '????????????',
          click: (record: any) => {
            this.edit(record)
          }
        }
      ],
    },
  ];



  history(item: NormalReagentStockListDto) {
    this.modalHelper.createStatic(RecordHistoryComponent, { i: item })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }


  edit(item: NormalReagentStockListDto): void {
    this.modalHelper.createStatic(QuicklyBindComponent, { i: item }, {
      size: 'md',
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
  }
  create() {
    this.modalHelper.createStatic(CommonStockInComponent, {}, {
      size: 'lg',
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
