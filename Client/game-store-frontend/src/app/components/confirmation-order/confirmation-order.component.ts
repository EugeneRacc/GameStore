import { Component, OnInit } from '@angular/core';
import {IOrderModel} from "../../models/order.model";
import {PaymentType} from "../../models/payment-type.enum";
import {CartService} from "../../services/cart.service";
import {AuthenticationService} from "../../services/authentication.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-confirmation-order',
  templateUrl: './confirmation-order.component.html',
  styleUrls: ['./confirmation-order.component.css']
})
export class ConfirmationOrderComponent implements OnInit {

  order: IOrderModel = {
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    userId: "",
    paymentType: PaymentType.cash,
    orderDate: new Date(),
    comment: "",
    orderedGames: []
  }
  constructor(private cartService: CartService, private authService: AuthenticationService,
              private router: Router) { }

  ngOnInit(): void {
    this.cartService.gamesInOrderS
      .subscribe(games => {
        for (const game of games) {
          this.order.orderedGames.push({gameId: game.game.id, amount: game.amount})
        }
      });
  }

  onSubmit() {
    this.order.orderDate = new Date();
    if (this.authService.isAuth) {
      this.authService.currentUser
        .subscribe(user => {
          if (user) {
            this.order.userId = user.id
          }
        });
    }
    if (this.order.orderedGames.length <=0) {
      alert("Cart can't be empty");
      this.router.navigateByUrl('main-page');
    }
    else {
      this.cartService.sendOrder(this.order)
        .subscribe(() => {
            alert('We got your order. Continue your shopping');
            this.router.navigateByUrl('main-page');
          },
          error => console.log(error))
    }
  }
}
