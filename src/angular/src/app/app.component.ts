
import { Component, OnInit } from '@angular/core';
import { SignalRAspNetCoreHelper } from '@shared/helpers/SignalRAspNetCoreHelper';
import { AppComponentBase } from '@shared/app-component-base';
import { Injector } from '@angular/core';
import { AfterViewInit } from '@angular/core';
import { SettingsService, TitleService, MenuService, Menu } from '@delon/theme';
import { Router } from '@angular/router';
import { NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { HostBinding } from '@angular/core';
import { MenuItem } from '@shared/layout/menu-item';
import { LayoutDefaultOptions } from '@delon/theme/layout-default';
import { CacheService } from '@delon/cache';
import { SelectCurrentWarehouseComponent } from '@appshared/select-current-warehouse/select-current-warehouse.component';
@Component({
  selector: 'app-app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less'],
  host: {
    '[class.alain-default]': 'true',
  },
})
export class AppComponent extends AppComponentBase
  implements OnInit, AfterViewInit {
  @HostBinding('class.alain-default__fixed')
  get isFixed() {
    return this.settings.layout.fixed;
  }
  @HostBinding('class.alain-default__boxed')
  get isBoxed() {
    return this.settings.layout.boxed;
  }
  @HostBinding('class.alain-default__collapsed')
  get isCollapsed() {
    return this.settings.layout.collapsed;
  }
  themeValue = "dark";
  // 全局的菜单
  Menums = [
    // 首页
    new MenuItem(this.l('HomePage'), '', 'home', '/app/home'),
    // 租户
    new MenuItem(
      this.l('Tenants'),
      'Pages.Tenants',
      'contact',
      '/app/public/contact',
    ),
    new MenuItem(
      this.l('统计分析'),
      'Pages.Reagent.Tongji',
      'line-chart',
      '/app/baseinfo/statistical',
    ),
    new MenuItem(this.l('试剂入库'), 'Pages.Reagent', 'database', '', [
      new MenuItem(this.l('专管试剂入库'), 'Pages.Reagent.Ruku', 'fire', '/app/reagent/master'),
      new MenuItem(this.l('普通试剂入库'), 'Pages.Reagent.Ruku', 'funnel-plot', '/app/reagent/common'),
    ]),
    new MenuItem(this.l('试剂出库'), 'Pages.Reagent', 'ordered-list', '', [
      new MenuItem(this.l('我的出库单'), 'Pages.Reagent.ChuKuDan', 'unordered-list', '/app/reagent/myoutorder'),
      new MenuItem(this.l('出库单管理'), 'Pages.Reagent.ChuKuDan.Manager', 'block', '/app/reagent/alloutorder'),
    ]),
    new MenuItem(this.l('专管试剂审核'), 'Pages.Reagent.DoubleConfirm,Pages.Reagent.ClientConfirm', 'check-square', '', [
      new MenuItem(this.l('入库双人双锁审核'), 'Pages.Reagent.DoubleConfirm', 'control', '/app/reagent/doubleConfirm'),
      new MenuItem(this.l('入库终端审核'), 'Pages.Reagent.ClientConfirm', 'experiment', '/app/reagent/clientConfirm'),

      new MenuItem(this.l('出库双人双锁审核'), 'Pages.Reagent.DoubleConfirm', 'control', '/app/reagent/orderdoubleConfirm'),
      new MenuItem(this.l('出库终端审核'), 'Pages.Reagent.ClientConfirm', 'experiment', '/app/reagent/orderclientConfirm'),
    ]),
    new MenuItem(this.l('操作记录'), 'Pages.Reagent.CaozuoJilu', 'history', '', [
      new MenuItem(this.l('专管操作记录'), 'Pages.Reagent.CaozuoJilu', 'control', '/app/reagent/master-record'),
      new MenuItem(this.l('普通操作记录'), 'Pages.Reagent.CaozuoJilu', 'menu', '/app/reagent/common-record'),
    ]),
    new MenuItem(this.l('库存查询'), 'Pages.Reagent.KucunChaxun', 'file-search', '', [
      new MenuItem(this.l('专管库存查询'), 'Pages.Reagent.KucunChaxun', 'fund-projection-screen', '/app/reagent/master-search'),
      new MenuItem(this.l('普通库存查询'), 'Pages.Reagent.KucunChaxun', 'folder', '/app/reagent/common-search'),
    ]),
    new MenuItem(this.l('设备监控'), 'Pages.Reagent.CaozuoJilu', 'one-to-one', '', [
      new MenuItem(this.l('视频监控'), 'Pages.Reagent.CaozuoJilu', 'video-camera', '/app/reagent/video-monitor'),
      new MenuItem(this.l('温湿度监控'), 'Pages.Reagent.CaozuoJilu', 'alert', '/app/reagent/temper-monitor'),
    ]),
    new MenuItem(this.l('系统设定'), 'Pages.Administrator', 'appstore-o', '', [
      // 角色
      new MenuItem(this.l('Roles'), 'Pages.Administrator.Roles', 'safety', '/app/admin/roles'),
      // 用户
      new MenuItem(this.l('Users'), 'Pages.Administrator.Users', 'user', '/app/admin/users'),
      new MenuItem(this.l('用户指纹'), 'Pages.Administrator.Users', 'holder', '/app/admin/userfinger'),
      new MenuItem(this.l('职能管理'), 'Pages.Administrator.Position', 'apartment', '/app/admin/position'),
      new MenuItem(this.l('部门管理'), 'Pages.Administrator.OrganizationUnits', 'apartment', '/app/admin/organization'),
      new MenuItem(this.l('仓库管理'), 'Pages.Administrator.WareHouse', 'bank', '/app/admin/warehouse'),
      new MenuItem(this.l('设置'), 'Pages.Administrator.Setting', 'setting', '/app/admin/setting'),
      new MenuItem(this.l('数据字典'), 'Pages.Administrator.Dict', 'book', '/app/admin/dict'),
      new MenuItem(this.l('审计日志'), 'Pages.Administrator.Audit', 'unordered-list', '/app/admin/auditlog')
    ]),
    new MenuItem(this.l('基础信息'), 'Pages.BaseInfo,', 'appstore-o', '', [

      new MenuItem(this.l('试剂信息'), 'Pages.Reagent.XinxiGuanli', 'book', '/app/baseinfo/reagent'),
      new MenuItem(this.l('厂商信息'), 'Pages.BaseInfo.Company', 'unordered-list', '/app/baseinfo/company'),
      new MenuItem(this.l('字典表管理'), 'Pages.BaseInfo.Dict', 'unordered-list', '/app/baseinfo/dict'),
      new MenuItem(this.l('库位管理'), 'Pages.BaseInfo.Location', 'unordered-list', '/app/baseinfo/location')
    ]),
  ];

  options: LayoutDefaultOptions = {
    logoExpanded: `./assets/images/logos/EAGLENOS.svg`,
    logoCollapsed: `./assets/images/logos/EAGLENOS.svg`,
  };
  constructor(
    private _localStorageService: CacheService,//LocalStorageService,
    injector: Injector,
    private settings: SettingsService,
    private router: Router,
    private titleSrv: TitleService,

    private menuService: MenuService,
  ) {
    super(injector);

    // 创建菜单

    const arrMenu = new Array<Menu>();
    this.processMenu(arrMenu, this.Menums);

    this.menuService.add(arrMenu);
  }

  ngOnInit(): void {
    this.drawerVisible = this._localStorageService.get('app.chat.isOpen', { mode: 'none', type: 's' });
    this.router.events
      .pipe(filter(evt => evt instanceof NavigationEnd))
      .subscribe(() => this.titleSrv.setTitle());

    SignalRAspNetCoreHelper.initSignalR(() => {
      //还是不要开在线聊天了吧 太卡
      //this._chatSignalrService.init();
    });

  }
  hideTab = false;

  ngAfterViewInit(): void {
    // ($ as any).AdminBSB.activateAll();
    // ($ as any).AdminBSB.activateDemo();
    abp.event.on('app.message.topbar.click', () => {
      this.drawerVisible = !this.drawerVisible;
    });
    abp.event.on('app.message.topbar.command', (flag: boolean) => {
      this.drawerVisible = flag;
    });

    this.checkCurrentUserWarehouse();
  }

  checkCurrentUserWarehouse() {
    if (this.appSession.user.userName != 'admin1') {
      //如果不是超级管理员，则需要选择所属仓库
      if (!this.appSession.user.currentSelectedWarehouseType) {
        this.modalHelper.createStatic(SelectCurrentWarehouseComponent, {}, {
          size: 'md',
          modalOptions: {
            nzClosable: false,
            nzKeyboard: false
          }
        }).subscribe(() => { })
      }
    }
  }

  drawerVisible = false;
  // 处理生成菜单
  processMenu(resMenu: Menu[], menus: MenuItem[], isChild?: boolean) {
    menus.forEach(item => {
      let subMenu: Menu;
      subMenu = {
        text: item.displayName,
        link: item.route,
        group: true,
        target: item.target,
        icon: `${item.icon}`,
        hide: item.hide,
      };
      if (item.permission !== '' && !this.isGranted(item.permission)) {
        subMenu.hide = true;
      }

      if (item.childMenus && item.childMenus.length > 0) {
        subMenu.children = [];
        this.processMenu(subMenu.children, item.childMenus);
      }

      resMenu.push(subMenu);
    });


  }
}
