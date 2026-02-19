import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GiftEdit } from './gift-edit';

describe('GiftEdit', () => {
  let component: GiftEdit;
  let fixture: ComponentFixture<GiftEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GiftEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GiftEdit);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
