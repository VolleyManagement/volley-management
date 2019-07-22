import { DivisionHeader } from "../../Models/Schedule/DivisionHeader";

export class DummyDivisionHeader implements DivisionHeader {
    Id: number;
    Name: string;
    Rounds: number[];

    constructor() {
        this.Id = DummyDivisionHeader.DummyHeaderId;
    }

    public static readonly DummyHeaderId = -1;
    public static readonly PlayOffHeaderId = 0;
}