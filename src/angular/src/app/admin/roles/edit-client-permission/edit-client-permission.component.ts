import { finalize } from 'rxjs/operators';
import { RoleDto, WarehousePermissionContainer } from './../../../../shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { RoleServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-edit-client-permission',
  templateUrl: './edit-client-permission.component.html',
  styles: [
  ]
})
export class EditClientPermissionComponent extends ModalComponentBase implements OnInit {

  constructor(
    public roleServiceProxy: RoleServiceProxy,
    injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }

  @Input() record: RoleDto;

  i: WarehousePermissionContainer[];
  init() {
    this.roleServiceProxy.getWarehousePermissionForEdit(this.record.id)
      .subscribe(res => {

        // res.forEach((element: WarehousePermissionContainer) => {
        //   element['checkOptions'] = element.item.map(res => {
        //     return {
        //       label: res.permissionName,
        //       value: res.permission,
        //       checked: res.isGranted
        //     };
        //   });
        // });
        this.i = res;
        console.log(this.i);
      });
  }

  loading = false;
  save() {
    this.loading = true;
    this.roleServiceProxy.updateWarehousePermission(this.i)
      .pipe(finalize(() => {
        this.loading = false;
      })).subscribe(res => {
        this.notify.success('设置成功！');
      })
  }

}
