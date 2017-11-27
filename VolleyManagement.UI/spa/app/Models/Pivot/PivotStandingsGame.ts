import { ShortGameResult } from './ShortGameResult';

export class PivotStandingsGame {
    constructor(
        public HomeTeamId: number,
        public AwayTeamId: number,
        public Results: ShortGameResult[]
    ) { }

    public static getNonPlayableCell(): PivotStandingsGame {
        return new PivotStandingsGame(
            0,
            0,
            [ShortGameResult.getNonPlayableCell()]);
    }

    public clone(): PivotStandingsGame {
        return new PivotStandingsGame(
            this.HomeTeamId,
            this.AwayTeamId,
            this.Results.map(item => new ShortGameResult(
                item.HomeSetsScore,
                item.AwaySetsScore,
                item.IsTechnicalDefeat)));
    }

    public transposeResult(): PivotStandingsGame {
        return new PivotStandingsGame(
            this.AwayTeamId,
            this.HomeTeamId,
            this.Results.map(item => new ShortGameResult(
                item.AwaySetsScore,
                item.HomeSetsScore,
                item.IsTechnicalDefeat)));
    }
}
