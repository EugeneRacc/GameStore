import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {AuthenticationService} from "../../services/authentication.service";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  isUserAuthenticated = false;
  currentUserName = "";
  constructor(private router: Router, private authService: AuthenticationService) { }

  ngOnInit(): void {
    this.authService.isAuth
      .subscribe(auth => this.isUserAuthenticated = auth);
    this.authService.currentUser
      .subscribe(user => {
        if(user != null) {
          this.currentUserName = user.firstName + " " + user.lastName;
        }
      })
  }

  onNavigateToMain() {
    this.router.navigate(['/']);
  }

  onLogout() {
    localStorage.removeItem('token');
    localStorage.removeItem('refresh');
    this.authService.isAuth.next(false);
    this.authService.setNewCurrentUser(null);
    this.router.navigateByUrl('main-page');
  }
}
