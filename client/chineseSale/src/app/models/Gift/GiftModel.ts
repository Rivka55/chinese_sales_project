export class GiftModel {
    id!: number;
    name!: string;
    description!: string;
    picture!: string;
    price!: number;
    donorName!: string;
    categoryName!: string;
    winnerName?: string;
    winnerEmail?: string;
}