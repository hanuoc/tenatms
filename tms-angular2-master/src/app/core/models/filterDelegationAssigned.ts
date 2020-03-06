export class filterDelegationAssigned {
    public usernameAssigned: string[];
    public statusRequestType: string[];
    public startDate: string;
    public endDate: string;
    constructor(usernameAssigned: string[], statusRequest: string[],
        startDate: string,
        endDate: string) {
            this.usernameAssigned = usernameAssigned;
            this.statusRequestType = statusRequest;
            this.startDate = startDate;
            this.endDate = endDate;
    }
}