import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatToggleButtonComponent } from './chat-toggle-button.component';

describe('ChatToggleButtonComponent', () => {
  let component: ChatToggleButtonComponent;
  let fixture: ComponentFixture<ChatToggleButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChatToggleButtonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatToggleButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
