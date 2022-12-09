import { Component, OnInit } from '@angular/core';
import {IRegistrationModel} from "../../../models/registration.model";
import {AuthenticationService} from "../../../services/authentication.service";
import {IErrorModel} from "../../../models/error.model";
import {ActivatedRoute, Router} from "@angular/router";


@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registrationModel: IRegistrationModel = {
    firstName: "",
    lastName: "",
    email: "",
    userName: "",
    password: "",
    role: "user"
  }
  message: string = ``;

  constructor(private authService: AuthenticationService, private router: Router,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  onCreateUser() {
    this.authService.registerUser(this.registrationModel)
      .subscribe(
        () => {
          this.router.navigate(['..', 'main-page', 'login'], {relativeTo: this.route});
        },
        err => {
          this.message = (err.error as IErrorModel).Message;
        }
      );

  }
}
