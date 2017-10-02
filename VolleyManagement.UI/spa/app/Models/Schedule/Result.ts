import { SetScore } from './SetScore';

export interface Result {
    TotalScore: SetScore;
    SetScores: SetScore[];
    IsTechnicalDefeat: boolean;
}
