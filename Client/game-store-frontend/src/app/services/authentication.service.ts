import {EventEmitter, Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {IRegistrationModel} from "../models/registration.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {IErrorModel} from "../models/error.model";
import {ILoginModel} from "../models/login.model";
import {ITokenModel} from "../models/token.model";
import {IUserModel} from "../models/user.model";
import {IRefreshTokenModel} from "../models/refresh.model";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  baseURL = 'https://localhost:7043/api/';
  loginOpened = new EventEmitter<boolean>();
  isAuth = new BehaviorSubject<boolean>(false);
  currentUser = new BehaviorSubject<IUserModel | null>(null);

  constructor(private http: HttpClient) { this.loginOpened.emit(false); }

  registerUser(registerModel: IRegistrationModel): Observable<string | IErrorModel> {
    return this.http.post<string | IErrorModel>(`https://localhost:7043/api/authentication/register`, registerModel);
  }

  login(loginModel: ILoginModel): Observable<ITokenModel> {
    return this.http.post<ITokenModel>(`https://localhost:7043/api/authentication/login`, loginModel);
  }

  getUserDetails(token:string): Observable<IUserModel> {
    let tokenHeader = new HttpHeaders({'Authorization':'Bearer ' + token});
    return this.http.get<IUserModel>(this.baseURL + "user", {headers: tokenHeader});
  }

  authorizeByRefresh(tokens: IRefreshTokenModel) : Observable<ITokenModel>{
    return this.http.post<ITokenModel>(this.baseURL + "authentication/refresh", tokens);
  }

  setNewCurrentUser(user: IUserModel | null) {
    this.currentUser.next(user);
  }

}
