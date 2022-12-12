import {Component, HostListener, OnInit} from '@angular/core';
import {AuthenticationService} from "./services/authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  @HostListener('window:unload', ['$event']) beforeUnloadHandler() {
    if (!localStorage.getItem("refresh")) {
      localStorage.removeItem('token');
    }
  }

  constructor(private authService: AuthenticationService) {
  }
  ngOnInit(): void {
    this.checkIfUserAuthorized();
  }

  checkIfUserAuthorized() {
    let refresh = localStorage.getItem('refresh');
    let token = localStorage.getItem('token');
    if (refresh && token){
      this.authService.authorizeByRefresh({ token: token, refreshToken: refresh })
        .subscribe(token => {
          localStorage.setItem('token', token.token);
          this.authService.isAuth.next(true);
          this.authService.getUserDetails(token.token)
            .subscribe(user => this.authService.setNewCurrentUser(user))
        })
    }
  }
}
