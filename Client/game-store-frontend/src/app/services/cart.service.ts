import { Injectable } from '@angular/core';
import {IGame} from "../models/game.model";
import {IOrderModel} from "../models/order.model";

@Injectable({
  providedIn: 'root'
})
export class CartService {
  gamesInOrder: IOrderModel[] = [];
  constructor() { }

  addGameToOrder(game: IGame) {
    let indexOfGame = this.gamesInOrder.findIndex(x => x.game.id === game.id);
    if (indexOfGame > -1) {
      this.gamesInOrder[indexOfGame].amount += 1;
    }
    else {
      this.gamesInOrder.push({game: game, amount: 0} as IOrderModel)
    }
  }

  setLowerAmountOfGame(game: IGame) {
    let indexOfGame = this.gamesInOrder.findIndex(x => x.game.id === game.id);
    if (indexOfGame > -1) {
      this.gamesInOrder[indexOfGame].amount -= 1;
    }
    else {
      console.error('Id not found');
    }
  }

  deleteGameFromOrder(game: IGame) {
    let indexOfGame = this.gamesInOrder.findIndex(x => x.game.id === game.id);
    if (indexOfGame > -1) {
      this.gamesInOrder.splice(indexOfGame, 1);
    }
    else {
      console.error('Id not found');
    }
  }
}
