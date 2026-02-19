import { GiftModel } from "../Gift/GiftModel";

export class DonorModel {
  id!: number;
  identityNumber!: string;
  name!: string;
  phone!: string;
  email!: string;
  gifts!: GiftModel[];
}