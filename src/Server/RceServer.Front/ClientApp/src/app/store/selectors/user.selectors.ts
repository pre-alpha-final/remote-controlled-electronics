import { createSelector } from '@ngrx/store';
import { AppState } from '../state/app.state';
import { UserState } from '../state/user.state';

const userSlice = (state: AppState) => state.user;

export const user = createSelector(
    userSlice,
    (state: UserState) => state
);

export const username = createSelector(
    userSlice,
    (state: UserState) => state.username
);

export const accessToken = createSelector(
    userSlice,
    (state: UserState) => state.accessToken
);

export const refreshToken = createSelector(
    userSlice,
    (state: UserState) => state.refreshToken
);
