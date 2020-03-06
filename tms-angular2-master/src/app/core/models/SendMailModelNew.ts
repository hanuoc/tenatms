export class SendMailModelNew {
    public toEmail: string[];
    public Title: string;
    public Action:string;
    public CreateBy: string;
    public EmailType: string;
    public EmailSubject:string;
    public ReasonReject:string;
    public Category:string;
    public ccMail: string[];
    //Request
    public GroupName:string;
    public RequestTypeName:string;
    public DetailReason:string;
    public StartDate:string;
    public EndDate:string;
    //OT Request
    public OTDate:string;
    public OTDateType:string;
    public OTTimeType:string;
    public StartTime:string;
    public EndTime:string;
    //Abnormal
    public ExplanationDate:string;
    public ExplanationReason:string;
    public Actual:string;
    public Description:string;
    constructor(GroupName:string,RequestTypeName:string,DetailReason:string,StartDate:string,EndDate:string,OTDate:string,OTDateType:string,OTTimeType:string,StartTime:string,EndTime:string,ExplanationDate:string,ExplanationReason:string,Actual:string,Description:string,Title: string, CreateBy: string,toEmail:string[],ccMail:string[],Action:string,EmailType:string,EmailSubject:string,ReasonReject:string,Category:string) {
        this.GroupName=GroupName;
        this.RequestTypeName=RequestTypeName;
        this.DetailReason=DetailReason;
        this.StartDate=StartDate;
        this.EndDate=EndDate;
        this.OTDate=OTDate;
        this.OTDateType=OTDateType;
        this.OTTimeType=OTTimeType;
        this.StartTime=StartTime;
        this.EndTime=EndTime;
        this.ExplanationDate=ExplanationDate;
        this.ExplanationReason=ExplanationReason;
        this.Actual=Actual;
        this.Description=Description;
        this.Title = Title;
        this.CreateBy = CreateBy;
        this.toEmail = toEmail;
        this.ccMail = ccMail;
        this.Action = Action;
        this.EmailType = EmailType;
        this.EmailSubject = EmailSubject;
        this.ReasonReject = ReasonReject;
        this.Category = Category;
    }
}
