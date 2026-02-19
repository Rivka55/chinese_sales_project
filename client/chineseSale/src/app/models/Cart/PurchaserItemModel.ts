export class PurchaserItemModel {
  giftName!: string;
  quantity!: number;
  pricePerUnit!: number;
  totalPrice!: number;
  purchaseDate!: Date; // ב-TS משתמשים ב-Date עבור DateTime
}
