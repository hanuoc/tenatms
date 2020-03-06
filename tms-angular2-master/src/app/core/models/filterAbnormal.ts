export class FilterAbnormal {
    AbnormalReason:string[];
    StatusRequestsss:string[];
    AbnormalReasonTypeFilter:string[];
    FullName: string [];
    StartDate:string;
    EndDate:string;

    constructor(FullName: string [],StatusRequestsss: string[], AbnormalReason: string[], AbnormalReasonTypeFilter: string[], StartDate:string,EndDate:string){
        this.FullName = FullName;
        this.StatusRequestsss = StatusRequestsss;
        this.AbnormalReason=AbnormalReason;
        this.AbnormalReasonTypeFilter=AbnormalReasonTypeFilter;
        this.StartDate=StartDate;
        this.EndDate=EndDate;
    }
}