export class IHolidayModel {
    public ID?: string;
    public Note?: string;
    public Date: Date;
    public Workingday?: Date;
}
export class HolidayModel implements IHolidayModel {
    public ID?: string;
    public Note?: string;
    public Date: Date;
    public Workingday?: Date;
    constructor(data?: IHolidayModel) {
        if (data) {
            for (const property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
}
