import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { AppRoutingModule } from './Modules/app-routing';

import { AppComponent } from './app.component';
import { StandingsComponent } from './Components/Standings/standings.component';

import { StandingsService } from './Services/standings.service';

@NgModule({
    declarations: [
        AppComponent,
        StandingsComponent
    ],
    imports: [
        BrowserModule,
        HttpModule,
        AppRoutingModule
    ],
    providers: [StandingsService],
    bootstrap: [AppComponent]
})
export class AppModule { }
