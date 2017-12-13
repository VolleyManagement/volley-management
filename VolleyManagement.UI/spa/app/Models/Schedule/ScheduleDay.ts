import { GameResult } from './GameResult';
import { DivisionTitle } from './DivisionTitle';

export interface ScheduleDay {
    Date: string;
    Games: GameResult[];
    Divisions: DivisionTitle[];
}
