import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'authError'
})
export class AuthErrorPipe implements PipeTransform {
  transform(value: any, args?: any): any {
    return JSON.stringify(value).includes('invalid_username_or_password')
    ? 'Invalid username or password'
    : value;
  }
}
