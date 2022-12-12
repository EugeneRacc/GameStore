import {Component, ElementRef, OnInit} from '@angular/core';
import {ILoginModel} from "../../../models/login.model";
import {AuthenticationService} from "../../../services/authentication.service";
import {ActivatedRoute, Router} from "@angular/router";
import {ITokenModel} from "../../../models/token.model";
import {IErrorModel} from "../../../models/error.model";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  errMessage = "";
  loginModel: ILoginModel = {
    email: "",
    password: ""
  }

  checkbox: boolean;

  constructor(private authService: AuthenticationService, private router: Router,
              private elRef: ElementRef, private activeRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
  }

  onLogin() {
    this.authService.login(this.loginModel)
      .subscribe(
        (token: ITokenModel) => {
          this.authService.isAuth.next(true);
          localStorage.setItem('token', token.token);
          if (this.checkbox) {
            localStorage.setItem('refresh', token.refreshToken);
          }
          this.authService.getUserDetails(token.token)
            .subscribe(user => {
              this.authService.setNewCurrentUser(user);
            }, error => {
              console.error(error);
            })
          this.router.navigateByUrl('/main-page');
        },
        (err) => {
          if(err.status == 401) {
            this.errMessage = (err.error as IErrorModel).Message;
          }
          else {
            this.errMessage = "Try again later";
          }
        }
      )
  }

  onCloseLogin() {
    this.router.navigate(['..'], {relativeTo: this.activeRoute});
  }
}
