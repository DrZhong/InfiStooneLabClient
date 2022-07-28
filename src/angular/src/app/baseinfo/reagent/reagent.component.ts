import { EditReagentComponent } from './edit-reagent/edit-reagent.component';
import { GetReagentDto, ReagentCatalog, ReagentDto, ReagentServiceProxy, ReagentStatus } from './../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { finalize } from 'rxjs/operators';
import { STColumn, STComponent } from '@delon/abc/st';
import { DeletedReagentComponent } from './deleted-reagent/deleted-reagent.component';
import { ImportByExcelComponent } from './import-by-excel/import-by-excel.component';

@Component({
  selector: 'app-reagent',
  templateUrl: './reagent.component.html',
  styles: [
  ]
})
export class ReagentComponent extends PagedListingComponentBase<ReagentDto> implements OnInit {

  no: string | undefined;
  casNo: string | undefined;
  supplierCompanyName: string | undefined;
  productionCompanyName: string | undefined;
  reagentCatalog: ReagentCatalog[] | undefined;
  reagentStatus: ReagentStatus | undefined;
  filter: string | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.reagentCatalog == null) {
      this.reagentCatalog = undefined;
    }
    if (this.reagentStatus == null) {
      this.reagentStatus = undefined;
    }
    let body: GetReagentDto = new GetReagentDto();
    body.no = this.no;
    body.casNo = this.casNo;
    body.supplierCompanyName = this.supplierCompanyName;
    body.productionCompanyName = this.productionCompanyName;
    body.reagentCatalogs = this.reagentCatalog;
    body.reagentStatus = this.reagentStatus;
    body.filter = this.filter;
    body.sorting = this.sorting;
    body.skipCount = request.skipCount;
    body.maxResultCount = request.maxResultCount;
    this.reagentServiceProxy
      .getAll(body)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: ReagentDto): void {
    this.reagentServiceProxy.delete(entity.id)
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

  scroll = {
    x: '1000px'
  }

  constructor(
    private reagentServiceProxy: ReagentServiceProxy,
    injector: Injector) {
    super(injector);
  }

  /**
   * 已删除试剂查询
   */
  deleted() {
    this.modalHelper.createStatic(DeletedReagentComponent, {}, {
      size: 1000
    })
      .subscribe(x => {

      });
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
    { title: 'ID', index: 'id', width: 100, className: 'text-truncate' },
    {
      title: '试剂编号', width: 120, index: 'no', className: 'text-truncate'
    },
    {
      title: '类型', type: 'tag', tag: {
        0: { text: '常规试剂', color: 'green' },
        1: { text: '标品试剂', color: 'orange' },
        2: { text: '专管试剂', color: 'red' }
      }, width: 100, index: 'reagentCatalog', className: 'text-truncate'
    },
    { title: 'Cas号', width: 120, index: 'casNo', className: 'text-truncate' },
    { title: '中文名', width: 140, index: 'cnName', className: 'text-truncate' },
    { title: '中文别名', iif: () => this.showCnAliasName, width: 140, index: 'cnAliasName', className: 'text-truncate' },
    { title: '英文名', iif: () => this.showEnName, width: 140, index: 'enName', className: 'text-truncate' },
    {
      title: '安全属性', type: 'tag', tag: {
        0: { text: '易制毒', color: 'red' },
        1: { text: '易制爆', color: 'orange' },
        2: { text: '剧毒品', color: 'yellow' },
        3: { text: '其它', color: 'green' }
      }, iif: () => this.showSafeAttribute, width: 140, index: 'safeAttribute', className: 'text-truncate'
    },
    { title: '存储条件', iif: () => this.showStorageCondition, width: 140, index: 'storageCondition', className: 'text-truncate' },
    {
      title: '容量', width: 120, index: 'capacity', format: (item: ReagentDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },
    { title: '纯度', width: 120, index: 'purity', className: 'text-truncate' },//price
    { title: '参考价格', type: 'currency', width: 120, index: 'price' },
    { title: '生产商', width: 120, iif: () => this.showProductionCompany, index: 'productionCompanyName', className: 'text-truncate' },
    { title: '供应商', width: 120, iif: () => this.showSupplierCompany, index: 'supplierCompanyName', className: 'text-truncate' },
    { title: '终端确认', width: 100, index: 'clientConfirm', type: 'yn', className: 'text-truncate' },
    { title: '双人双锁', width: 100, index: 'doubleConfirm', type: 'yn', className: 'text-truncate' },
    { title: '创建者', width: 120, index: 'createUserName', className: 'text-truncate' },
    { title: '创建时间', index: 'creationTime', type: 'date', width: 150 },
    {
      title: '操作区',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          text: '删除',
          type: 'del',
          click: (record: any) => {
            this.delete(record)
          }
        },
        {
          text: '编辑',
          click: (record: any) => {
            this.edit(record)
          }
        }
      ],
    },
  ];

  edit(item: ReagentDto): void {
    this.modalHelper.createStatic(EditReagentComponent, { id: item.id })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }
  create() {
    this.modalHelper.createStatic(EditReagentComponent, {})
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  };

  excel() {
    this.modalHelper.createStatic(ImportByExcelComponent, {})
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }

}
