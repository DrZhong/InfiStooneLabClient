import { SettingsService } from '@delon/theme';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { Component, Injector, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile-setting',
  templateUrl: './profile-setting.component.html',
  styles: [
  ]
})
export class ProfileSettingComponent extends ModalComponentBase implements OnInit {


  opp = true;
  constructor(
    public settingsService: SettingsService,
    injector: Injector) {
    super(injector);
  }
  ngModelChange(value) {
    this.settingsService.setData('tabs', value);
  }

  ngOnInit(): void {
    this.opp = this.settingsService.getData('tabs');
  }

}
