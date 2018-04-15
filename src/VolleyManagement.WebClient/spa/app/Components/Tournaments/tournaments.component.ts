import { Component, OnInit } from '@angular/core';
import { Tournament } from '../../Models/Tournaments/Tournament';
import { TournamentsService } from '../../Services/tournaments.service';

@Component({
  selector: 'vm-tournaments',
  templateUrl: './tournaments.component.html',
  styleUrls: ['./tournaments.component.scss']
})
export class TournamentsComponent implements OnInit {

  private data: Tournament[];

  constructor(private tournamentsService: TournamentsService) { }

  ngOnInit() {
    this.tournamentsService
      .getTournaments()
      .toPromise()
      .then(data => this.data = data);
  }

}
