export interface UserState {
    username: string;
    accessToken: string;
    refreshToken: string;
}

export const initialUserState: UserState = {
  username: 'n/a',
  accessToken: 'n/a',
  refreshToken: 'n/a',
};
