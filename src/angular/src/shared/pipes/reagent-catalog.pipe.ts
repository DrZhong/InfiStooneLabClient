import { Pipe, PipeTransform } from '@angular/core';
import { ReagentCatalog } from '@shared/service-proxies/service-proxies';

@Pipe({
  name: 'reagentCatalog'
})
export class ReagentCatalogPipe implements PipeTransform {

  transform(value: ReagentCatalog, ...args: unknown[]): unknown {
    switch (value) {
      case ReagentCatalog.常规试剂:
        return '常规试剂';
      case ReagentCatalog.标品试剂:
        return '标品试剂';
      default:
        return '--';
    }
  }

}
