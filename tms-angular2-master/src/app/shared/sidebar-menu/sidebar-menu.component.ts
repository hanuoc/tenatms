import { Component, OnInit, ElementRef } from '@angular/core';
import { DataService } from './../../core/services/data.service';
import { LoggedInUser } from 'app/core/domain/loggedin.user';
import { SystemConstants } from 'app/core/common/system.constants';

@Component({
  selector: 'app-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.css']
})
export class SidebarMenuComponent implements OnInit {
  public functions: any[];
  user: LoggedInUser;
  // whether user have group or not
  isGroup:boolean = true;
  constructor(private dataService: DataService, private elementRef: ElementRef) { }

  ngOnInit() {
    this.user = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER));
    this.isGroup = this.user.groupId.length != 0;
    this.dataService.get('/api/function/getlisthierarchy').subscribe((response: any[]) => {
      this.functions = response.sort((n1, n2) => {
        if (n1.DisplayOrder > n2.DisplayOrder)
          return 1;
        else if (n1.DisplayOrder < n2.DisplayOrder)
          return -1;
        return 0;
      });
    }, error => this.dataService.handleError(error));
  }
  sideBarClicked(event: any) {
    $('#sidebar-menu').find('a.no-child').off('click').on('click', function (ev) {
      var $li = $(this).parent();
      $li.siblings().removeClass('active active-sm').children('ul').stop().slideUp();
    });
    var check = event.target.localName;
    var target;
    if (check == 'a') {
      target =  event.target.parentNode;
    }
    if (check == 'i' || check == 'span') {
      target =  event.target.parentNode.parentNode;
    }
    var $li = $(target);

    if ($li.is('.active') || $li.is('.active-sm')) {
      $li.removeClass('active active-sm');
      $('ul:first', $li).slideUp(function () {
      });
    } else {
      // prevent closing menu if we are on child menu
      if (!$li.parent().is('.child_menu')) {
        $('#sidebar-menu').find('li').removeClass('active active-sm');
        $('#sidebar-menu').find('li ul').slideUp();
      }
      if ($('body').hasClass('nav-md')) {
        $li.addClass('active');
      }
      else {
        $li.addClass('active-sm');
      }

      $('ul:first', $li).slideDown(function () {
      });
    }
    event.stopPropagation();

  }
  sideBarDetailClicked(event: any) {
    var target = event.target.parentElement;
    var $ul = $(target);
    $('#sidebar-menu').find('a').parent('li').removeClass('current-page')
    $ul.addClass("current-page");
    if ($('body').hasClass('nav-sm')) {
      $ul.parent().slideToggle();
    }
  }
}
