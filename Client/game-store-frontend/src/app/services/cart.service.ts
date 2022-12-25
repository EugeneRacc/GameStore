import { Injectable } from '@angular/core';
import {IGame} from "../models/game.model";
import {IGameInOrderModel} from "../models/game-in-order.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {IOrderModel} from "../models/order.model";

@Injectable({
  providedIn: 'root'
})
export class CartService {
  gamesInOrder: IGameInOrderModel[] = [];
  gamesInOrderS = new BehaviorSubject<IGameInOrderModel[]>([]);
  constructor(private http: HttpClient) {
    this.gamesInOrderS
      .subscribe(order => this.gamesInOrder = order);
  }

  sendOrder(orderModel: IOrderModel): Observable<IOrderModel> {
    return this.http.post<IOrderModel>('https://localhost:7043/api/order', orderModel);
  }

  addGameToOrder(game: IGame) {
    let indexOfGame = this.gamesInOrder.findIndex(x => x.game.id === game.id);
    if (indexOfGame > -1) {
      this.gamesInOrder[indexOfGame].amount += 1;
      this.gamesInOrderS.next(this.gamesInOrder);
    }
    else {
      this.gamesInOrder.push({game: game, amount: 1} as IGameInOrderModel)
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
