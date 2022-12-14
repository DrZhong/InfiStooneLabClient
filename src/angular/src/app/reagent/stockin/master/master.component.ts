import { ShowReagentDetailComponent } from './../../shared/show-reagent-detail/show-reagent-detail.component';
import { MasterStockInComponent } from './master-stock-in/master-stock-in.component';
import { ReagentStockServiceProxy, ReagentStockStatusEnum } from './../../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentStockListDto, SafeAttributes } from '@shared/service-proxies/service-proxies';
import { STColumn, STComponent } from '@delon/abc/st';
import { finalize } from 'rxjs/operators';
import { FADE_CLASS_NAME_MAP } from 'ng-zorro-antd/modal';
import { DoubleConfirmComponent } from '../double-confirm/double-confirm.component';
import { ReagentStockAuditDialogComponent } from '../reagent-stock-audit-dialog/reagent-stock-audit-dialog.component';
import { QuicklyBindComponent } from '../common/quickly-bind/quickly-bind.component';
import { MasterQuicklyBindComponent } from './master-quickly-bind/master-quickly-bind.component';

@Component({
  selector: 'app-master',
  templateUrl: './master.component.html',
  styles: [
  ]
})
export class MasterComponent extends PagedListingComponentBase<ReagentStockListDto> implements OnInit {

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
    if (this.stockStatus == null) {
      this.stockStatus = undefined;
    }

    this.reagentStockServiceProxy
      .getAllMaster(this.barCode,
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
  protected delete(entity: ReagentStockListDto): void {
    this.reagentStockServiceProxy.deleteMasterStock(entity.id)
      .subscribe(res => {
        this.refresh();
      });
  }

  wareHouses: any[] = [];
  enumberEntityDto: any[] = [];
  canHuiShou = false;
  ngOnInit(): void {
    this.canHuiShou = this.isGranted('Pages.Reagent.Ruku.HuiShou');
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


  showMore = false;

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
    { title: '??????', type: 'currency', width: 120, index: 'price' },
    {
      title: '??????', width: 100, index: 'capacity', format: (item: ReagentStockListDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },
    { title: '????????????/g', width: 110, type: 'number', index: 'weight' },
    { title: '?????????', width: 120, index: 'supplierCompanyName', className: 'text-truncate' },
    {
      title: '??????', width: 90, index: 'stockStatus',
      type: 'tag',
      tag: {
        0: { text: '?????????', color: 'error' },
        1: { text: '??????', color: 'success' },
        2: { text: '??????', color: 'processing' },
        3: { text: '?????????', color: 'warning' },
      },
      className: 'text-truncate'
    },
    { title: '????????????', index: 'productionDate', type: 'date', dateFormat: 'yyyy-MM-dd', width: 120 },
    { title: '?????????', index: 'expirationDate', type: 'date', dateFormat: 'yyyy-MM-dd', width: 120 },
    { title: '????????????', width: 100, index: 'lockedOrderId', className: 'text-truncate' },
    { title: '?????????', width: 100, index: 'latestStockInUserName', className: 'text-truncate' },
    { title: '????????????', width: 120, index: 'clientConfirm', render: 'clientConfirm', className: 'text-truncate' },
    { title: '????????????', width: 120, index: 'doubleConfirm', render: 'doubleConfirm', className: 'text-truncate' },
    { title: '??????????????????', index: 'firstStockInTime', type: 'date', width: 150 },
    { title: '??????????????????', index: 'latestStockOutTime', type: 'date', width: 150 },
    { title: '???????????????', index: 'latestStockOutUserName', type: 'date', width: 110, className: 'text-truncate' },
    { title: '??????????????????', index: 'latestStockInTime', type: 'date', width: 150 },//latestStockOutTime
    {
      title: '?????????',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {

          icon: 'delete',
          tooltip: '??????',
          type: 'del',
          iif: (item: ReagentStockListDto) => item.stockStatus == ReagentStockStatusEnum.?????????,
          click: (record: any) => {
            this.delete(record)
          }
        },
        {
          text: '??????',
          type: 'del',
          pop: '???????????????????????????????????????????????????????????????????????????',
          iif: (item: ReagentStockListDto) => {
            if (!this.canHuiShou) return false;
            return item.stockStatus == ReagentStockStatusEnum.??????; //|| item.stockStatus == ReagentStockStatusEnum.??????
          },
          click: (record: any) => {
            this.back(record)
          }
        },
        {
          icon: 'printer',
          tooltip: '????????????',
          type: 'modal',
          modal: {
            component: MasterQuicklyBindComponent
          }
        }
      ],
    },
  ];

  /**
   * ????????????
   */
  back(entity: ReagentStockListDto) {
    this.isTableLoading = true;
    this.reagentStockServiceProxy.backMasterStock(entity)
      .pipe(finalize(() => {
        this.isTableLoading = false;
      }))
      .subscribe(res => {
        this.notify.success('???????????????');
        this.refresh();
      });
  }


  shwoAudit(item: ReagentStockListDto) {
    this.modalHelper.createStatic(ReagentStockAuditDialogComponent, { i: item })
      .subscribe(x => {

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
