import { STColumn } from '@delon/abc/st';

//import { SelectUserComponent } from './../../layout/common/select-user/select-user.component';


import { OrganizationUnitServiceProxy, UserToOrganizationUnitInput, OrganizationUnitUserListDto, MoveOrganizationUnitInput, UsersToOrganizationUnitInput, UserDto } from './../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector, TemplateRef, ViewChild } from '@angular/core';
import { OrganizationUnitDto } from '@shared/service-proxies/service-proxies';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { NzTreeNode, NzFormatEmitEvent, NzTreeComponent, NzFormatBeforeDropEvent } from 'ng-zorro-antd/tree';
import { NzContextMenuService, NzDropdownMenuComponent } from 'ng-zorro-antd/dropdown';
import { ArrayService } from '@delon/util';
import { CreateOrUpdateOrganizationComponent } from './create-or-update-organization/create-or-update-organization.component';
import { SelectUserComponent } from '../shared/select-user/select-user.component';
import { finalize } from 'rxjs/operators';
import { STChange } from '@delon/abc/st';
@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.less']
})
export class OrganizationComponent extends PagedListingComponentBase<OrganizationUnitDto> {
  protected delete(entity: OrganizationUnitDto): void {

  }
  dropdown: NzContextMenuService;
  activedNode: NzTreeNode;
  activeOrg: OrganizationUnitDto;
  contextMenu($event: { event: MouseEvent, node: NzTreeNode }, menu: NzDropdownMenuComponent): void {

    this.activedNode = $event.node;
    this.nzContextMenuService.create($event.event, menu);
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


  selectDropdown(): void {
    if (this.dropdown)
      this.dropdown.close();

    var insertOrg = new OrganizationUnitDto();
    if (this.activedNode) {
      insertOrg.parentId = this.activedNode.origin.id;
      insertOrg['parentName'] = this.activedNode.origin.displayName;
    }

    this.modalHelper
      .create(CreateOrUpdateOrganizationComponent, { organizationUnit: insertOrg })
      .subscribe(isSave => {
        if (isSave) {
          this.refresh();
        }
      });
  }
  create(): void {
    this.activedNode = undefined;
    this.activeOrg = new OrganizationUnitDto();
    this.selectDropdown();
  }

  updateNode() {
    if (this.dropdown)
      this.dropdown.close();
    this.modalHelper
      .create(CreateOrUpdateOrganizationComponent, { organizationUnit: this.activedNode.origin })
      .subscribe(isSave => {
        if (isSave) {
          this.refresh();
        }
      });
  }

  deleteNode() {

    abp.message.confirm('你确定要删除此节点吗？', '提示', x => {
      if (x) {
        this.organizationUnitService.deleteOrganizationUnit(<number><any>this.activedNode.key).subscribe(x => {
          this.refresh();
        })
      }
    })

  }

  // 选中节点
  activeNode(data: NzFormatEmitEvent): void {

    if (this.activedNode == data.node) {
      return;
    }


    this.activeOrg = data.node.origin as unknown as OrganizationUnitDto;
    this.activedNode = data.node;
    this.loadUser(1);

  }
  users: OrganizationUnitUserListDto[] = [];

  show(spage: STChange) {
    if (spage.type == 'pi' || spage.type == 'ps') {
      this.pageNumber = spage.pi;
      this.pageSize = spage.ps;
      this.loadUser(this.pageNumber);
    }

  }
  isTableLoading = false;

  loadingUser = false;
  loadUser(page: number) {
    if (page === 0) {
      page = 1;
    }
    const req = new PagedRequestDto();
    req.maxResultCount = this.pageSize;
    req.skipCount = (page - 1) * this.pageSize;
    this.loadingUser = true;
    this.organizationUnitService.getOrganizationUnitUsers(
      this.activeOrg.id, undefined, '',
      '', req.skipCount, req.maxResultCount)
      .pipe(finalize(() => {
        this.loadingUser = false;
      }))
      .subscribe((result) => {
        this.users = result.items;
        this.users.forEach(ele => {
          if (ele.orgMasterCode == this.activeOrg.code) {
            ele.isOrgMaster = true;
          }
        });
        this.totalItems = result.totalCount;
      });
  }

  selectUser() {
    //为部门添加用户
    this.modalHelper
      .create(SelectUserComponent, {})
      .subscribe((data: UserDto[]) => {
        if (data && data.length > 0) {

          let parama = new UsersToOrganizationUnitInput();
          parama.init({
            userIds: data.map(x => x.id)
          });
          parama.organizationUnitId = this.activeOrg.id;

          //this.isTableLoading = true;
          this.organizationUnitService.addUsersToOrganizationUnit(parama)
            .pipe(finalize(() => {
              //  this.isTableLoading = false;
            }))
            .subscribe(res => {
              this.loadUser(1);
            })
        }
      });
  }


  protected fetchData(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void {
    this.activedNode = null;
    this.organizationUnitService.getOrganizationUnits()
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.nodes = this.arrayService.arrToTreeNode(this.dataList, {
          idMapName: 'id',
          parentIdMapName: 'parentId',
          titleMapName: 'displayName',
          cb: w => {
            w.expanded = w.code.length < 15;
          }
        });

        //  console.log(this.nodes);

      })
  }

  constructor(
    private nzContextMenuService: NzContextMenuService,
    public arrayService: ArrayService,
    public organizationUnitService: OrganizationUnitServiceProxy,
    injector: Injector) {
    super(injector);
  }
  mouseAction(name: string, e: NzFormatEmitEvent): void {
    if (name == 'drop') {

      let msg = `确定要把部门【${e.dragNode.title}】 转移到部门 【${e.node.title}】 下吗？`;
      this.message.confirm(msg, '提示', res => {
        if (res) {
          this.activeOrg = null;
          const input = new MoveOrganizationUnitInput();
          input.id = <number><any>e.dragNode.key;
          input.newParentId = e.node.key ? <number><any>e.node.key : undefined; //(!data.parent || data.parent === '#') ? undefined : data.parent;
          this.organizationUnitService.moveOrganizationUnit(input)
            .subscribe(() => {
              this.notify.success(this.l('移动成功'));
              this.refresh();
            });
        } else {
          this.refresh();
        }
      });


    }
  }


  nodes: any[] = [];


  nzExpandAll = true;

  @ViewChild('tree', { static: true }) mytree: NzTreeComponent;
  expandAllToggle() {
    this.nzExpandAll = !this.nzExpandAll;
    this.mytree.getTreeNodes().forEach(w => w.isExpanded = this.nzExpandAll);
  }

  sync() {
  }
  searchValue = "";
  columns: STColumn[] = [
    { title: '用户名', index: 'userName' },
    { title: '用户名称', index: 'name' },
    { title: '部门负责人', index: 'isOrgMaster', type: 'yn', yn: { mode: 'full' } },
    {
      title: '操作区',
      buttons: [
        {
          text: '移出部门',
          type: 'del',
          popTitle: "确认移出吗？",
          click: (record: any) => {
            this.organizationUnitService.removeUserFromOrganizationUnit(record.id, this.activeOrg.id)
              .pipe(finalize(() => {
                //  this.isTableLoading = false;
                this.loadUser(1);
              })).subscribe(res => {
                abp.notify.success('移出成功');
              });
          }
        }
      ],
    },
  ];
}
