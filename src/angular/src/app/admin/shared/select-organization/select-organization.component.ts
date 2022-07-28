import { AppComponentBase } from '@shared/app-component-base';
import { Component, OnInit, Injector, Output, EventEmitter, Input, ViewChild } from '@angular/core';
import { NzTreeNode, NzFormatEmitEvent, NzTreeComponent, NzFormatBeforeDropEvent } from 'ng-zorro-antd/tree';
import { ArrayService } from '@delon/util';
import { OrganizationUnitServiceProxy, OrganizationUnitDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
@Component({
  selector: 'select-organization',
  templateUrl: './select-organization.component.html',
  styles: [
  ]
})
export class SelectOrganizationComponent extends AppComponentBase implements OnInit {

  constructor(
    public arrayService: ArrayService,
    public organizationUnitService: OrganizationUnitServiceProxy,
    injector: Injector) {
    super(injector);
  }

  isTableLoading = false;
  ngOnInit(): void {
    this.refresh();
  }

  @Input() showCheckBox = false;

  nodes: any[] = [];
  refresh() {
    this.isTableLoading = true;
    this.organizationUnitService.getOrganizationUnits()
      .pipe(finalize(() => {
        this.isTableLoading = false;
      }))
      .subscribe((result) => {
        this.nodes = this.arrayService.arrToTreeNode(result.items, {
          idMapName: 'id',
          parentIdMapName: 'parentId',
          titleMapName: 'displayName',
          cb: w => {
            w.expanded = w.code.length < 15;
          }
        });
      })
  }

  openFolder(data: NzTreeNode | Required<NzFormatEmitEvent>): void {
    // do something if u want
    if (data instanceof NzTreeNode) {
      data.isExpanded = !data.isExpanded;
    } else {
      const node = data.node;
      if (node) {
        node.isExpanded = !node.isExpanded;
      }
    }
  }

  @ViewChild('tree', { static: true }) tree: NzTreeComponent;
  @Output() checkboxChange: EventEmitter<number[]> = new EventEmitter<number[]>();
  nzCheckBoxChange(e: NzFormatEmitEvent) {
    if (!this.showCheckBox) return;
    this.checkboxChange.emit(this.arrayService.getKeysByTreeNode(this.tree.getTreeNodes()));
  }


  @Output() orgChange: EventEmitter<OrganizationUnitDto> = new EventEmitter<OrganizationUnitDto>();
  activedNode: NzTreeNode;
  activeOrg: OrganizationUnitDto;
  // 选中节点
  activeNode(data: NzFormatEmitEvent): void {

    if (this.activedNode == data.node) {
      return;
    }


    //this.activeOrg = as unknown as OrganizationUnitDto;
    this.activedNode = data.node;
    //this.loadUser(1);
    this.orgChange.emit(data.node.origin as unknown as OrganizationUnitDto);
  }


}
