import { Component, OnInit, Injector } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { SettingsService } from '@delon/theme';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'layout-sidebar',
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent implements OnInit {

  themeValue: string = 'dark';

  ngOnInit(): void {
    abp.event.on('abp.theme-setting.changed', themeName => {
      switch (themeName) {
        case 'A':
        case 'B':
        case 'C':
        case 'D':
        case 'E':
          this.themeValue = 'light';
          break;
        default:
          this.themeValue = 'dark';
          break;
      }
    });
  }

  constructor(
  ) {
  }
}
