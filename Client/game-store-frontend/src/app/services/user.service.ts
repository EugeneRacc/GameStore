import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {IUserModel} from "../models/user.model";

@Injectable({
  providedIn: 'root'
})
export class UserService{

  baseURL = 'https://localhost:7043/api/';
  constructor(private http: HttpClient) {  }

  getUserById(userId: string): Observable<IUserModel> {
    return this.http.get<IUserModel>(this.baseURL + `user/${userId}`);
  }

}
