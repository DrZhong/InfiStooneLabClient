import { Component, forwardRef, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonServiceProxy, CompanyListDto, CompanyType } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'select-supplier-company',
  templateUrl: './select-supplier-company.component.html',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectSupplierCompanyComponent),
    multi: true
  }]
})
export class SelectSupplierCompanyComponent implements OnInit, ControlValueAccessor {
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
    private commonServiceProxy: CommonServiceProxy) {
  }
  writeValue(obj: any): void {
    if (obj) {
      this._data = obj;
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
        this.list = res.filter(w => w.companyType == CompanyType.供应商);
      })
  }
}
