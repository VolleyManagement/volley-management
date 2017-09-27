import { PivotStandingsEntry } from './PivotStandingsEntry';
import { PivotStandingsGame } from './PivotStandingsGame';

export interface PivotStandings {
    TeamsStandings: PivotStandingsEntry[];
    GamesStandings: PivotStandingsGame[];
}
