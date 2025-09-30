import { BoardBasisEnum } from "../enums/BoardBasisEnum";
import { BoardOwnerEnum } from "../enums/ownerEntities/BoardOwnerEnum";

import { Dayjs } from "dayjs";

export interface IBoard {
    id: number;
    name: string;
    createdAt: Dayjs;
    boardBasis: BoardBasisEnum;
    owner: {
        type: BoardOwnerEnum;
        name: string;
        id: number;
    }
}