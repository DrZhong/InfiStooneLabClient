import { AppComponentBase } from '@shared/app-component-base';
import { Injector, OnInit, Component } from '@angular/core';
import { STChange } from '@delon/abc/st';



export class PagedResultDto {
  items: any[];
  totalCount: number;
}

export class EntityDto {
  id: number;
}

export class PagedRequestDto {
  skipCount: number;
  maxResultCount: number;
}

@Component({
  template: ''
})
export abstract class PagedListingComponentBase<EntityDto>
  extends AppComponentBase
  implements OnInit {
  public pageSize = 10;
  public pageNumber = 1;
  public totalPages = 1;
  public totalItems: number;
  public isTableLoading = true;
  public sorting = "";

  dataList: EntityDto[];


  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh(backToFirstPage: boolean = false): void {
    if (backToFirstPage) {
      this.pageNumber = 1;
    }
    this.getDataPage(this.pageNumber);
  }

  public getDataPage(page: number): void {
    if (page === 0) {
      page = 1;
    }
    const req = new PagedRequestDto();
    req.maxResultCount = this.pageSize;
    req.skipCount = (page - 1) * this.pageSize;

    this.isTableLoading = true;
    this.fetchData(req, page, () => {
      this.isTableLoading = false;
    });
  }

  /**
   * 基本翻页
   * @param $event 
   */
  change($event: STChange) {
    if ($event.type == 'pi' || $event.type == 'ps') {
      this.pageNumber = $event.pi;
      this.pageSize = $event.ps;
      this.refresh();
      return;
    };
    if ($event.type == 'sort') {

      let d = $event.sort.value == "ascend" ? "asc" : "desc"
      this.sorting = `${$event.sort.column.sort} ${d}`;
      this.refresh();
      return;
    }

  }

  protected abstract fetchData(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void;
  protected abstract delete(entity: EntityDto): void;
}
