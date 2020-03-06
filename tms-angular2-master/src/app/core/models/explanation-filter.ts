export class ExplanationFilter{
    public Creators: string[];
    public StatusRequest: string[];
    public ReasonRequest: string[];
    public FromDate: string;
    public ToDate: string;
    public ChosenCreatorFilterSuperAdmin:string[];
    constructor(Creators: string[], StatusRequest: string[], ReasonRequest: string[],FromDate: string, ToDate: string,ChosenCreatorFilterSuperAdmin: string[]) {
        this.Creators = Creators;
        this.StatusRequest = StatusRequest;
        this.ReasonRequest = ReasonRequest;
        this.FromDate = FromDate;
        this.ToDate = ToDate;
        this.ChosenCreatorFilterSuperAdmin=ChosenCreatorFilterSuperAdmin;
    }
}
