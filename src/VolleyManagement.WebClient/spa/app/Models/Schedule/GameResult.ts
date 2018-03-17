import { Result } from './Result';

export interface GameResult {
    Id: number;
    HomeTeamName: string;
    AwayTeamName: string;
    GameDate: string;
    Round: number;
    GameNumber: number;
    Result: Result;
    DivisionId: number;
    GroupId: number;
    UrlToGameVideo: string;
}
