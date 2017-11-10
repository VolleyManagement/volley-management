import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { TournamentMetadataJson } from './Models/TournamentMetadataJson/TournamentMetadataJson';
import { JsonService } from './Services/json.service';
import { PivotStandingsComponent } from './Components/PivotStanding/pivotstandings.component';
import { StandingsComponent } from './Components/Standings/standings.component';

@Component({
  selector: 'vm-app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  private pivotTable: PivotStandingsComponent;
  private standings: StandingsComponent;
  public tournamentJson: TournamentMetadataJson;
  public pivotId: number;
  public standingsId: number;
  public scheduleId: number;
  public isShowLoader = false;

  constructor(private jsonService: JsonService) { }

  ngOnInit(): void {
    this.isShowLoader = true;
    const tournamentJsonUrl = this.getTournamentMetadataFileName();

    this.getTournamentData(tournamentJsonUrl)
      .subscribe(json => {
        this.tournamentJson = json;
        this.getTableToShow(this.tournamentJson.mode, this.tournamentJson.id);
        this.isShowLoader = false;
      });
  }

  private getTournamentMetadataFileName(): string {
    return document.getElementsByTagName('vm-app')[0].getAttribute('metadatafile');
  }

  private getTournamentData(jsonUrl: string): Observable<TournamentMetadataJson> {
    return this.jsonService.getTournamentJson(jsonUrl);
  }

  private getTableToShow(modes: string[], id: number): void {
    modes.forEach(mode=>{
      switch (mode) {
        case 'pivot':
          this.pivotId = id;
          break;
        case 'standings':
          this.standingsId = id;
          break;
        case 'schedule':
          this.scheduleId = id;
          break;
      }
    });    
  }
}
