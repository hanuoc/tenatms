export class LoggedInUser {
    constructor(access_token: string, id:string,groupId:string, username: string, fullName: string, email: string, avatar: string,roles:any,permissions:any
    ) {
        this.access_token = access_token;
        this.id = id;
        this.fullName = fullName;
        this.username = username;
        this.email = email;
        this.roles = roles;
        this.permissions = permissions;
        this.id =id;
        this.groupId = groupId;
    }
    public id: string;
    public access_token: string;
    public username: string;
    public fullName: string;
    public email: string;
    public permissions: any;
    public roles: any;
    public groupId : string;
}