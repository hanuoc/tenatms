export class SendMailModel {
    public toEmail: string[];
    public Title: string;
    public Action:string;
    public CreateBy: string;
    public EmailType: string;
    public EmailSubject:string;
    public ReasonReject:string;
    public Category:string;
    
    constructor(Title: string, CreateBy: string,toEmail:string[],Action:string,EmailType:string,EmailSubject:string,ReasonReject:string,Category:string) {
        this.Title = Title;
        this.CreateBy = CreateBy;
        this.toEmail = toEmail;
        this.Action = Action;
        this.EmailType = EmailType;
        this.EmailSubject = EmailSubject;
        this.ReasonReject = ReasonReject;
        this.Category = Category;
    }
}
