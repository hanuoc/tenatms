import { Component, OnDestroy, AfterViewInit, EventEmitter, Input, Output, ViewChild } from '@angular/core';

@Component({
  selector: 'app-simple-tiny',
  templateUrl: './simple-tiny.component.html',
  styleUrls: ['./simple-tiny.component.css']
})
export class SimpleTinyComponent implements  AfterViewInit, OnDestroy {

 @Input() elementId: String;
  @Output() onEditorKeyup = new EventEmitter<any>();
  @Input() content: string;
  editor;

  ngAfterViewInit() {
    tinymce.baseURL = "/assets/tinymce";
    tinymce.init({
      max_chars : 2000,
      theme: 'modern',
      selector: '#' + this.elementId,
      height: 250,
      skin_url: '/assets/tinymce/skins/lightgray',
      plugins: "autosave autolink code colorpicker emoticons fullscreen hr image imagetools media preview table textcolor",
      
      setup: editor => {
        this.editor = editor;
        editor.on('keyup', () => {
          const content = editor.getContent();
          this.onEditorKeyup.emit(content);
        });
        var allowedKeys = [8, 37, 38, 39, 40, 46];
        editor.on('keydown', (e) => {
          if (allowedKeys.indexOf(e.keyCode) != -1) return true;
          if ($(editor.getBody()).text().length + 1 > editor.settings.max_chars) {
            e.preventDefault();
            e.stopPropagation();
            return false;
        }
        return true;
        });
        
      },
    });
  }

  ngOnDestroy() {
    tinymce.remove(this.editor);
  }
}
