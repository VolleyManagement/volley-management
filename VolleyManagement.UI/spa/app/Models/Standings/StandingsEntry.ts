export class StandingsEntry {
    constructor(
        public TeamName: string,
        public GamesTotal: number,
        public GamesWon: number,
        public GamesLost: number,
        public GamesWithScoreThreeNil: number,
        public GamesWithScoreThreeOne: number,
        public GamesWithScoreThreeTwo: number,
        public GamesWithScoreTwoThree: number,
        public GamesWithScoreOneThree: number,
        public GamesWithScoreNilThree: number,
        public SetsWon: number,
        public SetsLost: number,
        public SetsRatio: number,
        public SetsRatioText: string,
        public BallsWon: number,
        public BallsLost: number,
        public BallsRatio: number,
        public BallsRatioText: string
    ) { }
}
