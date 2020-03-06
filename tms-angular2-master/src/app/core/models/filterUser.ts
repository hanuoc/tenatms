export class FilterUser {
    UserID: string[];
    GroupID: string[];
    Active: boolean[];

    constructor( UserID:string[],GroupID: string[],Active: boolean[]){
            this.UserID = UserID;
            this.GroupID = GroupID;
            this.Active = Active;
    }
}