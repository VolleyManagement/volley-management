import { StandingsEntry } from "./StandingsEntry";

export interface DivisionStandings {
    LastUpdateTime: Date;
    StandingsTable: StandingsEntry[]
}