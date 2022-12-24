import {Component, Input, OnInit} from '@angular/core';
import {IGame} from "../../../models/game.model";

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  styleUrls: ['./cart-item.component.css']
})
export class CartItemComponent implements OnInit {

  @Input() game: IGame = {
    id: "",
    imageIds: [],
    genreIds: [],
    price: 300,
    description: "",
    title: "Test Title"
  }

  testAmount = 2;
  testTotal = 800;
  constructor() { }

  ngOnInit(): void {
  }

}
