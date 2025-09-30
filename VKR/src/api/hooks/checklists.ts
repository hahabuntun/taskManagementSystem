import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
    getChecklistsByOwner,
    createChecklist,
    addChecklistItem,
    updateChecklistItem,
    deleteChecklistItem,
} from "../queries/checklistsQueries";
import { CheckListOwnerEnum } from "../../enums/ownerEntities/CheckListOwnerEnum";
import { ICheckList, ICheckListItem } from "../../interfaces/ICheckList";
import { notification } from "antd";

export const useChecklists = (ownerType: CheckListOwnerEnum, ownerId: number) => {
    const queryClient = useQueryClient();
    const [newChecklistTitle, setNewChecklistTitle] = useState<string>("");
    const [newItemTitle, setNewItemTitle] = useState<string>("");

    // Fetch checklists
    const checklistsQuery = useQuery({
        queryKey: ["checklists", ownerType, ownerId],
        queryFn: () => getChecklistsByOwner(ownerType, ownerId),
    });

    // Create checklist
    const createChecklistMutation = useMutation({
        mutationFn: (title: string) => createChecklist(ownerType, ownerId, title),
        onMutate: async (title) => {
            await queryClient.cancelQueries({ queryKey: ["checklists", ownerType, ownerId] });
            const previousChecklists = queryClient.getQueryData<ICheckList[]>(["checklists", ownerType, ownerId]) || [];
            const optimisticChecklist: ICheckList = {
                id: Math.random(), // Temporary ID
                ownerType,
                ownerId,
                title,
                items: [],
            };
            queryClient.setQueryData(["checklists", ownerType, ownerId], [...previousChecklists, optimisticChecklist]);
            return { previousChecklists };
        },
        onError: (err, title, context) => {
            console.log(err, title)
            queryClient.setQueryData(["checklists", ownerType, ownerId], context?.previousChecklists);
            notification.error({ message: "Ошибка при добавлении чеклиста" })
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["checklists", ownerType, ownerId] });
            notification.success({ message: "Чеклист добавлен" })
            setNewChecklistTitle("");
        },
    });

    // Add item
    const addItemMutation = useMutation({
        mutationFn: ({ checklistId, title }: { checklistId: number; title: string }) =>
            addChecklistItem(checklistId, title),
        onMutate: async ({ checklistId, title }) => {
            await queryClient.cancelQueries({ queryKey: ["checklists", ownerType, ownerId] });
            const previousChecklists = queryClient.getQueryData<ICheckList[]>(["checklists", ownerType, ownerId]) || [];
            const optimisticItem: ICheckListItem = {
                id: Math.random(), // Temporary ID
                checklistId,
                title,
                isCompleted: false,
            };
            const newChecklists = previousChecklists.map((checklist) =>
                checklist.id === checklistId
                    ? { ...checklist, items: [...checklist.items, optimisticItem] }
                    : checklist
            );
            queryClient.setQueryData(["checklists", ownerType, ownerId], newChecklists);
            return { previousChecklists };
        },
        onError: (err, variables, context) => {
            console.log(err, variables)
            queryClient.setQueryData(["checklists", ownerType, ownerId], context?.previousChecklists);
            notification.error({ message: "Ошибка при добавлении элемента в чеклист" })
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["checklists", ownerType, ownerId] });
            setNewItemTitle("");
            notification.success({ message: "Элемент добавлен в чеклист" })
        },
    });

    // Update item
    const updateItemMutation = useMutation({
        mutationFn: ({ itemId, updates }: { itemId: number; updates: { title?: string; isCompleted?: boolean } }) =>
            updateChecklistItem(itemId, updates),
        onMutate: async ({ itemId, updates }) => {
            await queryClient.cancelQueries({ queryKey: ["checklists", ownerType, ownerId] });
            const previousChecklists = queryClient.getQueryData<ICheckList[]>(["checklists", ownerType, ownerId]) || [];
            const newChecklists = previousChecklists.map((checklist) => ({
                ...checklist,
                items: checklist.items.map((item) =>
                    item.id === itemId
                        ? { ...item, ...updates }
                        : item
                ),
            }));
            queryClient.setQueryData(["checklists", ownerType, ownerId], newChecklists);
            return { previousChecklists };
        },
        onError: (err, variables, context) => {
            console.log(err, variables)
            queryClient.setQueryData(["checklists", ownerType, ownerId], context?.previousChecklists);
            notification.error({ message: "Ошибка изменения" })
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["checklists", ownerType, ownerId] });
            notification.success({ message: "Чеклист изменен" })
        },
    });

    // Delete item
    const deleteItemMutation = useMutation({
        mutationFn: (itemId: number) => deleteChecklistItem(itemId),
        onMutate: async (itemId) => {
            await queryClient.cancelQueries({ queryKey: ["checklists", ownerType, ownerId] });
            const previousChecklists = queryClient.getQueryData<ICheckList[]>(["checklists", ownerType, ownerId]) || [];
            const newChecklists = previousChecklists.map((checklist) => ({
                ...checklist,
                items: checklist.items.filter((item) => item.id !== itemId),
            }));
            queryClient.setQueryData(["checklists", ownerType, ownerId], newChecklists);
            return { previousChecklists };
        },
        onError: (err, itemId, context) => {
            console.log(err, itemId)
            queryClient.setQueryData(["checklists", ownerType, ownerId], context?.previousChecklists);
            notification.error({ message: "Ошибка при удалении" })
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["checklists", ownerType, ownerId] });
            notification.success({ message: "Чеклист удален" })
        },
    });

    const handleAddChecklist = () => {
        if (newChecklistTitle.trim()) {
            createChecklistMutation.mutate(newChecklistTitle.trim());
        }
    };

    const handleAddItem = (checklistId: number) => {
        if (newItemTitle.trim()) {
            addItemMutation.mutate({ checklistId, title: newItemTitle.trim() });
        }
    };

    const handleToggleItem = (itemId: number, isCompleted: boolean) => {
        updateItemMutation.mutate({ itemId, updates: { isCompleted } });
    };

    const handleSaveItem = (itemId: number, newTitle: string) => {
        if (newTitle.trim()) {
            updateItemMutation.mutate({ itemId, updates: { title: newTitle.trim() } });
        }
    };

    const handleDeleteItem = (itemId: number) => {
        deleteItemMutation.mutate(itemId);
    };

    return {
        checklists: checklistsQuery.data || [],
        isLoading: checklistsQuery.isLoading,
        error: checklistsQuery.error,
        newChecklistTitle,
        newItemTitle,
        setNewChecklistTitle,
        setNewItemTitle,
        handleAddChecklist,
        handleAddItem,
        handleToggleItem,
        handleSaveItem,
        handleDeleteItem,
    };
};