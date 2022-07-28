import { SetAsMasterComponent } from './set-as-master/set-as-master.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { CreateOrUpdateUserComponent } from './create-or-update-user/create-or-update-user.component';
import { UserServiceProxy, WareHouseDto, CommonServiceProxy } from './../../../shared/service-proxies/service-proxies';
import { Component, OnInit, Injector } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/component-base/paged-listing-component-base';
import { UserDto, RoleDto } from '@shared/service-proxies/service-proxies';
import { STColumn } from '@delon/abc/st';
import { finalize } from 'rxjs/operators';
import { ResetPwdComponent } from './reset-pwd/reset-pwd.component';
import { SendEmailComponent } from './send-email/send-email.component';
import { ArrayService } from '@delon/util/array';
import { NzTreeNode } from 'ng-zorro-antd/tree';
import { EditUserComponent } from './edit-user/edit-user.component';


@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styles: []
})
export class UserComponent extends PagedListingComponentBase<UserDto> implements OnInit {

  filter: string;
  isActive: boolean | undefined = undefined;
  roleId: number | undefined = undefined;
  permissionKeys: any[] = [];
  nodes: NzTreeNode[] = [];
  phone: string | undefined;
  email: string | undefined;
  name: string | undefined;
  keyword: string | undefined;
  isMaster: boolean | undefined;
  warehouseId: number | undefined;
  protected fetchData(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {

    if (this.warehouseId == null) {
      this.warehouseId = undefined;
    }
    this.userServiceProxy
      .getAll(this.filter, this.isActive, this.isMaster, this.roleId, request.skipCount, request.maxResultCount)
      .pipe(finalize(() => {
        finishedCallback();
      }))
      .subscribe((result) => {
        this.dataList = result.items;
        this.totalItems = result.totalCount;
      });
  }
  protected delete(entity: UserDto): void {
    this.userServiceProxy.delete(entity.id)
      .subscribe(res => {
        this.refresh();
      });
  }

  roles: RoleDto[] = [];
  wareHouses: WareHouseDto[] = []
  ngOnInit(): void {
    this.refresh();
    this.userServiceProxy.getRoles().subscribe(res => {
      this.roles = res.items;
    });

    this.commonServiceProxy.getAllActiveWareHouse(true)
      .subscribe(res => {
        this.wareHouses = res;
      })


  }

  scroll = {
    x: '1200px'
  }

  hasCreate: boolean;
  hasDelete: boolean;
  hasUpdate: boolean;
  constructor(
    private commonServiceProxy: CommonServiceProxy,
    private arrayService: ArrayService,
    private userServiceProxy: UserServiceProxy,
    injector: Injector) {
    super(injector);
    this.hasCreate = true;
    this.hasDelete = true;
    this.hasUpdate = true;
  }



  columns: STColumn[] = [
    { title: '序号', index: 'id', className: 'text-truncate', width: 80 },
    { title: '手机号', index: 'userName', className: 'text-truncate', width: 120 },
    { title: '姓名', index: 'name', className: 'text-truncate', width: 100 },
    { title: '邮箱', index: 'emailAddress', width: 140 },
    { title: '是否专管', index: 'isMaster', type: 'yn', width: 80 },
    // { title: '手机号', index: 'phoneNumber', className: 'text-truncate', width: 100 },//
    // { title: '邮箱', index: 'emailAddress', className: 'text-truncate', width: 200 },//phoneNumber
    { title: '在职状态', index: 'isActive', type: 'yn', yn: { yes: '在职', no: '离职', mode: 'full' }, width: 80 },
    { title: '职能', index: 'roleNames', className: 'text-truncate', width: 130 },//

    {
      title: '当前选中仓库', index: 'currentSelectedWarehouseType',
      type: 'tag', tag: {
        1: { text: '试剂仓库', color: 'red' },
        2: { text: '耗材仓库', color: 'blue' },
        3: { text: '办公仓库', color: 'green' },
      },
      className: 'text-truncate', width: 140
    },
    //{ title: '登录失败次数', index: 'accessFailedCount', width: 110 },
    // { title: '解锁时间', index: 'lockoutEndDateUtc', type: 'date', width: 150 },
    //{ title: '创建时间', index: 'creationTime', type: 'date', width: 150 },
    {
      title: '操作区',
      width: 120,
      fixed: 'right',
      className: 'text-center',
      buttons: [
        {
          text: '详情',
          type: 'modal',
          modal: {
            component: UserDetailComponent,
            size: 1000
          }
        },
        {
          text: '操作',
          children: [
            // {
            //   text: '解锁',
            //   type: 'del',
            //   pop: '确定要解锁此用户吗？',
            //   click: (item: any) => {
            //     this.userServiceProxy.activate(item).subscribe(res => {
            //       this.notify.success('解锁成功！');
            //       this.refresh();
            //     });
            //   }
            // },
            {
              text: '重置密码',
              type: 'modal',
              modal: {
                component: ResetPwdComponent
              }
            },
            {
              text: '专管设置',
              type: 'modal',
              modal: {
                component: SetAsMasterComponent
              },
              click: (record: any, modal?: any) => {
                if (modal) {
                  this.refresh();
                }
              }
            },
            {
              text: '编辑',
              iif: () => this.hasUpdate,
              click: (record: any) => {
                this.edit(record)
              }
            }
          ]
        }
      ],
    },
  ];

  senUser: UserDto;
  isVisible = false;
  msg = '';
  handleOk() {
    if (!this.msg) {
      this.notify.warn('请输入要发的的消息内容！');
      return;
    }

  }
  edit(item: UserDto): void {
    this.modalHelper.createStatic(EditUserComponent, { i: item })
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  }
  create() {
    this.modalHelper.createStatic(CreateOrUpdateUserComponent, {})
      .subscribe(x => {
        if (x)
          this.refresh();
      });
  };

  sendEmail() {
    this.modalHelper.createStatic(SendEmailComponent, {})
      .subscribe(x => {

      });
  }
}
