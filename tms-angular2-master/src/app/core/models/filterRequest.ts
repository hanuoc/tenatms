export class FilterRequest {
    Creators:string[];
    StatusRequest: string[];
    RequestType:string[];
    RequestReasonType:string[];
    StartDate:string;
    EndDate:string;

    constructor(Creators:string[], StatusRequest: string[], RequestType: string[], RequestReasonType: string[],StartDate,EndDate){
        this.Creators=Creators;
        this.StatusRequest = StatusRequest;
        this.RequestType=RequestType;
        this.RequestReasonType=RequestReasonType;
        this.StartDate=StartDate;
        this.EndDate=EndDate;
    }
}