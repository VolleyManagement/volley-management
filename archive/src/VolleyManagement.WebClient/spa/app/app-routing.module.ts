import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ScheduleComponent } from '../app/Components/Schedule/schedule.component';
import { LoginComponent } from '../app/Components/Login/login.component';
import { TournamentsComponent } from '../app/Components/Tournaments/tournaments.component';



const routes: Routes = [
  { path: 'Login', component: LoginComponent },
  { path: 'Tournaments', component: TournamentsComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule],
  declarations: []
})
export class AppRoutingModule { }
