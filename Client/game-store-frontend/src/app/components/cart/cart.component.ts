import { Component, OnInit } from '@angular/core';
import {IGame} from "../../models/game.model";

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  gamesToBuy: IGame[] = [];
  constructor() { }

  ngOnInit(): void {
  }

  getTotalAmountOfOrder(): number {
    return this.gamesToBuy.reduce((a, b) => {
      return a + b.price
    }, 0);
  }
}
