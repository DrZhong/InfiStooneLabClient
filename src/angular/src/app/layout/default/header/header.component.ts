import { CacheService } from '@delon/cache';
import { of, Observable } from 'rxjs';
import { Component, ViewChild, ViewEncapsulation } from '@angular/core';
import { OnboardingService } from '@delon/abc/onboarding';
import { SettingsService } from '@delon/theme';
@Component({
  selector: 'layout-header',
  templateUrl: './header.component.html',
  encapsulation: ViewEncapsulation.None,
  styles: [`
  .alain-default__nav > li {
     padding: 8px;
    }
    .alain-default__nav > li:hover {
       border:1px solid #9a9a9a;
     }
  `]
})
export class HeaderComponent {
  searchToggleStatus: boolean;

  constructor(
    private cache: CacheService,
    private srv: OnboardingService,
    public settings: SettingsService) { }

  toggleCollapsedSideabar() {
    const collapsed = !this.settings.layout.collapsed;
    this.settings.setLayout('collapsed', collapsed);
    abp.event.trigger('abp.theme-setting.collapsed', collapsed);
  }


  searchToggleChange() {
    this.searchToggleStatus = !this.searchToggleStatus;
  }
  compact: false;
  ngOnInit(): void {

  }





  changeTheme(theme: boolean): boolean {
    if (theme) {
      const style = document.createElement('link');
      style.type = 'text/css';
      style.rel = 'stylesheet';
      style.id = 'compact-theme';
      style.href = 'assets/themes/ng-zorro-antd.compact.min.css';
      document.body.appendChild(style);
    } else {
      const dom = document.getElementById('compact-theme');
      if (dom) {
        dom.remove();
      }
    }
    return false;
  }


}
