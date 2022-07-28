export class MenuItem {
  displayName = '';
  permission = '';
  icon = '';
  route = '';
  childMenus: MenuItem[];
  target: '_blank' | '_self' | '_parent' | '_top';
  hide = false;

  constructor(
    displayName: string,
    permission: string,
    icon: string,
    route: string,
    childMenus: MenuItem[] = null,
    hide: boolean = false,
    target: '_blank' | '_self' | '_parent' | '_top' = '_self'
  ) {
    this.displayName = displayName;
    this.permission = permission;
    this.icon = icon;
    this.route = route;
    this.hide = hide;
    this.target = target;
    if (childMenus) {
      this.childMenus = childMenus;
    } else {
      this.childMenus = [];
    }
  }
}
