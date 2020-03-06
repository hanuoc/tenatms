import { Injectable } from '@angular/core';
import { CommonConstants } from 'app/core/common/common.constants';
import { MessageConstants } from 'app/core/common/message.constants';
declare var alertify: any;
@Injectable()
export class NotificationService {
  private _notifier: any = alertify;
  constructor() {
    alertify.defaults = {
      // dialogs defaults
      autoReset: true,
      basic: false,
      closable: true,
      closableByDimmer: true,
      frameless: false,
      maintainFocus: true, // <== global default not per instance, applies to all dialogs
      maximizable: true,
      modal: true,
      movable: true,
      moveBounded: false,
      overflow: true,
      padding: true,
      pinnable: true,
      pinned: true,
      preventBodyShift: false, // <== global default not per instance, applies to all dialogs
      resizable: true,
      startMaximized: false,
      transition: 'zoom',
      

      // notifier defaults
      notifier: {
        // auto-dismiss wait time (in seconds)  
        delay: 2,
        // default position
        position: 'top-right',
        // adds a close button to notifier messages
        closeButton: false
      },

      // language resources 
      glossary: {
        // dialogs default title
        title: 'Confirm',
        // ok button text
        ok: 'OK',
        // cancel button text
        cancel: 'Close'
      },

      // theme settings
      theme: {
        // class name attached to prompt dialog input textbox.
        input: 'ajs-input',
        // class name attached to ok button
        ok: 'ajs-ok',
        // class name attached to cancel button 
        cancel: 'ajs-cancel'
      },
    };
  }

  printSuccessMessage(message: string) {
    this._notifier.success(message).dismissOthers();
  }

  printErrorMessage(message: string) {
    this._notifier.error(message).dismissOthers();
  }

  printConfirmationDialog(message: string, okCallback: () => any) {
    this._notifier.confirm(message, function (e) {
      if (e) {
        okCallback();
      } else {
      }
    });
  }
  printPromptDialog(message: string, defaultValue:string, okCallback: (value:string) => any){
    this._notifier.prompt(message,defaultValue, function(e,value){
      var reasonReject = $('#reasonReject').val()
      if(reasonReject.trim() == ''){
        e.cancel = true;
      }
      okCallback(reasonReject);
      
    },function(){
      return;
    }).setContent('<input type="text" class="ajs-input" id="reasonReject" placeholder="Enter a reason to reject...."  maxlength="100" required></input').show();
  }
}
