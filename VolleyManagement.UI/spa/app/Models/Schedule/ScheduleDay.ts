import { GameResult } from './GameResult';
import { DivsionHeader } from './DivsionHeader';

export interface ScheduleDay {
    Date: string;
    Games: GameResult[];
    Divisions: DivsionHeader[];
}
