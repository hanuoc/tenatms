
    import {Pipe, PipeTransform} from '@angular/core';
 
    @Pipe({name: 'user'})
    export class UserPipe implements PipeTransform {
      transform(users: any, searchText: any): any {
         if(searchText == null) return users;

        return users.filter(function(user){
          return user.CategoryName.toLowerCase().indexOf(searchText) > -1;
        })
      }
    }