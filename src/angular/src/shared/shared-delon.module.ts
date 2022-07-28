import { AvatarListModule } from '@delon/abc/avatar-list';
import { DatePickerModule } from '@delon/abc/date-picker';
import { DownFileModule } from '@delon/abc/down-file';
import { EllipsisModule } from '@delon/abc/ellipsis';
import { ErrorCollectModule } from '@delon/abc/error-collect';
import { ExceptionModule } from '@delon/abc/exception';
import { FooterToolbarModule } from '@delon/abc/footer-toolbar';
import { FullContentModule } from '@delon/abc/full-content';
import { GlobalFooterModule } from '@delon/abc/global-footer';
import { NoticeIconModule } from '@delon/abc/notice-icon';
import { PageHeaderModule } from '@delon/abc/page-header';
import { QuickMenuModule } from '@delon/abc/quick-menu';
import { ResultModule } from '@delon/abc/result';
import { TagSelectModule } from '@delon/abc/tag-select';
import { QRModule } from '@delon/abc/qr';
import { SEModule } from '@delon/abc/se';
import { STModule } from '@delon/abc/st';
import { SVModule } from '@delon/abc/sv';
import { ReuseTabModule } from '@delon/abc/reuse-tab';
import { OnboardingModule } from '@delon/abc/onboarding';
import { LayoutDefaultModule } from '@delon/theme/layout-default';
export const SHARED_DELON_MODULES = [
  LayoutDefaultModule,
  OnboardingModule,
  AvatarListModule,
  DatePickerModule,
  //DownFileModule,
  EllipsisModule,
  //ErrorCollectModule,
  ExceptionModule,
  FooterToolbarModule,
  GlobalFooterModule,
  PageHeaderModule,
  //ResultModule,
  TagSelectModule,
  NoticeIconModule,
  //QuickMenuModule,
  //SidebarNavModule,
  FullContentModule,
  SEModule,
  SVModule,
  STModule,
  ReuseTabModule,
  QRModule
];
