import { LinkOwnerEnum } from "../enums/ownerEntities/LinkOwnerEnum";

export interface ILink {
    id: number;
    link: string;
    name?: string;
    owner: {
        type: LinkOwnerEnum;
        name: string;
        id: number;
    }
}