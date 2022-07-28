import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoticationDetailComponent } from './notication-detail.component';

describe('NoticationDetailComponent', () => {
  let component: NoticationDetailComponent;
  let fixture: ComponentFixture<NoticationDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NoticationDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NoticationDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
