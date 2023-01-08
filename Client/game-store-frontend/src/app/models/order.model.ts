import {PaymentType} from "./payment-type.enum";
import {IGameWithIdInOrderModel} from "./game-with-id-in-order.model";

export interface IOrderModel {
  id?: string,
  firstName: string,
  lastName: string,
  email: string,
  phone: string,
  paymentType: PaymentType,
  orderDate: Date,
  userId?: string,
  comment: string,
  orderedGames: IGameWithIdInOrderModel[]
}

