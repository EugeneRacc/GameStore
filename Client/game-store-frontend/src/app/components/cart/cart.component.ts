import { Component, OnInit } from '@angular/core';
import {IGameInOrderModel} from "../../models/game-in-order.model";
import {CartService} from "../../services/cart.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  gamesToBuy: IGameInOrderModel[] = [];
  isProceedDisabled = true;
  constructor(private cartService: CartService, private router: Router) { }

  ngOnInit(): void {
    this.cartService.gamesInOrderS
      .subscribe(order => {
        order.length > 0 ? this.isProceedDisabled = true : this.isProceedDisabled = false;
        this.gamesToBuy = order;
      });
  }

  getTotalAmountOfOrder(): number {
    return this.gamesToBuy.reduce((a, b) => {
      return a + b.game.price * b.amount;
    }, 0);
  }

  onProceedOrder() {
      this.router.navigateByUrl("/confirmation");
  }
}
