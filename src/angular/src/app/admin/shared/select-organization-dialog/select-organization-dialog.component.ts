import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, OnInit } from '@angular/core';

@Component({
  selector: 'app-select-organization-dialog',
  templateUrl: './select-organization-dialog.component.html',
  styles: [
  ]
})
export class SelectOrganizationDialogComponent extends ModalComponentBase implements OnInit {

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
  }

  checkedOrgId: number[];
  checkboxChange(list: number[]) {
    this.checkedOrgId = list;
  }

  select() {
    this.success(this.checkedOrgId);
  }
}
