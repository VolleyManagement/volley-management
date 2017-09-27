import { PivotStandingsEntry } from './PivotStandingsEntry';
import { PivotStandingsGame } from './PivotStandingsGame';

export class PivotStandings {
    constructor(
        public TeamsStandings: PivotStandingsEntry[],
        public GamesStandings: PivotStandingsGame[]
    ) { }
}
