import { Action } from '@ngrx/store';

export enum UserActionsEnum {
    UpdateUser = '[User] UpdateUser',
}

export interface UserPayload {
    username?: string;
    accessToken?: string;
    refreshToken?: string;
}

export class UpdateUser implements Action {
    readonly type = UserActionsEnum.UpdateUser;
    constructor(public payload: UserPayload) {}
}

export type UserActions = UpdateUser;
