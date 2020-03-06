export class SendMailDelegateRequest {
    public toEmail: string[];
    public Title: string;
    public Action:string;
    public CreateBy: string;
    public EmailType: string;
    public EmailSubject:string;
    public ReasonReject:string;
    public Category:string;
    //Request
    public GroupName:string;
    public RequestTypeName:string;
    public DetailReason:string;
    public StartDate:string;
    public EndDate:string;
    public Sender:string;
    public Receiver:string;
    //OT Request
    // public OTDate:string;
    // public OTDateType:string;
    // public OTTimeType:string;
    // public StartTime:string;
    // public EndTime:string;
    constructor(GroupName:string,RequestTypeName:string,DetailReason:string,StartDate:string,EndDate:string,
        Title: string, CreateBy: string,toEmail:string[],Action:string,EmailType:string,EmailSubject:string,ReasonReject:string,Category:string,Sender : string,Receiver:string) {
        this.GroupName=GroupName;
        this.RequestTypeName=RequestTypeName;
        this.DetailReason=DetailReason;
        this.StartDate=StartDate;
        this.EndDate=EndDate;

        this.Title = Title;
        this.CreateBy = CreateBy; // ngtao request
        this.toEmail = toEmail;
        this.Action = Action;
        this.EmailType = EmailType;
        this.EmailSubject = EmailSubject;
        this.ReasonReject = ReasonReject;
        this.Category = Category;
        this.Sender = Sender;
        this.Receiver = Receiver;
    }
}