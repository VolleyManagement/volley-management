import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { TournamentJson } from './Models/TournamentJson/TournamentJson';
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
  public tournamentJson: TournamentJson;
  private pivotId: number;
  private standingsId: number;

  constructor(private jsonService: JsonService) { }

  ngOnInit(): void {
    const tournamentJsonUrl = document.
      getElementsByTagName('vm-app')[0].
      getAttribute('metadatafile');

    this.getTournamentData(tournamentJsonUrl)
      .subscribe(json => {
        this.tournamentJson = json;
        this.getTableToShow(this.tournamentJson.mode, this.tournamentJson.id);
      });
  }

  private getTournamentData(jsonUrl: string): Observable<TournamentJson> {
    return this.jsonService.getTournamentJson(jsonUrl);
  }

  private getTableToShow(mode: string, id: number): void {
    switch (mode) {
      case 'pivotTable':
        this.pivotId = id;
        break;
      case 'standings':
        this.standingsId = id;
        break;
    }
  }
}
