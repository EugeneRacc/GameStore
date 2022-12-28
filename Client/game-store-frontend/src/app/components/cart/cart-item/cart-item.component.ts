import {Component, Input, OnInit} from '@angular/core';
import {IGameInOrderModel} from "../../../models/game-in-order.model";
import {CartService} from "../../../services/cart.service";
import {ImageService} from "../../../services/image.service";
import {IImage} from "../../../models/image.model";
import {SafeUrl} from "@angular/platform-browser";

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  styleUrls: ['./cart-item.component.css']
})
export class CartItemComponent implements OnInit {

  @Input() orderItem: IGameInOrderModel = {
      game: {
        id: "",
        imageIds: [],
        genreIds: [],
        price: 300,
        description: "",
        title: "Test Title"
      },
    amount: 0,
    }
  gameImages: IImage[];
  urlS: SafeUrl;
  constructor(private cartService: CartService, private imageService: ImageService) { }

  ngOnInit(): void {
    this.imageService.getImagesByGameId(this.orderItem.game.id)
      .subscribe(images => {
        this.gameImages = images;
        this.urlS = this.imageService.sanitizeImage(images);
      })
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
