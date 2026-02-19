import { PurchaserItemModel } from './PurchaserItemModel';

export class PurchaserDetailsModel {
  userId!: number;
  name!: string;
  email!: string;
  phone!: string;
  totalTicketsPurchased!: number;
  grandTotalSpent!: number;
  
  // כאן אנחנו מחברים את הרשימה מהמודל הקודם
  purchaseHistory!: PurchaserItemModel[];
}