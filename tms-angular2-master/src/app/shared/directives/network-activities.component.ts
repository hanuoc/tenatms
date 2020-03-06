import { Component, ElementRef, Inject, Input, OnInit } from '@angular/core';
@Component({
	selector: 'network-activities',
	template: `<div  id="abnormalChart" class="demo-placeholder" style="width: 100%; height:320px;" ></div>`
})


export class FlotCmp implements OnInit {
	@Input() private dataset: any;
	@Input() private showdataLateComing: boolean = false;
	@Input() private showdataEarlyLeaving: boolean = false;
	@Input() private showdataLeave: boolean = false;
	@Input() private showdataUnusedEarlyLeaving: boolean = false;
	@Input() private showdataUnusedLateComing: boolean = false;
	@Input() private showdataUnusedLeave: boolean = false;
	@Input() private showdataWithoutCheckIn: boolean = false;
	@Input() private showdataWithoutCheckOut: boolean = false;
	@Input() private showdataWithoutCheckInOut: boolean = false;

	@Input() private resetdata: boolean;
	private options;
	private dataLateComing;
	private dataEarlyLeaving;
	private dataLeave;
	private dataUnusedAuthorizedEarlyLeaving;
	private dataUnusedAuthorizedLateComing;
	private dataUnusedAuthorizedLeave;
	private dataOTWithoutCheckIn;
	private dataOTWithoutCheckOut;
	private dataOTWithoutCheckInOut;

	constructor(public el: ElementRef) {
		this.options = {
			grid: {
				show: true,
				aboveData: true,
				color: "#3f3f3f",
				labelMargin: 10,
				axisMargin: 0,
				borderWidth: 0,
				borderColor: null,
				minBorderMargin: 5,
				clickable: true,
				hoverable: true,
				autoHighlight: true,
				mouseActiveRadius: 100
			},
			series: {
				lines: {
					show: true,
					fill: true,
					lineWidth: 2,
					steps: false
				},
				points: {
					show: true,
					radius: 4.5,
					symbol: "circle",
					lineWidth: 3.0
				}
			},
			legend: {
				position: "ne",
				margin: [0, -25],
				noColumns: 0,
				labelBoxBorderColor: null,
				labelFormatter: function (label, series) {
					return label + '&nbsp;&nbsp;';
				},
				width: 40,
				height: 1,
				show: false
			},
			colors: ['#96CA59', '#3F97EB', 'red', '#6f7a8a', '#f7cb38', '#5a8022', '#2c7282'],
			shadowSize: 0,
			tooltip: true,
			tooltipOpts: {
				content: "%y.0, %s",
				xDateFormat: "%d/%m",
				shifts: {
					x: -30,
					y: -50
				},
				defaultTheme: false
			},
			yaxis: {
				min: 0,
				tickSize: 50

			},
			xaxis: {
				mode: "time",
				tickSize: [1, "day"],
				timeformat: "%d/%m",
				timezone: "browser"
			},
			
		}
	}//end of constructor
	ngOnInit() {
		this.plot();
	}
	ngOnChanges() {
		this.plot();
	}
	plot() {
		this.dataLateComing = [];
		this.dataEarlyLeaving = [];
		this.dataLeave = [];

		this.dataUnusedAuthorizedEarlyLeaving = [];
		this.dataUnusedAuthorizedLateComing = [];
		this.dataUnusedAuthorizedLeave = [];
		this.dataOTWithoutCheckIn = [];
		this.dataOTWithoutCheckOut = [];
		this.dataOTWithoutCheckInOut = [];
		this.dataset.forEach(element => {
			this.dataLeave.push([this.gd(element['AbnormalDate']), element['UnauthorizedLeave']]);
			this.dataLateComing.push([this.gd(element['AbnormalDate']), element['LateComing']]);
			this.dataEarlyLeaving.push([this.gd(element['AbnormalDate']), element['EarlyLeaving']]);

			this.dataUnusedAuthorizedEarlyLeaving.push([this.gd(element['AbnormalDate']), element['UnusedAuthorizedEarlyLeaving']]);
			this.dataUnusedAuthorizedLateComing.push([this.gd(element['AbnormalDate']), element['UnusedAuthorizedLateComing']]);
			this.dataUnusedAuthorizedLeave.push([this.gd(element['AbnormalDate']), element['UnusedAuthorizedLeave']]);
			this.dataOTWithoutCheckIn.push([this.gd(element['AbnormalDate']), element['OTWithoutCheckIn']]);
			this.dataOTWithoutCheckOut.push([this.gd(element['AbnormalDate']), element['OTWithoutCheckOut']]);
			this.dataOTWithoutCheckInOut.push([this.gd(element['AbnormalDate']), element['OTWithoutCheckInOut']]);
		});
		let plotArea = $(this.el.nativeElement).find('div');
		$.plot(plotArea, [
			{
				label: "Unauthorized Leave",
				data: this.dataLeave,
				lines: {
					show: this.showdataLeave,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataLeave,
					fillColor: "#fff"
				}
				
			}
			,
			{
				label: "Unauthorized Late-Coming",
				data: this.dataLateComing,
				lines: {
					show: this.showdataLateComing,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataLateComing,
					fillColor: "#fff"
				}
			}
			,
			{
				label: "Unauthorized Early-Leaving",
				data: this.dataEarlyLeaving,
				lines: {
					show: this.showdataEarlyLeaving,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataEarlyLeaving,
					fillColor: "#fff"
				}
			},
			{
				label: "Unused Early Leaving",
				data: this.dataUnusedAuthorizedEarlyLeaving,
				lines: {
					show: this.showdataUnusedEarlyLeaving,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataUnusedEarlyLeaving,
					fillColor: "#fff"
				}
			},
			{
				label: "Unused Late Coming",
				data: this.dataUnusedAuthorizedLateComing,
				lines: {
					show: this.showdataUnusedLateComing,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataUnusedLateComing,
					fillColor: "#fff"
				}
			},
			{
				label: "Unused Leave",
				data: this.dataUnusedAuthorizedLeave,
				lines: {
					show: this.showdataUnusedLeave,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataUnusedLeave,
					fillColor: "#fff"
				}
			},
			{
				label: "OT Without Check In",
				data: this.dataOTWithoutCheckIn,
				lines: {
					show: this.showdataWithoutCheckIn,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataWithoutCheckIn,
					fillColor: "#fff"
				}
			},
			{
				label: "OT Without Check Out",
				data: this.dataOTWithoutCheckOut,
				lines: {
					show: this.showdataWithoutCheckOut,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataWithoutCheckOut,
					fillColor: "#fff"
				}
			},
			{
				label: "OT Without Check In Out",
				data: this.dataOTWithoutCheckInOut,
				lines: {
					show: this.showdataWithoutCheckInOut,
					fillColor: "rgba(150, 202, 89, 0.12)"
				},
				points: {
					show: this.showdataWithoutCheckInOut,
					fillColor: "#fff"
				}
			}], this.options);

		$("<div id='tooltip'></div>").css({
			position: "absolute",
			display: "none",
			border: "1px solid #fdd",
			padding: "2px",
			"background-color": "#fee",
			opacity: 0.80
		}).appendTo("body");

		plotArea.bind("plothover", function (event, pos, item) {
			var str = "(" + pos.x.toFixed(2) + ", " + pos.y.toFixed(2) + ")";
			$("#hoverdata").text(str);
			if (item) {
				var y = item.datapoint[1];
				$("#tooltip").html(item.series.label + " = " + y)
					.css({ top: item.pageY + 5, left: item.pageX + 5 })
					.show();

			} else {
				$("#tooltip").hide();
			}
		});



	}

	private gd(date) {
		return new Date(date).getTime();
	}
}