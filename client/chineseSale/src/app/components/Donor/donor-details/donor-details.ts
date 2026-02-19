import { Component, Input } from '@angular/core';
import { DonorModel } from '../../../models/Donor/DonorModel';

@Component({
  selector: 'app-donor-details',
  imports: [],
  templateUrl: './donor-details.html',
  styleUrls: ['./donor-details.scss'],
})
export class DonorDetails {
  @Input() donor?: DonorModel;
}
