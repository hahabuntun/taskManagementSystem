import { apiClient } from "../../config/axiosConfig";
import { CheckListOwnerEnum } from "../../enums/ownerEntities/CheckListOwnerEnum";
import { ICheckList, ICheckListItem } from "../../interfaces/ICheckList";

export const getChecklistsByOwner = async (
    ownerType: CheckListOwnerEnum,
    ownerId: number
): Promise<ICheckList[]> => {
    const response = await apiClient.get("/checklists/", {
        params: { ownerType, ownerId },
    });
    return response.data.map((item: any) => ({
        id: item.Id,
        ownerType: item.OwnerType as CheckListOwnerEnum,
        ownerId: item.OwnerId,
        title: item.Title,
        items: item.Items.map((i: any) => ({
            id: i.Id,
            checklistId: i.ChecklistId,
            title: i.Title,
            isCompleted: i.IsCompleted,
        })),
    }));
};

export const createChecklist = async (
    ownerType: CheckListOwnerEnum,
    ownerId: number,
    title: string
): Promise<ICheckList> => {
    const response = await apiClient.post("/checklists/", { title }, {
        params: { ownerType, ownerId },
    });
    return {
        id: response.data.Id,
        ownerType: response.data.OwnerType,
        ownerId: response.data.OwnerId,
        title: response.data.Title,
        items: response.data.Items.map((i: any) => ({
            id: i.Id,
            checklistId: i.ChecklistId,
            title: i.Title,
            isCompleted: i.IsCompleted,
        })),
    };
};

export const addChecklistItem = async (
    checklistId: number,
    title: string
): Promise<ICheckListItem> => {
    const response = await apiClient.post(`/checklists/${checklistId}/items`, { title });
    return {
        id: response.data.Id,
        checklistId: response.data.ChecklistId,
        title: response.data.Title,
        isCompleted: response.data.IsCompleted,
    };
};

export const updateChecklistItem = async (
    itemId: number,
    updates: { title?: string; isCompleted?: boolean }
): Promise<void> => {
    await apiClient.patch(`/checklists/items/${itemId}`, updates);
};

export const deleteChecklistItem = async (itemId: number): Promise<void> => {
    await apiClient.delete(`/checklists/items/${itemId}`);
};