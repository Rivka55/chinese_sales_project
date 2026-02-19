import { Component, EventEmitter, inject, Input, Output, SimpleChanges, OnInit, OnChanges } from '@angular/core';
import { GiftService } from '../../../services/Gift/gift-service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { UniqGiftName } from '../../../shared/uniq-gift-name.validator';
import { GiftModel } from '../../../models/Gift/GiftModel';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../../services/Category/category-service';
import { DonorService } from '../../../services/Donor/donor-service';
import { forkJoin } from 'rxjs';
import { DonorModel } from '../../../models/Donor/DonorModel';
import { CategoryModel } from '../../../models/Category/CategoryModel';

@Component({
  selector: 'app-gift-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './gift-edit.html',
  styleUrl: './gift-edit.scss',
})
export class GiftEdit implements OnInit, OnChanges {
  private giftSrv = inject(GiftService);
  private donorSrv = inject(DonorService);
  private categorySrv = inject(CategoryService);

  gifts: GiftModel[] = [];
  donors: DonorModel[] = [];
  categories: CategoryModel[] = [];

  private _id: number = -1;

  @Input() set id(value: number) {
    this._id = value;
  }
  get id(): number {
    return this._id;
  }

  @Output() idChange = new EventEmitter<number>();

  frmGift: FormGroup = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    description: new FormControl('', [Validators.required, Validators.maxLength(500)]),
    picture: new FormControl('', [Validators.required, Validators.maxLength(250)]),
    price: new FormControl(10, [Validators.required, Validators.min(5), Validators.max(500)]),
    donor: new FormControl(null, [Validators.required]),
    category: new FormControl(null, [Validators.required]),
  });

  ngOnInit() {
    this.loadInitialData();
  }

  ngOnChanges(c: SimpleChanges) {
    if (c['id']) {
      this.handleIdChange();
    }
  }

  private handleIdChange() {
    if (this.frmGift) {
      this.frmGift.reset({ price: 10 });
    }
    if (this.id > 0) {
      this.fetchGiftForEdit();
    }
  }

  private loadInitialData() {
    forkJoin({
      donors: this.donorSrv.getAll(),
      categories: this.categorySrv.getAll(),
      gifts: this.giftSrv.getAll()
    }).subscribe(res => {
      this.donors = res.donors;
      this.categories = res.categories;
      this.gifts = res.gifts;

      this.updateNameValidator();

      if (this.id > 0) this.fetchGiftForEdit();
      else this.frmGift.reset({ price: 10 });
    });
  }

  private updateNameValidator() {
    this.frmGift.get('name')?.setValidators([
      Validators.required,
      Validators.maxLength(50),
      UniqGiftName(this.gifts, this.id)
    ]);
    this.frmGift.get('name')?.updateValueAndValidity();
  }

  private fetchGiftForEdit() {
    this.giftSrv.getById(this.id).subscribe(data => {
      if (data) {
        const donor = this.donors.find(d => d.name === data.donorName);
        const category = this.categories.find(c => c.name === data.categoryName);

        this.updateNameValidator();

        this.frmGift.patchValue({
          name: data.name,
          description: data.description,
          picture: data.picture,
          price: data.price,
          donor: donor,
          category: category
        }, { emitEvent: false });
      }
    });
  }

  saveGift() {
    if (this.frmGift.invalid) return;

    const val = this.frmGift.getRawValue();
    const giftData = {
      ...val,
      donorId: val.donor?.id,
      categoryId: val.category?.id
    };

    const action = this.id === 0 ? this.giftSrv.add(giftData) : this.giftSrv.update(this.id, giftData);

    action.subscribe({
      next: () => this.idChange.emit(-2),
      error: (err) => console.error('שגיאה בשמירה', err)
    });
  }

  cancelChanges() {
    this.idChange.emit(-2);
  }

  deleteGift(id: number) {
    this.giftSrv.delete(id).subscribe(() => this.idChange.emit(-2));
  }

  handleImageError(event: any) {
    event.target.src = 'https://via.placeholder.com/150?text=No+Image'; 
 }
}











































// import { Component, EventEmitter, inject, Input, Output, SimpleChanges, OnInit } from '@angular/core';
// import { GiftService } from '../../../services/Gift/gift-service';
// import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
// import { UniqGiftName } from '../../../shared/uniq-gift-name.validator';
// import { GiftModel } from '../../../models/Gift/GiftModel';
// import { CommonModule } from '@angular/common';
// import { CategoryService } from '../../../services/Category/category-service';
// import { DonorService } from '../../../services/Donor/donor-service';
// import { forkJoin } from 'rxjs';
// import { DonorModel } from '../../../models/Donor/DonorModel';
// import { CategoryModel } from '../../../models/Category/CategoryModel';

// @Component({
//   selector: 'app-gift-edit',
//   standalone: true,
//   imports: [CommonModule, FormsModule, ReactiveFormsModule],
//   templateUrl: './gift-edit.html',
//   styleUrl: './gift-edit.scss',
// })
// export class GiftEdit implements OnInit {
//   private giftSrv = inject(GiftService);
//   private donorSrv = inject(DonorService);
//   private categorySrv = inject(CategoryService);

//   //gifts: GiftModel[] = [];
//   donors: DonorModel[] = [];
//   categories: CategoryModel[] = [];

//   //@Input() id: number = -1;
//   @Output() idChange = new EventEmitter<number>();

//   frmGift: FormGroup = new FormGroup({
//     name: new FormControl('', [Validators.required, Validators.maxLength(50)]),
//     description: new FormControl('', [Validators.required, Validators.maxLength(500)]),
//     picture: new FormControl('', [Validators.required, Validators.maxLength(250)]),
//     price: new FormControl(10, [Validators.required, Validators.min(5), Validators.max(500)]),
//     donor: new FormControl(null, [Validators.required]),
//     category: new FormControl(null, [Validators.required]),
//   });

//   @Input() set id(value: number) {
//     this._id = value;
//     this.handleIdChange(); // בכל פעם שה-ID משתנה, נפעיל לוגיקת רענון
//   }
//   get id(): number {
//     return this._id;
//   }
//   private _id: number = -1;

//   // פונקציה שמטפלת בהחלפת ה-ID
//   private handleIdChange() {
//     if (this.frmGift) {
//       this.frmGift.reset({ price: 10 }); // איפוס הטופס כדי למנוע ערבוב נתונים
//     }

//     // טעינה מחדש של הנתונים הספציפיים למתנה החדשה
//     if (this._id > 0) {
//       this.fetchGiftForEdit();
//     } else if (this._id === 0) {
//       this.frmGift?.reset({ price: 10 });
//     }
//   }

//   ngOnInit() {
//     this.loadInitialData();
//   }

//   ngOnChanges(c: SimpleChanges) {
//     if (c['id'] && !c['id'].firstChange) {
//       this.handleIdChange();
//     }
//   }

//   private loadInitialData() {
//     forkJoin({
//       donors: this.donorSrv.getAll(),
//       categories: this.categorySrv.getAll(),
//       gifts: this.giftSrv.getAll()
//     }).subscribe(res => {
//       this.donors = res.donors;
//       this.categories = res.categories;

//       this.frmGift.get('name')?.setValidators([
//         Validators.required,
//         Validators.maxLength(50),
//         UniqGiftName(res.gifts, this.id)
//       ]);

//       if (this.id > 0)
//         this.fetchGiftForEdit();
//       else
//         this.frmGift.reset({ price: 10 });
//     });
//   }

//   // private resetAndLoad() {
//   //   if (this.id > 0)
//   //      this.fetchGiftForEdit();
//   //   else 
//   //     this.frmGift.reset({ price: 10 });
//   // }

//   private fetchGiftForEdit() {
//   this.giftSrv.getById(this.id).subscribe(data => {
//     if (data) {
//       const donor = this.donors.find(d => d.name === data.donorName);
//       const category = this.categories.find(c => c.name === data.categoryName);

//       // עדכון ה-Validator עם ה-ID הנוכחי כדי שלא יגיד שהשם "קיים" בגלל המתנה של עצמה
//       this.frmGift.get('name')?.setValidators([
//         Validators.required, 
//         Validators.maxLength(50), 
//         UniqGiftName(this.gifts, this.id)
//       ]);

//       this.frmGift.patchValue({
//         name: data.name,
//         description: data.description,
//         picture: data.picture,
//         price: data.price,
//         donor: donor,
//         category: category
//       }, { emitEvent: false }); // מונע הרצת Validation מיותרת בזמן המילוי
      
//       this.frmGift.get('name')?.updateValueAndValidity();
//     }
//   });
// }

//   // private fetchGiftForEdit() {
//   //   this.giftSrv.getById(this.id).subscribe(data => {
//   //     if (data) {
//   //       const donor = this.donors.find(d => d.name === data.donorName);
//   //       const category = this.categories.find(c => c.name === data.categoryName);

//   //       this.frmGift.patchValue({
//   //         name: data.name,
//   //         description: data.description,
//   //         picture: data.picture,
//   //         price: data.price,
//   //         donor: donor,
//   //         category: category
//   //       });
//   //     }
//   //   });
//   // }

//   saveGift() {
//     if (this.frmGift.invalid)
//       return;

//     const val = this.frmGift.getRawValue();
//     const giftData = {
//       ...val,
//       donorId: val.donor?.id,
//       categoryId: val.category?.id
//     };

//     const action = this.id === 0 ? this.giftSrv.add(giftData) : this.giftSrv.update(this.id, giftData);

//     action.subscribe({
//       next: () => {
//         this.idChange.emit(-2);
//       },
//       error: (err) => console.error('שגיאה בשמירה', err)
//     });
//   }

//   cancelChanges() {
//     this.idChange.emit(-2);
//   }

//   deleteGift(id: number) {
//     if (confirm('?האם אתה בטוח שברצונך למחוק מתנה זו')) {
//       this.giftSrv.delete(id).subscribe(() => this.idChange.emit(-2));
//     }
//   }
// }