import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseOrganization } from './choose-organization';

describe('ChooseOrganization', () => {
  let component: ChooseOrganization;
  let fixture: ComponentFixture<ChooseOrganization>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChooseOrganization],
    }).compileComponents();

    fixture = TestBed.createComponent(ChooseOrganization);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
