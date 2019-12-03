import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from './store/state/app.state';
import { user } from './store/selectors/user.selectors';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnDestroy {
  private userSubscription: Subscription;
  username: string;

  constructor(private store: Store<AppState>) {
    this.userSubscription = this.store.select(user).subscribe(e => {
      this.username = e.username;
    });
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }
}
