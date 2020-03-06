
export class ExplanationRequest {
    public Title: string;
    public ReasonDetail: string;
    public TimeSheetId: number;
    public ReceiverId: string;
    public CreatedBy: string;
    public StatusRequestId: number;
    public Actual: string;
    public GroupId:string;
    public DateExplanation:string;
    public ReasonList:string
    constructor(title: string, timeSheetId: number, reasonDetail: string,
        receiver: string, createdBy: string, statusRequestId: number,groupId:string,dateExplanation,reasonList) {
        this.Title = title;
        this.TimeSheetId = timeSheetId;
        this.ReasonDetail = reasonDetail;
        this.ReceiverId = receiver;
        this.CreatedBy = createdBy;
        this.StatusRequestId = statusRequestId;
        this.Actual = "";
        this.GroupId= groupId;
        this.DateExplanation=dateExplanation;
        this.ReasonList=reasonList;
    }
}