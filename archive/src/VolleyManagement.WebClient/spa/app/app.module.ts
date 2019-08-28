import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { VouIntegrationComponent } from './integration/vou-integration.component';

import { environment } from '../environments/environment';

import { StandingsService } from './Services/standings.service';
import { JsonService } from './Services/json.service';
import { ScheduleService } from './Services/schedule.service';
import { AppToolsService } from './Services/app-tools.service';
import { TournamentsService } from './Services/tournaments.service';


import { InfinityDecimalPipe } from './CustomPipes/InfinityDecimalPipe';
import { LocalDatePipe } from './CustomPipes/LocalDatePipe';

import { PivotStandingsComponent } from './Components/PivotStanding/pivotstandings.component';
import { StandingsComponent } from './Components/Standings/standings.component';
import { ScheduleComponent } from './Components/Schedule/schedule.component';
import { LoaderComponent } from './Components/loader/loader.component';
import { GameReportsBoardComponent } from './Components/GameReportsBoard/game-reports-board.component';
import { TournamentDataService } from './Services/tournament-data.service';
import { GameResultCellDirective } from './Components/PivotStanding/game-result-cell.directive';
import { LoginComponent } from './Components/Login/login.component';
import { AppRoutingModule } from './app-routing.module';
import { TournamentsComponent } from './Components/Tournaments/tournaments.component';
import { MenuComponent } from './Components/Menu/menu.component';
import { GoogleLoginService } from './Services/googleLogin.service';
import { TokenInterceptor } from './Services/tokenInterceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';


@NgModule({
    declarations: [
        AppComponent,
        VouIntegrationComponent,

        GameResultCellDirective,
        PivotStandingsComponent,
        StandingsComponent,
        ScheduleComponent,
        InfinityDecimalPipe,
        LoaderComponent,
        GameReportsBoardComponent,
        LocalDatePipe,
        LoginComponent,
        TournamentsComponent,
        MenuComponent
    ],
    imports: [
        BrowserModule,
        HttpModule,
        AppRoutingModule,
        HttpClientModule
    ],
    providers: [
        JsonService,
        AppToolsService,
        TournamentDataService,
        StandingsService,
        ScheduleService,
        TournamentsService,
        GoogleLoginService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: TokenInterceptor,
            multi: true
        }
    ],
    bootstrap: [(environment.vouIntegration ? VouIntegrationComponent : AppComponent)]
})
export class AppModule { }
