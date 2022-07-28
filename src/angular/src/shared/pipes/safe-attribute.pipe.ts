import { Pipe, PipeTransform } from '@angular/core';
import { SafeAttributes } from '@shared/service-proxies/service-proxies';

@Pipe({
  name: 'safeAttribute'
})
export class SafeAttributePipe implements PipeTransform {

  transform(value: SafeAttributes, ...args: unknown[]): unknown {
    switch (value) {
      case SafeAttributes.易制毒:
        return '易制毒';
      case SafeAttributes.易制爆:
        return '易制爆';
      default:
        return '--';
    }
  }

}
