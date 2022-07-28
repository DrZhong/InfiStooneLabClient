import { finalize } from 'rxjs/operators';
import { LocationDto, LocationStorageAttrDto } from './../../../../shared/service-proxies/service-proxies';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { CommonServiceProxy, EnumberEntityDto, LocationServiceProxy, WareHouseDto } from '@shared/service-proxies/service-proxies';
import { zip } from 'rxjs';

@Component({
  selector: 'app-edit-location',
  templateUrl: './edit-location.component.html',
  styleUrls: ['./edit-location.component.less']
})
export class EditLocationComponent extends ModalComponentBase implements OnInit {

  constructor(
    private commonServiceProxy: CommonServiceProxy,
    private locationServiceProxy: LocationServiceProxy,
    injector: Injector) {
    super(injector);
  }
  @Input() record: LocationDto;

  wareHouses: WareHouseDto[];
  enumberEntityDto: EnumberEntityDto[] = [];
  ngOnInit(): void {
    this.commonServiceProxy.getStorageAttrList().subscribe((ress) => {

      this.enumberEntityDto = ress;
      this.checkOptions = ress.map(w => {
        return {
          label: w.desction,
          value: w.enumValue,
          checked: this.record.locationStorageAttr?.findIndex(qc => qc.storageAttr == w.enumValue && qc.isActive) > -1
        };
      })

    });
    if (!this.record.id) {
      this.record.isActive = true;
    }
  }

  save() {
    this.record.locationStorageAttr = this.checkOptions
      .filter(w => w.checked)
      .map(m => {
        let e = new LocationStorageAttrDto();
        e.storageAttr = m.value
        return e;
      });
    console.log(this.record.locationStorageAttr, this.checkOptions);
    if (this.record.id) {
      this.locationServiceProxy.update(this.record)
        .pipe(finalize(() => { }))
        .subscribe(() => {
          this.notify.success('修改成功');
          this.success(1);
        })
    } else {
      this.locationServiceProxy.create(this.record)
        .pipe(finalize(() => { }))
        .subscribe(() => {
          this.notify.success('创建成功');
          this.success(1);
        })
    }
  }

  checkOptions = [];

}
