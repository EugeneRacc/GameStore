import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {
  checkLoginWindow: boolean = false;
  constructor(private router: Router, private authService: AuthenticationService) { }

  ngOnInit(): void {
    this.authService.loginOpened
      .subscribe((popupOpened) => {
        this.checkLoginWindow = popupOpened;
      })
  }
}
