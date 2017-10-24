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
  private pivotId: number;
  private standingsId: number;
  private scheduleId: number;
  public isShowLoader = false;
  private isCss3Supported = false;

  constructor(private jsonService: JsonService) { }

  ngOnInit(): void {
    this.isShowLoader = true;
    const tournamentJsonUrl = this.getTournamentMetadataFileName();
    this.isCss3Supported = this.checkCss3Support();

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

  private getTableToShow(mode: string, id: number): void {
    switch (mode) {
      case 'pivotTable':
        this.pivotId = id;
        break;
      case 'standings':
        this.standingsId = id;
        break;
      case 'schedule':
        this.scheduleId = id;
        break;
    }
  }

  private checkCss3Support(): boolean {
    let propertyToCheck = 'border-radius';
    const div = document.createElement('div');
    const vendors = 'Khtml Ms O Moz Webkit'.split(' ');
    let len = vendors.length;
    let result = false;

    if (propertyToCheck in div.style) {
      result = true;
    }

    propertyToCheck = propertyToCheck.replace(/^[a-z]/, function (val) {
      return val.toUpperCase();
    });

    while (len--) {
      if (vendors[len] + propertyToCheck in div.style) {
        result = true;
      }
    }
    return result;
  }
}
