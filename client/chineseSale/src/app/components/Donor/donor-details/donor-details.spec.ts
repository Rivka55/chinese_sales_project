import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonorDetails } from './donor-details';

describe('DonorDetails', () => {
  let component: DonorDetails;
  let fixture: ComponentFixture<DonorDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonorDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DonorDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
