import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IconsCardComponent } from './icons-card.component';

describe('IconsCardComponent', () => {
  let component: IconsCardComponent;
  let fixture: ComponentFixture<IconsCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IconsCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IconsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
