export class FilterReport {
    ListUserID: string [];
    StartDate:string;
    EndDate:string;

    constructor(ListUserID: string [],StartDate:string,EndDate:string){
        this.ListUserID = ListUserID;
        this.StartDate=StartDate;
        this.EndDate=EndDate;
    }
}