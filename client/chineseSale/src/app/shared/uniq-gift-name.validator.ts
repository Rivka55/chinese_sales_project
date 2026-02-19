import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { GiftModel } from "../models/Gift/GiftModel";

export function UniqGiftName(giftList: GiftModel[], id: number): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null  =>{
        let controlGiftName = control.value;

        if(!control.value)
            return null;

        var existName = giftList.find(n => n.name == controlGiftName && n.id != id);

        return (existName) ? {existName: 'שם מתנה כבר קיים!'} :null;
    }
}  