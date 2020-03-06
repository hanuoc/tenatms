export class ListEntitleDayFilter {
    public FullName: string[];
    public HolidayType: string[];
    constructor(FullName:string[],HolidayType: string[]) {
        this.FullName = FullName;
        this.HolidayType = HolidayType;
    }
}
