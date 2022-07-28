import { Component, OnInit, Injector, Input } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { OrganizationUnitServiceProxy, CreateOrganizationUnitInput, OrganizationUnitDto } from '@shared/service-proxies/service-proxies';
import { SFSchema } from '@delon/form';
import { finalize } from 'rxjs/operators';


@Component({
  selector: 'app-create-or-update-organization',
  templateUrl: './create-or-update-organization.component.html',
  styles: []
})
export class CreateOrUpdateOrganizationComponent extends ModalComponentBase {

  @Input() organizationUnit: OrganizationUnitDto;

  i: CreateOrganizationUnitInput;
  loading = false;
  constructor(
    public organizationUnitServiceProxy: OrganizationUnitServiceProxy,
    injector: Injector) {
    super(injector);
  }
  schema: SFSchema = {
    properties: {
      parentName: {
        type: 'string',
        title: '父部门',
        ui: {
          widget: 'text'
        }
      },
      displayName: {
        type: 'string',
        title: '组织架构名称'
      },
      sort: {
        type: 'integer',
        title: '顺序',
        default: 0
      }
    },
    required: ['displayName']
  };


  save(obj: OrganizationUnitDto) {
    // console.log(obj);
    // return;
    this.loading = true;
    if (obj.id) {
      this.organizationUnitServiceProxy.updateOrganizationUnit(obj)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(x => {
          this.success(true);
        });
    } else {
      this.organizationUnitServiceProxy.createOrganizationUnit(obj)
        .pipe(finalize(() => {
          this.loading = false;
        }))
        .subscribe(x => {
          this.success(true);
        });
    }

  }
}
