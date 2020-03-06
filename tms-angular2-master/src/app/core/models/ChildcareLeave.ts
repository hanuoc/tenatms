export class ChildcareLeave {
    constructor(id: any, startdate: any, enddate: any, isLateComing: any, isEarlyLeaving: any, time: any) {
        this.Id = id;
        this.StartDate = startdate;
        this.EndDate = enddate;
        this.IsLateComing = isLateComing;
        this.IsEarlyLeaving = isEarlyLeaving;
        this.Time = time;
    }
    public Id: any
    public StartDate: any;
    public EndDate: any;
    public IsLateComing: any;
    public IsEarlyLeaving: any;
    public Time: any;
}