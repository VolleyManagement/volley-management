import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PivotStandingsComponent } from '../Components/PivotStanding/pivotstandings.component';
import { StandingsComponent } from '../Components/Standings/standings.component';


const routes: Routes = [
    { path: 'PivotTable/:id', component: PivotStandingsComponent },
    { path: 'Standings/:id', component: StandingsComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }
