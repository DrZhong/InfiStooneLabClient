import { Pipe, PipeTransform } from '@angular/core';
import dayjs from 'dayjs'
@Pipe({
  name: 'timeLocalize'
})
export class TimeLocalizePipe implements PipeTransform {

  transform(value: Date, ...args: unknown[]): unknown {
    return dayjs(value).fromNow();;
  }

}
