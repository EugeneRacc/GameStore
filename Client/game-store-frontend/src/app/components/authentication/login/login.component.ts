import {Component, ElementRef, HostListener, OnInit} from '@angular/core';
import {ILoginModel} from "../../../models/login.model";
import {AuthenticationService} from "../../../services/authentication.service";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginModel: ILoginModel = {
    email: "",
    password: ""
  }

  @HostListener('document:click', ['$event']) toggleOpen(event: Event) {
    /*if (this.elRef.nativeElement.class) {
      this.router.navigate(['..'], {relativeTo: this.activeRoute});
    }*/
    console.log(this.elRef.nativeElement);
    console.log(event.target)
  }

  checkbox: boolean;

  constructor(private authService: AuthenticationService, private router: Router,
              private elRef: ElementRef, private activeRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
  }

  onLogin() {
    this.authService.login(this.loginModel)
      .subscribe((next) => console.log(next))
  }

}
