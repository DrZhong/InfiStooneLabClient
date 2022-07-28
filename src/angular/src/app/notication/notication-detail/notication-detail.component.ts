
import { finalize } from 'rxjs/operators';
import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationServiceProxy, UserNotificationDto } from '@shared/service-proxies/service-proxies';
@Component({
  selector: 'app-notication-detail',
  templateUrl: './notication-detail.component.html',
  styles: [
  ]
})
export class NoticationDetailComponent extends AppComponentBase implements OnInit {

  constructor(
    private router: Router,
    private notificationServiceProxy: NotificationServiceProxy,
    injector: Injector, private activatedRoute: ActivatedRoute,) {
    super(injector);
  }


  id: string;
  ngOnInit(): void {
    this.activatedRoute.params.subscribe(p => {
      this.id = p['id'];
      this.refresh();
    });
  }

  i: UserNotificationDto = new UserNotificationDto();
  loadding = false;
  link = '';
  from = '';
  refresh() {
    this.loadding = true;
    this.notificationServiceProxy.getUserNotificationById(this.id)
      .pipe(finalize(() => {
        this.loadding = false;
      }))
      .subscribe(res => {
        this.i = res;
        this.i['title'] = res.notification.data.properties['Message'];
        this.link = res.notification.data.properties['Link'];
        this.from = res.notification.data.properties['userFullName'];
      })
  }
  back() {
    this.router.navigateByUrl('/app/notication');
  }

  msg = '';
  send() {

  }
}
