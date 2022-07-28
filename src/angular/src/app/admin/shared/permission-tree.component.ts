import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { NzTreeNode, NzTreeComponent } from 'ng-zorro-antd/tree';

@Component({
  selector: 'permission-tree',
  template:
    `
    <nz-tree #menus [nzData]="nodes" [nzCheckable]="true" (nzCheckBoxChange)="nzCheckBoxChange($event)" [nzMultiple]="true"
      [nzCheckStrictly]="true" [nzCheckedKeys]="checkedKeys">
    </nz-tree>
    `,
  styles: []
})
export class PermissionTreeComponent {

  @Input()
  nodes: NzTreeNode[] = [];

  @Input()
  checkedKeys: string[] = [];
  @Output()
  checkedKeysChange = new EventEmitter<string[]>();
  @ViewChild('menus', { static: false }) menus: NzTreeComponent;


  nzCheckBoxChange(e) {
    // console.log(this.menus.getCheckedNodeList());
    this.checkedKeysChange.emit(this.menus.getCheckedNodeList().map(w => w.key));
  }

}
