import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { PivotStandingsComponent } from './Components/PivotStanding/pivotstandings.component';
import { StandingsComponent } from './Components/Standings/standings.component';
import { ScheduleComponent } from './Components/Schedule/schedule.component';

import { StandingsService } from './Services/standings.service';
import { JsonService } from './Services/json.service';
import { ScheduleService } from './Services/schedule.service';

@NgModule({
    declarations: [
        AppComponent,
        PivotStandingsComponent,
        StandingsComponent,
        ScheduleComponent
    ],
    imports: [
        BrowserModule,
        HttpModule
    ],
    providers: [
        StandingsService,
        JsonService,
        ScheduleService],
    bootstrap: [AppComponent]
})
export class AppModule { }
