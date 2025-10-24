import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IconsCardsComponent } from './icons-cards.component';

describe('IconsCardsComponent', () => {
  let component: IconsCardsComponent;
  let fixture: ComponentFixture<IconsCardsComponent>;

  beforeEach(async () => {
    TestBed.configureTestingModule({
      declarations: [ IconsCardsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(IconsCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
