import { SetScore } from './SetScore';

export interface GameResult {
    Id: number;
    HomeTeamName: string;
    AwayTeamName: string;
    GameDate: string;
    TotalScore: SetScore;
    SetScores: SetScore[];
    IsTechnicalDefeat: boolean;
}
