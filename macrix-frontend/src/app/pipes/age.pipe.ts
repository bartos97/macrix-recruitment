import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'age'
})
export class AgePipe implements PipeTransform {

  transform(value: Date): number {
    if (!value) return 0;
    
    const today = new Date();
    const m = today.getMonth() - value.getMonth();
    let age = today.getFullYear() - value.getFullYear();
    if (m < 0 || (m === 0 && today.getDate() < value.getDate())) {
      age--;
    }
    return age >= 0 ? age : 0;
  }

}
