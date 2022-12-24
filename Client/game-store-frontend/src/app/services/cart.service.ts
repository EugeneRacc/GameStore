import { Injectable } from '@angular/core';
import {IGame} from "../models/game.model";
import {IOrderModel} from "../models/order.model";
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CartService {
  gamesInOrder: IOrderModel[] = [];
  gamesInOrderS = new BehaviorSubject<IOrderModel[]>([]);
  constructor() {
    this.gamesInOrderS
      .subscribe(order => this.gamesInOrder = order);
  }

  addGameToOrder(game: IGame) {
    let indexOfGame = this.gamesInOrder.findIndex(x => x.game.id === game.id);
    if (indexOfGame > -1) {
      this.gamesInOrder[indexOfGame].amount += 1;
      this.gamesInOrderS.next(this.gamesInOrder);
    }
    else {
      this.gamesInOrder.push({game: game, amount: 1} as IOrderModel)
      this.gamesInOrderS.next(this.gamesInOrder);
    }
    console.log(this.gamesInOrder);
  }

  setLowerAmountOfGame(game: IGame) {
    let indexOfGame = this.gamesInOrder.findIndex(x => x.game.id === game.id);
    if (indexOfGame > -1) {
      this.gamesInOrder[indexOfGame].amount -= 1;
      if (this.gamesInOrder[indexOfGame].amount <= 0) {
        this.gamesInOrder.splice(indexOfGame, 1);
        this.gamesInOrderS.next(this.gamesInOrder);
      }
      this.gamesInOrderS.next(this.gamesInOrder);
    }
    else {
      console.error('Id not found');
    }
  }

  deleteGameFromOrder(game: IGame) {
    let indexOfGame = this.gamesInOrder.findIndex(x => x.game.id === game.id);
    if (indexOfGame > -1) {
      this.gamesInOrder.splice(indexOfGame, 1);
      this.gamesInOrderS.next(this.gamesInOrder);
    }
    else {
      console.error('Id not found');
    }
  }
}
