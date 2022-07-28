import { Pipe, PipeTransform } from '@angular/core';
import { StorageAttrEnum } from '@shared/service-proxies/service-proxies';

@Pipe({
  name: 'storageAttr'
})
export class StorageAttrPipe implements PipeTransform {

  transform(value: StorageAttrEnum, ...args: unknown[]): unknown {
    switch (value) {
      case StorageAttrEnum.爆炸品:
        return '爆炸品';
      case StorageAttrEnum.气体_易燃:
        return '气体_易燃';
      case StorageAttrEnum.气体惰性:
        return '其他物质';
      case StorageAttrEnum.易燃固体:
        return '易燃固体';
      case StorageAttrEnum.易燃液体:
        return '易燃液体';
      case StorageAttrEnum.遇水释放易燃气体:
        return '遇水释放易燃气体得性质';
      case StorageAttrEnum.易于自燃的物质_自燃:
        return '易于自燃的物质_自燃';
      case StorageAttrEnum.易于自燃的物质_发火:
        return '易于自燃的物质_发火';
      case StorageAttrEnum.氧化性物质:
        return '氧化性物质';
      case StorageAttrEnum.有机过氧化物:
        return '有机过氧化物';
      case StorageAttrEnum.毒性物质_剧毒:
        return '毒性物质_剧毒';
      case StorageAttrEnum.毒性物质_其它:
        return '毒性物质_其它';
      case StorageAttrEnum.腐蚀性物质_酸性:
        return '腐蚀性物质_酸性';
      case StorageAttrEnum.腐蚀性物质碱性及其它:
        return '腐蚀性物质碱性及其它';
      case StorageAttrEnum.其他物质:
        return '其他物质';
      default:
        return '--';
    }
  }

}
