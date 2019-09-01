import { InjectionToken } from '@angular/core';

export const JQ_TOKEN = new InjectionToken('jQuery');

export function jQueryFactory() {
  return window['jQuery'];
}

export const JQUERY_PROVIDER = [
  { provide: JQ_TOKEN, useFactory: jQueryFactory }
];
