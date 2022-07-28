import { finalize } from 'rxjs/operators';
import { AppConsts } from './../../../../shared/AppConsts';
import { Component, Injector, OnInit } from '@angular/core';
import { SFSchema } from '@delon/form';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { ReagentServiceProxy } from '@shared/service-proxies/service-proxies';
import { NzUploadFile } from 'ng-zorro-antd/upload';

@Component({
  selector: 'app-import-by-excel',
  templateUrl: './import-by-excel.component.html',
  styles: [
  ]
})
export class ImportByExcelComponent extends ModalComponentBase {

  constructor(
    private reagentServiceProxy: ReagentServiceProxy,
    injector: Injector) {
    super(injector);
  }


  downUrl: string;
  loading = false;
  // clientConfirm = false;
  // doubleConfirm = false;
  save(v: any) {
    if (v.token) {
      this.loading = true;
      this.reagentServiceProxy.excelImport(v.token)
        .pipe(finalize(() => {
          this.loading = false;
        })).subscribe(res => {
          this.notify.success(res);
          this.success(1);
        })
    } else {

      this.notify.warn('请先上传excl文件');
    }

  }

  ngOnInit(): void {
    this.downUrl = AppConsts.appBaseUrl + '/assets/reagent.tpl.xlsx';
  }



  schema: SFSchema = {
    type: "object",
    properties: {
      down: {
        title: '模板',
        type: 'string',
        ui: {
          widget: 'custom'
        }
      },
      token: {
        type: 'string',
        title: '选择文件',
        ui: {
          widget: 'upload',
          action: `${AppConsts.remoteServiceBaseUrl}/home/UploadExcel`,
          // fileType: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
          resReName: 'result',
          beforeUpload: (file: NzUploadFile) => {
            console.log(file);
            if (file.type.toLocaleLowerCase() == 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
              return true;
            }
            this.notify.warn('只允许上传后缀为 .xlsx 的excl文件！')
            return false;
          }
        }
      },
    },
    required: ['token']
  };

}
