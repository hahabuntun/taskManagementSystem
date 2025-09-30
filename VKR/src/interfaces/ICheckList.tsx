import { CheckListOwnerEnum } from "../enums/ownerEntities/CheckListOwnerEnum";

export interface ICheckList {
  id: number;
  ownerType: CheckListOwnerEnum;
  ownerId: number;
  title: string;
  items: ICheckListItem[];
}

export interface ICheckListItem {
  id: number;
  checklistId: number;
  title: string;
  isCompleted: boolean;
}
