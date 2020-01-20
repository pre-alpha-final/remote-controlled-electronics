import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../store/state/app.state';
import { UpdateUser } from '../store/actions/user.actions';
import { Observable, of } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, tap } from 'rxjs/operators';
import { ErrorType } from '../shared/error-type';
import { TokenResponse } from '../shared/token-response';
import { DecodedAccessToken } from '../shared/decoded-access-token';

@Injectable({
  providedIn: 'root'
})
export class AuthApiWrapperService {
  constructor(private store: Store<AppState>, private httpClient: HttpClient, private router: Router) { }

  register(email: string, password: string, password2: string): Observable<void | ErrorType> {
    return this.httpClient.post<void | ErrorType>('/api/auth/register', {
      email: email,
      password: password,
      password2: password2
    }).pipe(catchError(e => of({ error: (e as HttpErrorResponse).error.error })));
  }

  logIn(email: string, password: string): Observable<TokenResponse | HttpErrorResponse> {
    return this.httpClient.post<TokenResponse | HttpErrorResponse>('/api/auth/login', {
      login: email,
      password: password
    }).pipe(
      tap(e => this.onNewToken(e as TokenResponse)),
      catchError(e => of(e as HttpErrorResponse))
    );
  }

  logOut() {
    this.httpClient.get<void | ErrorType>('/api/auth/logout').pipe(
      catchError(e => of({ error: 'Logging out failed' } as ErrorType)));
    this.store.dispatch(new UpdateUser({
      username: '',
      accessToken: '',
      refreshToken: '',
    }));
    this.router.navigateByUrl('/auth/login');
  }

  checkEmail(userId: string, code: string): Observable<void | ErrorType> {
    return this.httpClient.get<void | ErrorType>('/api/auth/checkemail', {
      params: {
        'userId': userId || '',
        'code': code || '',
      }
    }).pipe(catchError(e => of({ error: 'Unable to process request' } as ErrorType)));
  }

  forgotPassword(email: string): Observable<void | ErrorType> {
    return this.httpClient.post<void | ErrorType>('/api/auth/forgotpassword', {
      email: email
    }).pipe(catchError(e => of({ error: 'Unable to process request' } as ErrorType)));
  }

  resetPassword(userId: string, code: string, password: string, password2: string): Observable<void | ErrorType> {
    return this.httpClient.post<void | ErrorType>('/api/auth/resetpassword', {
      userId: userId,
      code: code,
      password: password,
      password2: password2
    }).pipe(catchError(e => of({ error: 'Unable to process request' } as ErrorType)));
  }

  private onNewToken(userData: TokenResponse) {
    if (userData == null) {
      return;
    }
    const jwtHelperService = new JwtHelperService();
    const decodedAccessToken: DecodedAccessToken = jwtHelperService.decodeToken(userData.access_token);
    this.store.dispatch(new UpdateUser({
      username: decodedAccessToken && decodedAccessToken.username || '',
      accessToken: userData && userData.access_token || '',
      refreshToken: userData && userData.refresh_token || '',
    }));
  }
}
