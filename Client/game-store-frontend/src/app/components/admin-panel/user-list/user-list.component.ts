import { Component, OnInit } from '@angular/core';
import {IUserModel} from "../../../models/user.model";
import {AdminService} from "../../../services/admin.service";

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  allUsers: IUserModel[];
  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.adminService.allUsers.subscribe(users => this.allUsers = users);
  }

}
