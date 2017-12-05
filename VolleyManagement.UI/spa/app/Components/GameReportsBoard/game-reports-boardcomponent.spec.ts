import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GameReportsBoardComponent } from './game-reports-board.component';

describe('GameReportsBoardComponent', () => {
  let component: GameReportsBoardComponent;
  let fixture: ComponentFixture<GameReportsBoardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameReportsBoardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameReportsBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
