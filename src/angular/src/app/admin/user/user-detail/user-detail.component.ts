import { finalize } from 'rxjs/operators';
import { UserServiceProxy, UserDto } from '@shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styles: [
  ]
})
export class UserDetailComponent extends ModalComponentBase implements OnInit {

  constructor(injector: Injector,
    private userServiceProxy: UserServiceProxy) {
    super(injector);
  }

  ngOnInit(): void {
    this.init();
  }
  @Input() record: UserDto;

  i: any;
  init() {

    this.userServiceProxy.get(this.record.id)
      .pipe(finalize(() => {

      }))
      .subscribe(res => {
        this.i = res;
        console.log(this.i);
      });
  }

}
