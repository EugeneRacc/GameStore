import { Injectable } from '@angular/core';
import {Observable} from "rxjs";
import {IRegistrationModel} from "../models/registration.model";
import {HttpClient} from "@angular/common/http";
import {IErrorModel} from "../models/error.model";

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  registerUser(registerModel: IRegistrationModel): Observable<string | IErrorModel> {
    return this.http.post<string | IErrorModel>(`https://localhost:7043/api/authentication/register`, registerModel);
  }
}
