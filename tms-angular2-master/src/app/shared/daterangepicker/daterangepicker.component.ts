import { Directive, OnInit, AfterViewInit, Input, Output, EventEmitter } from '@angular/core';
import { KeyValueDiffer, KeyValueDiffers, ElementRef, OnDestroy, DoCheck, OnChanges, SimpleChanges  } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { DaterangepickerConfig } from '../../core/services/config.service';

import * as $ from "jquery";
import * as moment from 'moment';
import 'bootstrap-daterangepicker';

import { DateTimeConstants } from './../../core/common/datetime.constants'

@Directive({
    selector: '[daterangepicker]',
})
export class DaterangePickerComponent implements AfterViewInit, OnDestroy, DoCheck,OnChanges {

    private activeRange: any;
    private targetOptions: any = {};
    private _differ: any = {};

    public datePicker: any;

    // daterangepicker properties
    @Input() options: any = {};

    // daterangepicker events
    @Output() selected = new EventEmitter();
    @Output() cancelDaterangepicker = new EventEmitter();
    @Output() applyDaterangepicker = new EventEmitter();
    @Output() hideCalendarDaterangepicker = new EventEmitter();
    @Output() showCalendarDaterangepicker = new EventEmitter();
    @Output() hideDaterangepicker = new EventEmitter();
    @Output() showDaterangepicker = new EventEmitter();

    constructor(
        private input: ElementRef,
        private config: DaterangepickerConfig,
        private differs: KeyValueDiffers
    ) {
    }

    ngAfterViewInit() {
        this.config.embedCSS();
        this.render();
        this.attachEvents();
    }

    render() {
        this.targetOptions = Object.assign({}, this.config.settings, this.options);

        // cast $ to any to avoid jquery type checking
        (<any>$(this.input.nativeElement)).daterangepicker(this.targetOptions, this.callback.bind(this));

        this.datePicker = (<any>$(this.input.nativeElement)).data(DateTimeConstants.DATE_RANGE_PICKER);
    }

    attachEvents() {
        $(this.input.nativeElement).on(DateTimeConstants.CANCEL_DATE_RANGE_PICKER,
            (e:any, picker:any) => {
                let event = { event: e, picker: picker };
                this.cancelDaterangepicker.emit(event);
            }
        );

        $(this.input.nativeElement).on(DateTimeConstants.APPLY_DATE_RANGE_PICKER,
            (e:any, picker:any) => {
                let event = { event: e, picker: picker };
                this.applyDaterangepicker.emit(event);
            }
        );

        $(this.input.nativeElement).on(DateTimeConstants.HIDECALENDAR_DATE_RANGE_PICKER,
            (e:any, picker:any) => {
                let event = { event: e, picker: picker };
                this.hideCalendarDaterangepicker.emit(event);
            }
        );

        $(this.input.nativeElement).on(DateTimeConstants.SHOWCALENDAR_DATE_RANGE_PICKER,
            (e:any, picker:any) => {
                let event = { event: e, picker: picker };
                this.showCalendarDaterangepicker.emit(event);
            }
        );

        $(this.input.nativeElement).on(DateTimeConstants.HIDE_DATE_RANGE_PICKER,
            (e:any, picker:any) => {
                let event = { event: e, picker: picker };
                this.hideDaterangepicker.emit(event);
            }
        );

        $(this.input.nativeElement).on(DateTimeConstants.SHOW_DATE_RANGE_PICKER,
            (e:any, picker:any) => {
                let event = { event: e, picker: picker };
                this.showDaterangepicker.emit(event);
            }
        );
    }

    private callback(start?: any, end?: any, label?: any): void {
        this.activeRange = {
            start: start,
            end: end,
            label: label
        }

        this.selected.emit(this.activeRange);
    }

    destroyPicker() {
        try {
            (<any>$(this.input.nativeElement)).data(DateTimeConstants.DATE_RANGE_PICKER).remove();
        } catch(e) {
        }
    }

    ngOnDestroy() {
        this.destroyPicker();
    }

    ngDoCheck() {
    }

    ngOnChanges(changes: SimpleChanges): void {
        if ('options' in changes && changes['options']) {
            const options = changes['options'].currentValue;
            this.render();
            this.attachEvents();
            if(options && this.datePicker) {
                this.datePicker.setStartDate(options.startDate);
                this.datePicker.setEndDate(options.endDate);
            }
        }
    }
}
