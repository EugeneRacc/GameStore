import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-confirmation-order',
  templateUrl: './confirmation-order.component.html',
  styleUrls: ['./confirmation-order.component.css']
})
export class ConfirmationOrderComponent implements OnInit {

  order = {
    first: "",
    last: "",
    email: "",
    phone: "",
    payment: 0,
    comments: ""
  }
  constructor() { }

  ngOnInit(): void {
  }

}
