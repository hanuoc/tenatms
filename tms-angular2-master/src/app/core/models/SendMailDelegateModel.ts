export class SendMailDelegateModel {
    public toEmail: string[];
    public Title: string;
    public Action:string;
    public CreateBy: string; // người tạo giải trình
    public EmailType: string;
    public EmailSubject:string;
    public ReasonReject:string;
    public Category:string;
    // add
    public GroupName: string;
    public EndDate: string;
    public ExplanationReason: string;
    public Description: string;
    public Receiver : string;  // người nhận mail
    public Sender : string;  // người Gửi mail
    constructor(Title: string, CreateBy: string,toEmail:string[],Action:string,EmailType:string,EmailSubject:string,ReasonReject:string,
        Category:string,GroupName: string,EndDate: string,ExplanationReason: string,Description: string,Receiver: string,Sender) {
        this.Title = Title;
        this.CreateBy = CreateBy;
        this.toEmail = toEmail;
        this.Action = Action;
        this.EmailType = EmailType;
        this.EmailSubject = EmailSubject;
        this.ReasonReject = ReasonReject;
        this.Category = Category;
        this.GroupName= GroupName;
        this.EndDate= EndDate;
        this.ExplanationReason= ExplanationReason;
        this.Description= Description;
        this.Receiver= Receiver;
        this.Sender= Sender;
    }
}