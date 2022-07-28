import { Injector, ElementRef, Component } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  template: ''
})
export abstract class ModalComponentBase extends AppComponentBase {
  title = '';
  modalRef: NzModalRef;

  constructor(injector: Injector) {
    super(injector);
    this.modalRef = injector.get(NzModalRef);
  }

  success(result?: any) {
    if (result) {
      this.modalRef.close(result);
    } else {
      this.close();
    }
  }

  close($event?: MouseEvent): void {
    this.modalRef.close();
  }
}
