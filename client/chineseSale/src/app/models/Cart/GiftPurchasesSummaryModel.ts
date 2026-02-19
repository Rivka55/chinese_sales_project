import { GiftPurchaseModel } from './GiftPurchaseModel';

export class GiftPurchasesSummaryModel {
  giftId!: number;
  giftName!: string;
  purchasers!: GiftPurchaseModel[];
  totalTicketsPurchased!: number;
  totalEarned!: number;
}
