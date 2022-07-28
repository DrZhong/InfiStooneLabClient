import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';

import { MenuItem } from '@shared/layout/menu-item';

import { AppComponentBase } from '@shared/app-component-base';
import { MenuService, Menu, SettingsService } from '@delon/theme';
import { Router, NavigationEnd, ActivatedRoute, Data } from '@angular/router';
import { filter, map, mergeMap } from 'rxjs/operators';

@Component({
    selector: 'abp-sidebar-nav',
    templateUrl: './sidebar-nav.component.html',
    styleUrls: ["./sidebar-nav.component.less"]
})
export class SideBarNavComponent extends AppComponentBase implements OnInit {

    //菜单是通过MenuService中获取，设置菜单的位置在src/app/app.component.ts中。之所以这么干是因为在其他组件中需要获得菜单信息，因此将菜单数据放到服务中，以便共享给其他组件使用
    list: Menu[];
    collapsed: boolean = false;
    themeValue: string = 'dark';

    constructor(
        injector: Injector,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        public settings: SettingsService,
        private menuService: MenuService
    ) {
        super(injector);
        this.list = this.menuService.menus;

        // router.events
        //     .pipe(filter(e => e instanceof NavigationEnd))
        //     .pipe(map(() => this.activatedRoute))
        //     .pipe(map(route => {
        //         while (route.firstChild) {
        //             route = route.firstChild;
        //         }
        //         return route;
        //     }))
        //     .pipe(mergeMap(route => {
        //         return route.data;
        //     }))
        //     .subscribe(res => this.openStatus(res));
    }

    private openStatus(res: Data) {
        if (res['skip']) return;
        let item: Menu;
        const url = this.router.url;
        const inFn = (list: Menu[], parent: Menu) => {
            for (const i of list) {
                i._open = false;
                i._selected = false;
                if (!item && i.link === url) {
                    item = i;
                    i._selected = true;
                }
                if (i.children && i.children.length > 0) {
                    inFn(i.children, i);
                }

            }
        };
        inFn(this.list, null);
        do {
            if (!item) {
                continue;
            }
            item = item.__parent;
            if (item) item._open = true;
        } while (item);

    }

    ngOnInit() {
        // event.on
        abp.event.on('abp.theme-setting.collapsed', collapsed => {
            this.collapsed = collapsed;
        });

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

        const self = this;

        setTimeout(() => {
            self.collapsed = self.settings.layout.collapsed;
        }, 1);
    }

    showMenuItem(menuItem): boolean {
        if (menuItem.permissionName) {
            return this.permission.isGranted(menuItem.permissionName);
        }
        return true;
    }


    hasChildren(item: Menu): boolean {
        if (item.children && item.children.length == 0) {
            return false;
        }
        return true;
    }

    canShow(item: Menu): boolean {
        return !item.hide;
    }
}