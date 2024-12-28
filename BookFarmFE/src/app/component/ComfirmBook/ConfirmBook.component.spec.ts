import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmBookComponent } from './ConfirmBook.component';

describe('ConfirmBookComponent', () => {
  let component: ConfirmBookComponent;
  let fixture: ComponentFixture<ConfirmBookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmBookComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ConfirmBookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
