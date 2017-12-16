import { Component, OnInit, Input, OnDestroy, Output, EventEmitter } from '@angular/core';

import 'rxjs/add/operator/toPromise';

import { StandingsService } from '../../Services/standings.service';
import { StandingsEntry } from '../../Models/Standings/StandingsEntry';
import { DivisionStandings } from '../../Models/Standings/DivisionStandings';

@Component({
    selector: 'standings',
    templateUrl: './standings.component.html',
    styleUrls: ['./standings.component.scss']
})
export class StandingsComponent implements OnInit {

    @Input() standingsId: number;
    @Output() ready: EventEmitter<void> = new EventEmitter<void>();

    data: DivisionStandings[];

    constructor(private _service: StandingsService) { }

    ngOnInit(): void {
        this._service
            .getStandings(this.standingsId)
            .toPromise()
            .then(data => {
                this.data = data;
                this.ready.emit();
            });
    }
}
