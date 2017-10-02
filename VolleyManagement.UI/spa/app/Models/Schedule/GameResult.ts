import { Result } from './Result';

export interface GameResult {
    Id: number;
    HomeTeamName: string;
    AwayTeamName: string;
    GameDate: string;
    Result: Result;
}
