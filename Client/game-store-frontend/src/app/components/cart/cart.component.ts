import { Component, OnInit } from '@angular/core';
import {IOrderModel} from "../../models/order.model";
import {CartService} from "../../services/cart.service";

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  gamesToBuy: IOrderModel[] = [];
  constructor(private cartService: CartService) { }

  ngOnInit(): void {
    this.cartService.gamesInOrderS
      .subscribe(order => this.gamesToBuy = order);
  }

  getTotalAmountOfOrder(): number {
    return this.gamesToBuy.reduce((a, b) => {
      return a + b.game.price * b.amount;
    }, 0);
  }
}
