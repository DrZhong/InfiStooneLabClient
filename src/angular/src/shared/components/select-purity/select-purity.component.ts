import { Component, forwardRef, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonServiceProxy, DictDto } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'select-purity',
  templateUrl: './select-purity.component.html',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SelectPurityComponent),
    multi: true
  }]
})
export class SelectPurityComponent implements OnInit, ControlValueAccessor {

  _data: any;

  get data() {
    return this._data;
  }

  set data(val: any) {

    if (val == null) {
      val = '';
    }
    this._data = val;
    this.change(val);
  }

  change = (value: any) => { };
  constructor(private commonServiceProxy: CommonServiceProxy) { }
  writeValue(obj: any): void {
    if (obj) {
      this.data = obj;

    }
  }
  registerOnChange(fn: any): void {
    this.change = fn;
  }
  registerOnTouched(fn: any): void {

  }
  setDisabledState?(isDisabled: boolean): void {

  }

  list: DictDto[] = [];
  ngOnInit(): void {
    this.commonServiceProxy
      .getPuritySelectList()
      .subscribe(res => {
        this.list = res;
      });
  }

}
