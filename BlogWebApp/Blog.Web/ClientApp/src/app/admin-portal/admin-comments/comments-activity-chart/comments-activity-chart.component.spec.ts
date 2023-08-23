import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentsActivityChartComponent } from './comments-activity-chart.component';

describe('CommentsActivityChartComponent', () => {
  let component: CommentsActivityChartComponent;
  let fixture: ComponentFixture<CommentsActivityChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommentsActivityChartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommentsActivityChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
