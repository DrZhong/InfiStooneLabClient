import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  ExternalLoginProviderInfoModel,
  ExternalAuthenticateModel,
} from '@shared/service-proxies/service-proxies';

import {
  TokenAuthServiceProxy,
  AuthenticateModel,
  AuthenticateResultModel,
} from '@shared/service-proxies/service-proxies';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { AppConsts } from '@shared/AppConsts';

//import * as _ from 'lodash';
// import { UtilsService } from 'abp-ng2-module/dist/src/utils/utils.service';
// import { MessageService } from 'abp-ng2-module/dist/src/message/message.service';
// import { TokenService } from 'abp-ng2-module/dist/src/auth/token.service';
// import { LogService } from 'abp-ng2-module/dist/src/log/log.service';
import { UtilsService, MessageService, TokenService, LogService, LocalizationService } from 'abp-ng2-module';
import { finalize, map } from 'rxjs/operators';
@Injectable()
export class LoginService {
  static readonly twoFactorRememberClientTokenName =
    'TwoFactorRememberClientToken';

  authenticateModel: AuthenticateModel;
  authenticateResult: AuthenticateResultModel;

  rememberMe: boolean;

  constructor(
    private _tokenAuthService: TokenAuthServiceProxy,
    private _router: Router,
    private _utilsService: UtilsService,
    private _messageService: MessageService,
    private _tokenService: TokenService,
    private _logService: LogService,
  ) {
    this.clear();
  }

  returnUrl: string;

  setReturnUrl(returnUrl: string) {
    this.returnUrl = returnUrl;
  }

  authenticate(finallyCallback: () => void, returnUrl: string): void {
    this.returnUrl = returnUrl;
    finallyCallback = finallyCallback || (() => { });

    this._tokenAuthService
      .authenticate(this.authenticateModel)
      .pipe(finalize(() => {
        finallyCallback();
      }))
      .subscribe((result) => {
        this.processAuthenticateResult(result);
      });
  }

  private processAuthenticateResult(
    authenticateResult: AuthenticateResultModel,
  ) {
    this.authenticateResult = authenticateResult;

    if (authenticateResult.accessToken) {
      // Successfully logged in
      // tslint:disable-next-line:max-line-length
      this.login(
        authenticateResult.accessToken,
        authenticateResult.encryptedAccessToken,
        authenticateResult.expireInSeconds,
        this.rememberMe,
      );
    } else {
      // Unexpected result!

      this._logService.warn('Unexpected authenticateResult!');
      this._router.navigate(['account/login']);
    }
  }

  login(
    accessToken: string,
    encryptedAccessToken: string,
    expireInSeconds: number,
    rememberMe?: boolean,
  ): void {
    const tokenExpireDate = rememberMe
      ? new Date(new Date().getTime() + 1000 * expireInSeconds)
      : undefined;

    this._tokenService.setToken(accessToken, tokenExpireDate);

    this._utilsService.setCookieValue(
      AppConsts.authorization.encrptedAuthTokenName,
      encryptedAccessToken,
      tokenExpireDate,
      abp.appPath,
    );

    let initialUrl = this.returnUrl;//UrlHelper.initialUrl;
    if (initialUrl.indexOf('/account') > 0) {
      initialUrl = AppConsts.appBaseUrl;
    }
    location.href = initialUrl;
  }

  private clear(): void {
    this.authenticateModel = new AuthenticateModel();
    this.authenticateModel.rememberClient = false;
    this.authenticateResult = null;
    this.rememberMe = false;
  }
}
