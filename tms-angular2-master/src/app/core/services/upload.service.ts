
import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { UrlConstants } from '../../core/common/url.constants';
import { UtilityService } from './utility.service';

@Injectable()
export class UploadService {
  public responseData: any;
  
  constructor(private dataService: DataService, private utilityService: UtilityService) { }

  postWithFile(url: string, postData: any, files: File[]) {
    let formData: FormData = new FormData();
    if(files.length > 1){
      for(let items of files)
        formData.append(items.name, items, items.name); //Update miulti file edit by nvthang
    }
    else
      formData.append('files', files[0], files[0].name); //Only Update a file edit by nvthang
    if (postData !== "" && postData !== undefined && postData !== null) {
      for (var property in postData) {
        if (postData.hasOwnProperty(property)) {
          formData.append(property, postData[property]);
        }
      }
    }
    var returnReponse = new Promise((resolve, reject) => {
      this.dataService.postFile(url, formData).subscribe(
        res => {
          this.responseData = res;
          resolve(this.responseData);
        },
        error => this.dataService.handleError(error)
      );
    });
    return returnReponse;
  }
}
