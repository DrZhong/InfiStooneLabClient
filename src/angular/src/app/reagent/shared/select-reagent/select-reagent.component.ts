import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { EditReagentComponent } from '@app/baseinfo/reagent/edit-reagent/edit-reagent.component';
import { STComponent, STColumn, STChange } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ReagentDto, ReagentCatalog, ReagentStatus, ReagentServiceProxy, GetReagentDto } from '@shared/service-proxies/service-proxies';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-select-reagent',
  templateUrl: './select-reagent.component.html',
  styles: [
  ]
})
export class SelectReagentComponent extends PagedListingComponentBase<ReagentDto> implements OnInit {

  no: string | undefined;
  casNo: string | undefined;
  supplierCompanyName: string | undefined;
  productionCompanyName: string | undefined;
  @Input()
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

  }

  wareHouses: any[] = [];
  enumberEntityDto: any[] = [];
  ngOnInit(): void {
    this.sorting = 'id desc';
    this.refresh();
  }

  scroll = {
    x: '800px'
  }
  change1($event: STChange) {

    if ($event.type == 'radio') {
      this.isTableLoading = true;
      this.reagentServiceProxy.getForEdit($event.radio.id)
        .pipe(finalize(() => {
          this.isTableLoading = false;
        }))
        .subscribe(res => {
          this.success(res);
        });

      return;
    }

    this.change($event)
  }

  constructor(
    private modalRef: NzModalRef,
    private reagentServiceProxy: ReagentServiceProxy,
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
    { title: '选择', index: 'id', type: 'radio', width: 50, className: 'text-truncate' },
    {
      title: '试剂编号', width: 100, index: 'no', className: 'text-truncate'
    },
    {
      title: '类型', type: 'tag', tag: {
        0: { text: '常规试剂', color: 'green' },
        1: { text: '标品试剂', color: 'yellow' },
        2: { text: '专管试剂', color: 'red' }
      }, width: 100, index: 'reagentCatalog', className: 'text-truncate'
    },
    { title: 'Cas号', width: 120, index: 'casNo', className: 'text-truncate' },
    { title: '价格', type: 'currency', width: 100, index: 'price' },
    { title: '中文名', width: 140, index: 'cnName', className: 'text-truncate' },
    { title: '中文别名', iif: () => this.showCnAliasName, width: 140, index: 'cnAliasName', className: 'text-truncate' },
    { title: '英文名', iif: () => this.showEnName, width: 140, index: 'enName', className: 'text-truncate' },
    {
      title: '安全属性', type: 'tag', tag: {
        0: { text: '易制毒', color: 'red' },
        1: { text: '易制爆', color: 'orange' }
      }, width: 140, index: 'safeAttribute', className: 'text-truncate'
    },
    { title: '存储条件', width: 140, index: 'storageCondition', className: 'text-truncate' },
    {
      title: '容量', width: 120, index: 'capacity', format: (item: ReagentDto) => {
        return `${item.capacity}${item.capacityUnit}`;
      }, className: 'text-truncate'
    },
    { title: '纯度', width: 120, index: 'purity', className: 'text-truncate' },
  ];


  success(result?: any) {
    if (result) {
      this.modalRef.close(result);
    } else {
      this.close();
    }
  }

  close($event?: MouseEvent): void {
    this.modalRef.close();
  }
}
