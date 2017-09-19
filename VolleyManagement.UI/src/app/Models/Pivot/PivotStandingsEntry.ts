import { PivotStandingsGame } from './PivotStandingsGame';

export class PivotStandingsEntry {
    constructor(
        public TeamId: number,
        public TeamName: string,
        public Points: number,
        public SetsRatio: number,
        public SetsRatioText: string
    ) { }
}
