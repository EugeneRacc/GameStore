import {Component, Input, OnInit} from '@angular/core';
import {IOrderModel} from "../../../models/order.model";
import {CartService} from "../../../services/cart.service";

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  styleUrls: ['./cart-item.component.css']
})
export class CartItemComponent implements OnInit {

  @Input() orderItem: IOrderModel = {
      game: {
        id: "",
        imageIds: [],
        genreIds: [],
        price: 300,
        description: "",
        title: "Test Title"
      },
    amount: 0
    }

  constructor(private cartService: CartService) { }

  ngOnInit(): void {
  }

  getTotalPrice() {
    return this.orderItem.game.price * this.orderItem.amount;
  }

  onAddOne() {
    this.cartService.addGameToOrder(this.orderItem.game);
  }

  onRemoveOne() {
    this.cartService.setLowerAmountOfGame(this.orderItem.game);
  }

  onDeleteGameFromCart() {
    this.cartService.deleteGameFromOrder(this.orderItem.game);
  }
}
