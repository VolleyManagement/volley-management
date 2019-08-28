import { PivotStandingsEntry } from './PivotStandingsEntry';
import { PivotStandingsGame } from './PivotStandingsGame';

export interface PivotStandings {
    LastUpdateTime: Date;
    TeamsStandings: PivotStandingsEntry[];
    GamesStandings: PivotStandingsGame[];
}
