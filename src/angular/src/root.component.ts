import { Component, OnInit } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { TitleService } from '@delon/theme';

@Component({
  selector: 'app-root',
  template: `<router-outlet></router-outlet>`,
})
export class RootComponent implements OnInit {
  constructor(
    private titleService: TitleService,
  ) {
    // G2.track(false);

  }

  ngOnInit(): void {
    this.titleService.suffix = "原石智能实验室管理系统";
  }
}
