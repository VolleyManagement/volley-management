import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../environments/environment';
import { JsonService } from './json.service';
import { Tournament } from '../Models/Tournaments/Tournament';

@Injectable()
export class TournamentsService {

  private tournamentsUrl = 'api/Tournaments';

  constructor(private _jsonService: JsonService) { }

  getTournaments(): Observable<Tournament[]> {
    const url = environment.apiUrl.concat(this.tournamentsUrl);
    return this._jsonService.getJson<Tournament[]>(url)
      .map((data: any) => {
        return data.map(item => ({
          Name: item.name,
          Season: item.season
        }));
      });
  }
}
