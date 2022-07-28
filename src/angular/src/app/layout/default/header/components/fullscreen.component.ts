import { Component, HostListener, Injector } from '@angular/core';
import * as screenfull from 'screenfull';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'header-fullscreen',
  template: `
  <div (click)="click()">
    <i nz-icon nzType="{{status ? 'shrink' : 'arrows-alt'}}"></i>
    {{ status ? '关闭全屏' : '全屏' }}
  </div>
  `
})
export class HeaderFullScreenComponent extends AppComponentBase {
  status = false;

  constructor(
    injector: Injector
  ) {
    super(injector);
  }

  @HostListener('window:resize')
  _resize() {
    this.status = screenfull.isEnabled;
  }

  click() {
    if (screenfull.isEnabled) {
      screenfull.toggle();
    }
  }
}
