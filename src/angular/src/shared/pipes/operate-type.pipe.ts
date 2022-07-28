import { Pipe, PipeTransform } from '@angular/core';
import { OperateTypeEnum } from '@shared/service-proxies/service-proxies';

@Pipe({
  name: 'operateType'
})
export class OperateTypePipe implements PipeTransform {

  transform(value: OperateTypeEnum, ...args: unknown[]): unknown {
    switch (value) {
      case OperateTypeEnum.入库:
        return '入库';
      case OperateTypeEnum.领用:
        return '领用';
      case OperateTypeEnum.归还:
        return '归还';
      case OperateTypeEnum.回收:
        return '回收';
      default:
        return '--';
    }
  }

}
