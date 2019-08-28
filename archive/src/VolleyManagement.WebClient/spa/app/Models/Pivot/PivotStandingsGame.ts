import { ShortGameResult } from './ShortGameResult';

export class PivotStandingsGame {
    constructor(
        public HomeTeamId: number,
        public AwayTeamId: number,
        public Results: ShortGameResult[]
    ) { }

    public clone(): PivotStandingsGame {
        return new PivotStandingsGame(
            this.HomeTeamId,
            this.AwayTeamId,
            this.Results.slice());
    }

    public transposeResult(): PivotStandingsGame {
        return new PivotStandingsGame(
            this.AwayTeamId,
            this.HomeTeamId,
            this.Results.map(item => ({
                HomeSetsScore: item.AwaySetsScore,
                AwaySetsScore: item.HomeSetsScore,
                IsTechnicalDefeat: item.IsTechnicalDefeat,
                RoundNumber: item.RoundNumber
            })));
    }
}
