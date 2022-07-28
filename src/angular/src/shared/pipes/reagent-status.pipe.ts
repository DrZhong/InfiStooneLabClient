import { Pipe, PipeTransform } from '@angular/core';
import { ReagentStatus } from '@shared/service-proxies/service-proxies';

@Pipe({
  name: 'reagentStatus'
})
export class ReagentStatusPipe implements PipeTransform {

  transform(value: ReagentStatus, ...args: unknown[]): unknown {
    switch (value) {
      case ReagentStatus.固体:
        return '固体';
      case ReagentStatus.气体:
        return '气体';
      case ReagentStatus.液体:
        return '液体';
      default:
        return '--';
    }
  }

}
