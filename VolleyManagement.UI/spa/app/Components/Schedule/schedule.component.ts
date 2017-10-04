import { Component, OnInit, Input } from '@angular/core';

import { ScheduleByRounds } from '../../Models/Schedule/ScheduleByRounds';
import { ScheduleService } from '../../Services/schedule.service';

@Component({
  selector: 'schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {

  @Input() scheduleId: number;
  gameResults: ScheduleByRounds[];

  constructor(private scheduleService: ScheduleService) { }

  ngOnInit() {
    this.scheduleService.getSchedule(this.scheduleId)
      .subscribe(gameResults => {
        this.gameResults = gameResults;
        this.gameResults.forEach((item, index) => {
          item.Round = item[0] ? item[0].Round : index + 1;
        });
      });
  }
}
