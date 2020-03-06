export class Announcement {
    public Title: string;
    public Content: string;
    public CreateDate: string;
    public UserID: string;
    constructor(Title : string,
                Content : string,
                CreateDate : string,
                UserID:string) {
            this.Title = Title;
            this.Content = Content;
            this.CreateDate = CreateDate;
            this.UserID = UserID;
    }

}