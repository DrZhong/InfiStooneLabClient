import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { ReagentDto, ReagentServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-show-reagent-detail',
  templateUrl: './show-reagent-detail.component.html',
  styles: [
  ]
})
export class ShowReagentDetailComponent extends ModalComponentBase implements OnInit {

  constructor(
    private reagentServiceProxy: ReagentServiceProxy,
    injector: Injector) {
    super(injector);
  }


  ngOnInit(): void {
    this.init();
  }

  @Input() id: number;
  i: ReagentDto = new ReagentDto();
  loading = false;
  locationName = "";
  init() {
    this.loading = true;
    this.reagentServiceProxy.getForEdit(this.id)
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe(res => {
        this.i = res;
        this.locationName = res.reagentLocations.map(w => w.locationName).join(',');
      })
  }

}
