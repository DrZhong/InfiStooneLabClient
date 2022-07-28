import { Injectable } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';

@Injectable()
export class FileDownloadService {

    downloadTempFile(fileToken: string) {
        const url = AppConsts.remoteServiceBaseUrl + '/File/DownloadTempFile?fileToken=' + fileToken;
        location.href = url; //TODO: This causes reloading of same page in Firefox
    }
}
