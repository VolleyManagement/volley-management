export class ShortGameResult {
    constructor(
        public HomeSetsScore: number,
        public AwaySetsScore: number,
        public IsTechnicalDefeat: boolean,
        public FormattedResult: string,
        public CssClass: string
    ) { }
}
