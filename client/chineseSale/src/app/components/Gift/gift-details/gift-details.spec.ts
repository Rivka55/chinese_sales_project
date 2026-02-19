import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftDetails } from './gift-details';

describe('GiftDetails', () => {
  let component: GiftDetails;
  let fixture: ComponentFixture<GiftDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftDetails);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
