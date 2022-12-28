import {IGame} from "./game.model";

export interface IGameInOrderModel {
  game: IGame,
  amount: number,
  orderedDetailsId?: string
}
