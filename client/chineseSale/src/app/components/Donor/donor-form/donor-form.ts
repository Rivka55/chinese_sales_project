import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DonorService } from '../../../services/Donor/donor-service';
import { DonorModel } from '../../../models/Donor/DonorModel';

@Component({
  selector: 'app-donor-form',
  imports: [ReactiveFormsModule],
  templateUrl: './donor-form.html',
  styleUrls: ['./donor-form.scss'],
})
export class DonorForm {
  donorForm!: FormGroup;
  @Input() donorToEdit?: DonorModel; // מקבל נתונים לעריכה
  @Output() close = new EventEmitter<void>(); // מודיע לרשימה לסגור ולרענן

  constructor(private fb: FormBuilder, private donorService: DonorService) { }

  ngOnInit(): void {
    this.donorForm = this.fb.group({
      identityNumber: [this.donorToEdit?.identityNumber || '', Validators.required],
      name: [this.donorToEdit?.name || '', Validators.required],
      phone: [this.donorToEdit?.phone || '', Validators.required],
      email: [this.donorToEdit?.email || '', [Validators.required, Validators.email]]
    });
  }

  save() {
    if (this.donorToEdit) {
      this.donorService.update(this.donorToEdit.id, this.donorForm.value).subscribe(() => this.close.emit());
    } else {
      this.donorService.add(this.donorForm.value).subscribe(() => this.close.emit());
    }
  }
}
