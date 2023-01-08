import {Component, Input, OnInit} from '@angular/core';
import {IUserModel} from "../../../../models/user.model";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  @Input() user: IUserModel = {
    id: "",
    userName: "",
    firstName: "",
    lastName: "",
    email: "",
    roleNames: []
  }
  isChangeWindowExists = false;
  constructor() { }

  ngOnInit(): void {
  }

  onChangeRole() {
    this.isChangeWindowExists = true;
  }
}
