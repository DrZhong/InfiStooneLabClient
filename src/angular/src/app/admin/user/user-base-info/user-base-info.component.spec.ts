import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserBaseInfoComponent } from './user-base-info.component';

describe('UserBaseInfoComponent', () => {
  let component: UserBaseInfoComponent;
  let fixture: ComponentFixture<UserBaseInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserBaseInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBaseInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
