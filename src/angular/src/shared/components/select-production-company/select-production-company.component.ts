import { Component, forwardRef, OnInit, NgZone } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonServiceProxy, CompanyListDto, CompanyType } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'select-production-company',
  templateUrl: './select-production-company.component.html',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectProductionCompanyComponent),
    multi: true
  }]
})
export class SelectProductionCompanyComponent implements OnInit, ControlValueAccessor {
  _data: any;

  get data() {
    return this._data;
  }

  set data(val: any) {
    this._data = val;
    this.change(val);
  }

  change = (value: any) => { };
  constructor(
    private ngZone: NgZone,
    private commonServiceProxy: CommonServiceProxy) {
  }
  writeValue(obj: any): void {
    if (obj) {
      this.ngZone.run(() => {
        this.data = obj;
      });

    }
  }
  registerOnChange(fn: any): void {
    this.change = fn;
  }
  registerOnTouched(fn: any): void {

  }
  setDisabledState?(isDisabled: boolean): void {

  }
  list: CompanyListDto[] = [];
  ngOnInit(): void {
    this.commonServiceProxy
      .getAllCompany(true)
      .subscribe(res => {
        this.list = res.filter(w => w.companyType == CompanyType.生产商);
      })
  }
}
