import { GameResult } from './GameResult';
import { DivisionHeader } from './DivisionHeader';

export interface ScheduleDay {
    Date: string;
    Games: GameResult[];
    Divisions: DivisionHeader[];
}
