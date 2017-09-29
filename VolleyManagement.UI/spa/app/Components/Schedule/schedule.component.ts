import { Component, OnInit, Input } from '@angular/core';

import { GameResult } from '../../Models/Schedule/GameResult';
import { ScheduleService } from '../../Services/schedule.service';

@Component({
  selector: 'schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {

  @Input() scheduleId: number;
  gameResults: GameResult[];

  constructor(private scheduleService: ScheduleService) { }

  ngOnInit() {
    this.scheduleService.getSchedule(1)
      .subscribe(gameResults => {
        this.gameResults = gameResults;
      });

  }
}
