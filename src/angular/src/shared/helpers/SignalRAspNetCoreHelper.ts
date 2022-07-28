import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from 'abp-ng2-module';
import { environment } from '@env/environment';
export class SignalRAspNetCoreHelper {
  static initSignalR(callback: () => void): void {

    if (!environment.production) {
      // return;
    }

    const encryptedAuthToken = new UtilsService().getCookieValue(
      AppConsts.authorization.encrptedAuthTokenName,
    );

    abp.signalr = {
      autoConnect: true, // _zone.runOutsideAngular in ChatSignalrService
      // autoReconnect: true,
      connect: undefined,
      hubs: undefined,
      qs:
        AppConsts.authorization.encrptedAuthTokenName +
        '=' +
        encodeURIComponent(encryptedAuthToken),
      remoteServiceBaseUrl: AppConsts.remoteServiceBaseUrl,
      startConnection: undefined,
      url: '/signalr',
    };

    let script = document.createElement('script');
    script.onload = () => {
      callback();
    };

    script.src = AppConsts.appBaseUrl + '/assets/abp/abp.signalr-client.js';
    document.head.appendChild(script);
    //console.log('============================');
    // jQuery.getScript(
    //   //AppConsts.appBaseUrl + '/assets/abp/abp.signalr.js',
    //   AppConsts.appBaseUrl + '/assets/abp/abp.signalr-client.js',
    // );
  }
}
