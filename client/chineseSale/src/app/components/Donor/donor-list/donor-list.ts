import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DonorModel } from '../../../models/Donor/DonorModel';
import { DonorService } from '../../../services/Donor/donor-service';
import { DonorForm } from '../donor-form/donor-form';
import { DonorDetails } from '../donor-details/donor-details';

@Component({
  selector: 'app-donor-list',
  imports: [FormsModule, DonorForm, DonorDetails],
  templateUrl: './donor-list.html',
  styleUrls: ['./donor-list.scss'],
})
export class DonorList {
  donors: DonorModel[] = [];
  showForm = false;
  selectedDonor?: DonorModel;
  expandedDonor?: DonorModel;

  // Search fields
  searchName = '';
  searchGift = '';
  searchEmail = '';

  constructor(private donorService: DonorService) {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.showForm = false;
    this.selectedDonor = undefined;
    this.donorService.getAll().subscribe(res => this.donors = res);
  }

  search() {
    this.donorService.search(
      this.searchName || undefined,
      this.searchGift || undefined,
      this.searchEmail || undefined
    ).subscribe(res => this.donors = res);
  }

  clearSearch() {
    this.searchName = '';
    this.searchGift = '';
    this.searchEmail = '';
    this.refresh();
  }

  openAdd() {
    this.selectedDonor = undefined;
    this.showForm = true;
  }

  openEdit(donor: DonorModel) {
    this.selectedDonor = donor;
    this.showForm = true;
  }

  toggleDetails(donor: DonorModel) {
    this.expandedDonor = this.expandedDonor?.id === donor.id ? undefined : donor;
  }

  delete(id: number) {
    if (confirm('האם אתה בטוח שברצונך למחוק את התורם?')) {
      this.donorService.delete(id).subscribe(() => this.refresh());
    }
  }
}
