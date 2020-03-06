export class ListOTFilter {
    public FullName: string[];
    public OTDateType : string[];
    public OTTimeType: string[];
    public startDate: string;
    public endDate: string;
    constructor(FullName:string[],OTDateType: string[],OTTimeType: string[], startDate: string, endDate: string) {
        this.FullName = FullName;
        this.OTDateType = OTDateType;
        this.OTTimeType = OTTimeType;
        this.startDate = startDate;
        this.endDate = endDate;
    }
}
