import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { PivotStandingsComponent } from './Components/PivotStanding/pivotstandings.component';
import { StandingsComponent } from './Components/Standings/standings.component';

import { StandingsService } from './Services/standings.service';
import { JsonService } from './Services/json.service';
import { CustomDecimalPipe } from './CustomPipes/CustomDecimalPipe';

@NgModule({
    declarations: [
        AppComponent,
        PivotStandingsComponent,
        StandingsComponent,
        CustomDecimalPipe
    ],
    imports: [
        BrowserModule,
        HttpModule
    ],
    providers: [
        StandingsService,
        JsonService],
    bootstrap: [AppComponent]
})
export class AppModule { }
