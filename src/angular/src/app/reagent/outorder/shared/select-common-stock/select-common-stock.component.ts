import { Component, Injector, OnInit, Input } from '@angular/core';
import { CommonSearchDetailComponent } from '@app/reagent/search/common-search/common-search-detail/common-search-detail.component';
import { ShowReagentDetailComponent } from '@app/reagent/shared/show-reagent-detail/show-reagent-detail.component';
import { STChange, STColumn } from '@delon/abc/st';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { ClientStockDto, NormalReagentStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-select-common-stock',
  templateUrl: './select-common-stock.component.html',
  styles: [
  ]
})
export class SelectCommonStockComponent extends PagedListingComponentBase<ClientStockDto> implements OnInit {

  no: string | undefined;
  casNo: string | undefined;
  purity: string | undefined;
  storageCondition: string | undefined;
  @Input()
  warehouseId: number | undefined;
  stockStatus: number | undefined;
  filter: string | undefined;



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
        true,
        true,
        this.filter,
        this.sorting,
        request.skipCount, request.maxResultCount)
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
    private modalRef: NzModalRef,
    private normalReagentStockServiceProxy: NormalReagentStockServiceProxy,
    injector: Injector) {
    super(injector);
  }


  success(result?: any) {
    if (result) {
      this.modalRef.close(result);
    } else {
      this.close();
    }
  }
  selectedReagentStockListDto: ClientStockDto[] = [];
  close($event?: MouseEvent): void {
    this.modalRef.close();
  }
  select() {
    this.success(this.selectedReagentStockListDto);
  }

  changeOne($event: STChange) {
    if ($event.type == 'checkbox') {
      this.selectedReagentStockListDto = $event.checkbox as ClientStockDto[];
      console.log(this.selectedReagentStockListDto)
      return;
    }
    this.change($event);
  }
  columns: STColumn[] = [
    { title: 'ID', index: 'id', width: 40, type: 'checkbox', className: 'text-truncate' },
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
    {
      title: '库位', width: 100, index: 'locationName',
    },
    { title: '库存', width: 120, index: 'num', type: 'number' },
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
    { title: '存储条件', width: 180, index: 'storageCondition', className: 'text-truncate' }

  ];


}
