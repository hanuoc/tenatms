export class FilterTimeSheet {
    StatusExplanation: string[];
    AbnormalTimeSheetType:string[];
    FromDate: string;
    ToDate: string;
    FullName: string [];
    constructor(statusRequest: string[],abnormalTimeSheetType: string[], fromDate: string, toDate: string,FullName: string []) {
        this.StatusExplanation = statusRequest;
        this.FromDate = fromDate;
        this.ToDate = toDate;
        this.AbnormalTimeSheetType=abnormalTimeSheetType;
        this.FullName = FullName;
    }

}