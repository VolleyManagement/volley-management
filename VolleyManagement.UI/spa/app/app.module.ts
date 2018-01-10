import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';

import { StandingsService } from './Services/standings.service';
import { JsonService } from './Services/json.service';
import { ScheduleService } from './Services/schedule.service';
import { AppToolsService } from './Services/app-tools.service';

import { InfinityDecimalPipe } from './CustomPipes/InfinityDecimalPipe';
import { LocalDatePipe } from './CustomPipes/LocalDatePipe';

import { PivotStandingsComponent } from './Components/PivotStanding/pivotstandings.component';
import { StandingsComponent } from './Components/Standings/standings.component';
import { ScheduleComponent } from './Components/Schedule/schedule.component';
import { LoaderComponent } from './Components/loader/loader.component';
import { GameReportsBoardComponent } from './Components/GameReportsBoard/game-reports-board.component';
import { TournamentDataService } from './Services/tournament-data.service';
import { GameResultCellDirective } from './Components/PivotStanding/game-result-cell.directive';

@NgModule({
    declarations: [
        AppComponent,

        GameResultCellDirective,
        PivotStandingsComponent,
        StandingsComponent,
        ScheduleComponent,
        InfinityDecimalPipe,
        LoaderComponent,
        GameReportsBoardComponent,
        LocalDatePipe
    ],
    imports: [
        BrowserModule,
        HttpModule
    ],
    providers: [
        JsonService,
        AppToolsService,
        TournamentDataService,
        StandingsService,
        ScheduleService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
