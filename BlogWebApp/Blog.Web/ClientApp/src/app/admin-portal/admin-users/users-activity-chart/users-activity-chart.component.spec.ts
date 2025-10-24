import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersActivityChartComponent } from './users-activity-chart.component';

describe('UsersActivityChartComponent', () => {
  let component: UsersActivityChartComponent;
  let fixture: ComponentFixture<UsersActivityChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UsersActivityChartComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UsersActivityChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
