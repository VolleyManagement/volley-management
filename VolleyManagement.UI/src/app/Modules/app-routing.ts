import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { StandingsComponent } from '../Components/Standings/standings.component';


const routes: Routes = [
    { path: 'Standings/:id', component: StandingsComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})

export class AppRoutingModule { }
