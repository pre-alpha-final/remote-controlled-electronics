import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from './store/state/app.state';
import { accessToken } from './store/selectors/user.selectors';
import { AuthApiWrapperService } from './auth/auth-api-wrapper.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html'
})
export class NavBarComponent implements OnDestroy {
  private userSubscription: Subscription;
  loggedIn: boolean;

  constructor(private store: Store<AppState>, private authApiWrapperService: AuthApiWrapperService) {
    this.userSubscription = this.store.select(accessToken).subscribe(e => this.loggedIn = !!e && e !== 'n/a');
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

  logOut(): void {
    this.authApiWrapperService.logOut();
  }
}
