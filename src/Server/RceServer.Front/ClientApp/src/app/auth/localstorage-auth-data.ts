export class LocalStorageAuthData {
  username: string;
  get accessToken(): string {
    return localStorage.getItem('accessToken') || '';
  }
  set accessToken(value: string) {
    localStorage.setItem('accessToken', value);
  }
  get refreshToken(): string {
    return localStorage.getItem('refreshToken') || '';
  }
  set refreshToken(value: string) {
    localStorage.setItem('refreshToken', value);
  }
}
