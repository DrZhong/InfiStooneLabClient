import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoticationComponent } from './notication.component';

describe('NoticationComponent', () => {
  let component: NoticationComponent;
  let fixture: ComponentFixture<NoticationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NoticationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NoticationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
