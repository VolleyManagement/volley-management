import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { TournamentMetadataJson } from './Models/TournamentMetadataJson/TournamentMetadataJson';
import { JsonService } from './Services/json.service';
import { PivotStandingsComponent } from './Components/PivotStanding/pivotstandings.component';
import { StandingsComponent } from './Components/Standings/standings.component';

@Component({
    selector: 'vm-app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    ngOnInit(): void {
    }
}
