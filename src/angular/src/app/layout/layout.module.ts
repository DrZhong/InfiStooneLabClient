import { HeaderLanguageswitch } from './default/header/components/languageswitch.component';
import { NgModule } from '@angular/core';

import { HeaderComponent } from '@app/layout/default/header/header.component';
import { SidebarComponent } from '@app/layout/default/sidebar/sidebar.component';
import { HeaderSearchComponent } from '@app/layout/default/header/components/search.component';
import { HeaderNotifyComponent } from '@app/layout/default/header/components/notify.component';

import { HeaderFullScreenComponent } from '@app/layout/default/header/components/fullscreen.component';
import { HeaderStorageComponent } from '@app/layout/default/header/components/storage.component';
import { HeaderUserComponent } from '@app/layout/default/header/components/user.component';




const COMPONENTS = [
  HeaderComponent,
  SidebarComponent,
  SideBarNavComponent,
  SideBarLogoComponent,
  SidebarUserComponent
];

const HEADERCOMPONENTS = [
  HeaderSearchComponent,
  HeaderNotifyComponent,

  HeaderFullScreenComponent,
  HeaderStorageComponent,
  HeaderUserComponent,
  HeaderLanguageswitch,
  ChangePwdComponent
];

//
import { SharedModule } from '@shared/shared.module';
import { SideBarNavComponent } from '@app/layout/default/sidebar/components/sidebar-nav.component';
import { SideBarLogoComponent } from '@app/layout/default/sidebar/components/sidebar-logo.component';
import { SidebarUserComponent } from '@app/layout/default/sidebar/components/sidebar-user.component';
import { ChangePwdComponent } from './default/header/change-pwd/change-pwd.component';
import { ProfileSettingComponent } from './default/header/components/profile-setting/profile-setting.component';
import { ChatToggleButtonComponent } from './default/header/components/chat-toggle-button.component';

@NgModule({
    imports: [SharedModule],
    providers: [],
    declarations: [...COMPONENTS, ...HEADERCOMPONENTS, ProfileSettingComponent, ChatToggleButtonComponent],
    exports: [...COMPONENTS, ...HEADERCOMPONENTS]
})
export class LayoutModule { }
