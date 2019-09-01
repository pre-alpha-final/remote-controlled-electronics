import { Injectable, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../store/state/app.state';
import { user } from '../store/selectors/user.selectors';
import { UpdateUser } from '../store/actions/user.actions';
import { Subscription } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { TokenResponse } from '../shared/token-response';
import { DecodedAccessToken } from '../shared/decoded-access-token';
import { LocalStorageAuthData } from './localstorage-auth-data';

interface AuthData {
  username: string;
  accessToken: string;
  refreshToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService implements OnDestroy {
  private authData: AuthData;
  private userSubscription: Subscription;
  get accessToken() {
    return this.authData.accessToken;
  }

  constructor(private store: Store<AppState>, private httpClient: HttpClient, private router: Router) {
    this.authData = new LocalStorageAuthData();
    this.userSubscription = this.store.select(user).subscribe(e => {
      this.authData.username = e.username;
      this.authData.accessToken = (e.accessToken != null && e.accessToken !== 'n/a') ? e.accessToken : this.authData.accessToken;
      this.authData.refreshToken = (e.refreshToken != null && e.refreshToken !== 'n/a') ? e.refreshToken : this.authData.refreshToken;
    });

    if (this.isTokenValid()) {
      const decodedAccessToken: DecodedAccessToken = new JwtHelperService().decodeToken(this.authData.accessToken);
      this.store.dispatch(new UpdateUser({
        username: decodedAccessToken && decodedAccessToken.username || '',
        accessToken: this.authData.accessToken,
        refreshToken: this.authData.refreshToken,
      }));
    }
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

  isAuthenticated(offset?: number): Promise<boolean> {
    return new Promise<boolean>(resolve => {
      if (this.isTokenValid(offset)) {
        return resolve(true);
      }
      this.refreshToken()
        .then(() => resolve(this.isTokenValid(offset)));
    });
  }

  private isTokenValid(offset?: number): boolean {
    const jwtHelperService = new JwtHelperService();
    if (!jwtHelperService.isTokenExpired(this.authData.accessToken, offset ? offset : 0)) {
      return true;
    }
    return false;
  }

  private refreshToken(): Promise<void> {
    return new Promise<void>(resolve => {
      if (!this.authData.refreshToken) {
        resolve();
      }
      this.httpClient.post<TokenResponse>('/api/auth/refresh', {
        refreshToken: this.authData.refreshToken,
      }).toPromise().then(e => {
        this.store.dispatch(new UpdateUser({
          accessToken: e && e.access_token || '',
          refreshToken: e && e.refresh_token || '',
        }));
        resolve();
      }).catch(e => resolve());
    });
  }
}
