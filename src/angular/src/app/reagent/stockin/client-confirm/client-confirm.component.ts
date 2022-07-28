import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STComponent, STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentStockListDto, ReagentStockStatusEnum, ReagentStockServiceProxy, SafeAttributes } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { MasterStockInComponent } from '../master/master-stock-in/master-stock-in.component';
import { ReagentStockAuditDialogComponent } from '../reagent-stock-audit-dialog/reagent-stock-audit-dialog.component';

@Component({
  selector: 'app-client-confirm',
  templateUrl: './client-confirm.component.html',
  styles: [
  ]
})
export class ClientConfirmComponent extends PagedListingComponentBase<ReagentStockListDto> implements OnInit {

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
      .getAllMasterClientConfirm(this.barCode,
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
      title: '条形码', width: 120, index: 'barCode', className: 'text-truncate'
    },

    {
      title: '存放位置', width: 150, index: 'warehouseName',
      format: (item: ReagentStockListDto) => {
        return `${item.warehouseName} ${item.reagentSafeAttribute == SafeAttributes.易制毒 ? '易制毒' : '易制爆'}`;
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

    { title: '批次号', width: 120, index: 'batchNo', className: 'text-truncate' },
    {
      title: '容量', width: 100, index: 'capacity', format: (item: ReagentStockListDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },

    { title: '供应商', width: 120, index: 'supplierCompanyName', className: 'text-truncate' },
    {
      title: '状态', width: 100, index: 'stockStatus',
      type: 'tag',
      tag: {
        0: { text: '待入库', color: 'error' },
        1: { text: '在库', color: 'success' },
        2: { text: '离库', color: 'processing' },
        3: { text: '已用完', color: 'warning' },
      },
      className: 'text-truncate'
    },
    { title: '入库人', width: 100, index: 'latestStockInUserName', className: 'text-truncate' },
    { title: '创建人', width: 100, index: 'createUserName', className: 'text-truncate' },
    { title: '终端确认', width: 120, index: 'clientConfirm', render: 'clientConfirm', className: 'text-truncate' },
    { title: '双人双锁', width: 120, index: 'doubleConfirm', render: 'doubleConfirm', className: 'text-truncate' },
    { title: '首次入库时间', index: 'firstStockInTime', type: 'date', width: 150 },
    { title: '最新入库时间', index: 'latestStockInTime', type: 'date', width: 150 },
    {
      title: '操作区',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          text: '删除',
          type: 'del',
          iif: (item: ReagentStockListDto) => item.stockStatus == ReagentStockStatusEnum.待入库,
          click: (record: any) => {
            this.delete(record)
          }
        },
        {
          text: '审核通过',
          pop: '你确定要审核通过吗？',
          type: 'del',
          iif: (item: ReagentStockListDto) => item.stockStatus == ReagentStockStatusEnum.待入库,
          click: (record: any) => {
            this.audit(record)
          }
        },
      ],
    },
  ];


  protected audit(entity: ReagentStockListDto): void {
    this.reagentStockServiceProxy.masterClientConfirm(entity)
      .subscribe(res => {
        this.refresh();
      });
  }

  shwoAudit(item: ReagentStockListDto) {
    this.modalHelper.createStatic(ReagentStockAuditDialogComponent, { i: item })
      .subscribe(x => {

      });
  }


}
