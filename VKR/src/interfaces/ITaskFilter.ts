import { ITaskFilterOptions } from "../api/options/filterOptions/ITaskFilterOptions";

export interface ITaskFilter {
    name: string;
    options: ITaskFilterOptions;
}