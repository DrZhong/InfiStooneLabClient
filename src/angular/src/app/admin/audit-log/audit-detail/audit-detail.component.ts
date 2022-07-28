import { Component, OnInit, Input, Injector } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { AuditLogListDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-audit-detail',
  templateUrl: './audit-detail.component.html',
  styles: []
})
export class AuditDetailComponent extends ModalComponentBase implements OnInit {

  @Input() record: AuditLogListDto;
  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit() {

  }

}
