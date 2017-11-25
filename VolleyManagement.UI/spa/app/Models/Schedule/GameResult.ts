import { Result } from './Result';

export class GameResult {
    public IsGameWasPlayedAndHasResult: boolean;
    constructor(
        public Id: number,
        public HomeTeamName: string,
        public AwayTeamName: string,
        public GameDate: string,
        public Round: number,
        public Result: Result,
        public DivisionId: number,
        public GroupId: number
    ) {
        this.IsGameWasPlayedAndHasResult = this.isGameWasPlayedAndHasResult();
     }

    private isGameWasPlayedAndHasResult(): boolean {
        return this.Result && (!this.Result.TotalScore.IsEmpty || this.Result.IsTechnicalDefeat);
    }
}
