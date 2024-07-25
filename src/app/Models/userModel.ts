import { IRoleModel } from "./roleModel";

export interface IUserModel {
    id: string,
    username: string,
    email: string,
    isauthenticated: boolean,
    message: string,
    roles: string[],
    token: string,
    refreshtoken: string,
    refreshtokenexpiration: Date
}