import { initialUserState, UserState } from '../state/user.state';
import { UserActions, UserActionsEnum } from '../actions/user.actions';

export function userReducers(state = initialUserState, action: UserActions): UserState {
    switch (action.type) {
        case UserActionsEnum.UpdateUser: {
            return {
                ...state,
                ...action.payload,
            };
        }

        default:
            return state;
    }
}
