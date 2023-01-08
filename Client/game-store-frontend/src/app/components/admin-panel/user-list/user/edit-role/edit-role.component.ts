import {Component, Input, OnInit} from '@angular/core';
import {IUserModel} from "../../../../../models/user.model";
import {AdminService} from "../../../../../services/admin.service";

@Component({
  selector: 'app-edit-role',
  templateUrl: './edit-role.component.html',
  styleUrls: ['./edit-role.component.css']
})
export class EditRoleComponent implements OnInit {

  @Input() currentUser: IUserModel = {
    id: "",
    userName: "",
    firstName: "",
    lastName: "",
    email: "",
    roleNames: []
  };
  existedRoles: string[] = [];
  chosenRole: string;
  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.adminService.getAllExistRoles()
      .subscribe(roles => {
        roles.forEach(x => this.existedRoles.push(x.name));
      })
  }

  onSubmit() {
    this.adminService.changeUserRole(this.chosenRole, this.currentUser.id)
      .subscribe(() => {
        this.adminService.getAllUsers()
          .subscribe(users => {
            this.adminService.allUsers.next(users);
          })
      }, error => {
        console.error(error);
      });
  }
}
