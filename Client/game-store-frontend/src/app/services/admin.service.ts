import {EventEmitter, Injectable} from '@angular/core';
import {BehaviorSubject, Observable} from "rxjs";
import {IUserModel} from "../models/user.model";
import {HttpClient} from "@angular/common/http";
import {ISearchingByName} from "../interfaces/iSearchingByName";
import {IRoleModel} from "../models/role.model";

@Injectable({
  providedIn: 'root'
})
export class AdminService implements ISearchingByName{

  baseURL = 'https://localhost:7043/api/';
  selectedNamesChanged = new EventEmitter<string>;
  allUsers = new BehaviorSubject<IUserModel[]>([]);
  constructor(private http: HttpClient) { this.getAllUsers(); }


  getAllUsers(userName?: string): Observable<IUserModel[]> {
    let response = this.http.get<IUserModel[]>(
      userName ? this.baseURL + `admin/users?name=${userName}` : this.baseURL + `admin/users`
    );
    response.subscribe(
      users => {
        this.allUsers.next(users);
      }
    )
    return response;
  }

  getAllExistRoles(): Observable<IRoleModel[]> {
    return this.http.get<IRoleModel[]>(this.baseURL + 'admin/roles');
  }

  changeUserRole(roleName: string, userId: string): Observable<any> {
    return  this.http.get(this.baseURL + 'admin/' + userId + '?roleName=' + roleName);
  }

  nameFilteringChanged(name: string): void {
    this.selectedNamesChanged.emit(name);
    this.getAllUsers(name);
  }
}
