export class FilterOTRequest {
    public StatusRequestType: string[];
    public OTTimeType: string[];
    public OTDateType: string[];
    public FullName: string[];
    public startDate: string;
    public endDate: string;
    constructor(StatusReuqestType : string[],
                OTTimeType : string[],
                OTDateType : string[],
                FullName:string[],
                startDate:string,
                endDate:string) {
                    this.StatusRequestType  = StatusReuqestType;
                    this.OTDateType = OTDateType;
                    this.OTTimeType = OTTimeType;
                    this.FullName = FullName;
                    this.startDate = startDate;
                    this.endDate = endDate;
    }

}