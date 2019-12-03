import { ActionReducerMap } from '@ngrx/store';
import { userReducers } from './user.reducers';
import { AppState } from '../state/app.state';

export const appReducers: ActionReducerMap<AppState, any> = {
    user: userReducers,
};
