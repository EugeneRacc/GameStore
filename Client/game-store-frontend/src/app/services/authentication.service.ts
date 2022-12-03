import {EventEmitter, Injectable} from '@angular/core';
import {Observable} from "rxjs";
import {IRegistrationModel} from "../models/registration.model";
import {HttpClient} from "@angular/common/http";
import {IErrorModel} from "../models/error.model";
import {ILoginModel} from "../models/login.model";
import {ITokenModel} from "../models/token.model";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  loginOpened = new EventEmitter<boolean>();

  constructor(private http: HttpClient) { this.loginOpened.emit(false); }

  registerUser(registerModel: IRegistrationModel): Observable<string | IErrorModel> {
    return this.http.post<string | IErrorModel>(`https://localhost:7043/api/authentication/register`, registerModel);
  }

  login(loginModel: ILoginModel): Observable<ITokenModel> {
    return this.http.post<ITokenModel>(`https://localhost:7043/api/authentication/login`, loginModel);
  }
}
