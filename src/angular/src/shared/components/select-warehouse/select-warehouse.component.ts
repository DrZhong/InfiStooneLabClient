import { Component, forwardRef, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonServiceProxy, WareHouseDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'select-warehouse',
  templateUrl: './select-warehouse.component.html',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectWarehouseComponent),
    multi: true
  }]
})
export class SelectWarehouseComponent implements OnInit, ControlValueAccessor {
  _data: any;

  get data() {
    return this._data;
  }

  set data(val: any) {
    console.log(val);
    this._data = val;
    this.change(val);
  }

  change = (value: any) => { };
  constructor(
    private commonServiceProxy: CommonServiceProxy) {
  }
  writeValue(obj: any): void {
    console.log(obj);
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
  wareHouses: WareHouseDto[] = [];
  ngOnInit(): void {
    this.commonServiceProxy.getMyWareActiveWareHouse(undefined)
      .subscribe((res) => {
        this.wareHouses = res;
      });
  }

}
