import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STComponent, STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentStockListDto, ReagentStockStatusEnum, ReagentStockServiceProxy, SafeAttributes } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { ReagentStockAuditDialogComponent } from '../reagent-stock-audit-dialog/reagent-stock-audit-dialog.component';

@Component({
  selector: 'app-double-confirm',
  templateUrl: './double-confirm.component.html',
  styles: [
  ]
})
export class DoubleConfirmComponent extends PagedListingComponentBase<ReagentStockListDto> implements OnInit {

  barCode: string | undefined;
  reagentCasNo: string | undefined;
  reagentNo: string | undefined;
  batchNo: string | undefined;
  supplierCompanyName: string | undefined;
  warehouseId: number | undefined;
  stockStatus: ReagentStockStatusEnum | undefined;
  filter: string | undefined;

  clientConfirmed: boolean = false;;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    if (this.stockStatus == null) {
      this.stockStatus = undefined;
    }

    this.reagentStockServiceProxy
      .getAllMasterDoubleConfirm(this.barCode,
        this.reagentCasNo,
        this.reagentNo,
        this.batchNo,
        this.supplierCompanyName,
        this.warehouseId,
        this.stockStatus,
        this.filter, this.sorting, request.skipCount, request.maxResultCount, this.clientConfirmed)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: ReagentStockListDto): void {
    this.reagentStockServiceProxy.deleteMasterStock(entity.id)
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
    private reagentStockServiceProxy: ReagentStockServiceProxy,
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




  columns: STColumn[] = [
    { title: 'ID', index: 'id', width: 60, className: 'text-truncate' },
    {
      title: '?????????', width: 120, index: 'barCode', className: 'text-truncate'
    },

    {
      title: '????????????', width: 150, index: 'warehouseName',
      format: (item: ReagentStockListDto) => {
        return `${item.warehouseName} ${item.reagentSafeAttribute == SafeAttributes.????????? ? '?????????' : '?????????'}`;
      },
      className: 'text-truncate'
    },
    {
      title: '????????????', width: 220, index: 'cnName',
      render: 'reagentNo',
      className: 'text-truncate'
    },
    {
      title: '??????',
      format: (item: ReagentStockListDto) => {
        return `${item.capacity} ${item.capacityUnit}`;
      }
      , width: 100, index: 'capacity', className: 'text-truncate'
    },
    { title: '??????', width: 100, index: 'reagentPurity', className: 'text-truncate' },

    { title: '?????????', width: 120, index: 'batchNo', className: 'text-truncate' },
    {
      title: '??????', width: 100, index: 'capacity', format: (item: ReagentStockListDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },

    { title: '?????????', width: 120, index: 'supplierCompanyName', className: 'text-truncate' },
    {
      title: '??????', width: 100, index: 'stockStatus',
      type: 'tag',
      tag: {
        0: { text: '?????????', color: 'error' },
        1: { text: '??????', color: 'success' },
        2: { text: '??????', color: 'processing' },
        3: { text: '?????????', color: 'warning' },
      },
      className: 'text-truncate'
    },
    { title: '?????????', width: 100, index: 'latestStockInUserName', className: 'text-truncate' },
    { title: '?????????', width: 100, index: 'createUserName', className: 'text-truncate' },
    { title: '????????????', width: 120, index: 'clientConfirm', render: 'clientConfirm', className: 'text-truncate' },
    { title: '????????????', width: 120, index: 'doubleConfirm', render: 'doubleConfirm', className: 'text-truncate' },
    { title: '??????????????????', index: 'firstStockInTime', type: 'date', width: 150 },
    { title: '??????????????????', index: 'latestStockInTime', type: 'date', width: 150 },
    {
      title: '?????????',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          text: '??????',
          type: 'del',
          iif: (item: ReagentStockListDto) => item.stockStatus == ReagentStockStatusEnum.?????????,
          click: (record: any) => {
            this.delete(record)
          }
        },
        {
          text: '????????????',
          pop: '??????????????????????????????',
          type: 'del',
          iif: (item: ReagentStockListDto) => item.stockStatus == ReagentStockStatusEnum.?????????,
          click: (record: any) => {
            this.audit(record)
          }
        },
      ],
    },
  ];


  shwoAudit(item: ReagentStockListDto) {
    this.modalHelper.createStatic(ReagentStockAuditDialogComponent, { i: item })
      .subscribe(x => {

      });
  }


  protected audit(entity: ReagentStockListDto): void {
    this.reagentStockServiceProxy.masterDoubleConfirm(entity)
      .subscribe(res => {
        this.refresh();
      });
  }



}
