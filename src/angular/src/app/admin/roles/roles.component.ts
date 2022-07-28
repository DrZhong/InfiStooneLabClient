import { EditClientPermissionComponent } from './edit-client-permission/edit-client-permission.component';

import { Component, OnInit, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import {
  PagedListingComponentBase,
  PagedRequestDto,
} from '@shared/component-base/paged-listing-component-base';
import {
  RoleDto,
  RoleServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { ArrayService } from '@delon/util';

import { CreatOrUpdateRoleComponent } from '@app/admin/roles/creat-or-update-role/creat-or-update-role.component';
import { STColumn } from '@delon/abc/st';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styles: [],
})
export class RolesComponent extends PagedListingComponentBase<RoleDto> {
  constructor(
    arrayService: ArrayService,
    injector: Injector, private rolesService: RoleServiceProxy) {
    super(injector);
  }
  reName = {};



  protected fetchData(
    request: PagedRequestDto,
    pageNumber: number,
    finishedCallback: Function,
  ): void {
    this.rolesService
      .getAll("", request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }



  columns: STColumn[] = [
    { title: '角色', index: 'name' },
    { title: '角色名称', index: 'displayName' },
    {
      title: '操作区',
      buttons: [
        {
          text: '删除',
          type: 'del',
          click: (record: any) => {
            this.delete(record);
          }
        },
        {
          text: '客户端权限',
          type: 'modal',
          modal: {
            component: EditClientPermissionComponent
          }
        },
        {
          text: '编辑',
          click: (record: any) => {
            this.createOrUpdate(record)
          }
        }
      ],
    },
  ];

  protected delete(entity: RoleDto): void {
    this.rolesService
      .delete(entity.id)
      .pipe(finalize(() => {
        abp.notify.info('Deleted Role: ' + entity.displayName);
        this.refresh();
      }))
      .subscribe(() => { });
  }

  createOrUpdate(item: RoleDto) {
    this.modalHelper
      .createStatic(CreatOrUpdateRoleComponent, item)
      .subscribe(isSave => {
        if (isSave) {
          this.refresh();
        }
      });
  }

  create(): void {
    this.modalHelper
      .createStatic(CreatOrUpdateRoleComponent, {})
      .subscribe(isSave => {
        if (isSave) {
          this.refresh();
        }
      });
  }

  edit(item: RoleDto): void {
    // this.modalHelper
    //   .open(EditRoleComponent, { id: item.id }, 'md', {
    //     nzMask: true,
    //   })
    //   .subscribe(isSave => {
    //     if (isSave) {
    //       this.refresh();
    //     }
    //   });
  }
}
