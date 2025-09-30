import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"; // Added useQueryClient
import { INotificationFilterOptions } from "../options/filterOptions/INotificationFilterOptions";
import { useState } from "react";
import {
    getAllNotifications,
    getAllSubscriptions,
    getIsSubscribedToEntity,
    subscribeToEntity,
    unsubscribeFromEntity,
    markNotificationAsRead,
    deleteNotification
} from "../queries/notificationsQueries";
import { NotificationOwnerEnum } from "../../enums/ownerEntities/NotificationOwnerEnum";
import { notification } from "antd";

export const useGetNotifications = (workerId: number, enabled: boolean = true) => {
    const [filters, setFilters] = useState<INotificationFilterOptions>({});
    const query = useQuery({
        queryKey: ['notifications', filters],
        enabled: enabled,
        queryFn: () => getAllNotifications(workerId, filters)
    });

    return {
        ...query,
        filters,
        setFilters
    };
};

export const useGetSubscriptions = (workerId: number, enabled: boolean = true) => {
    const query = useQuery({
        queryKey: ['subscriptions'],
        enabled: enabled,
        queryFn: () => getAllSubscriptions(workerId)
    });

    return {
        ...query,
    };
};

export const useGetIsSubscribedToEntity = (workerId: number, entityId: number | null, entityType: NotificationOwnerEnum, enabled: boolean = true) => {
    const query = useQuery({
        queryKey: ['isSubscribed', entityId, entityType],
        enabled: enabled && entityId !== null,
        queryFn: () => {
            if (entityId) {
                return getIsSubscribedToEntity(workerId, entityId, entityType);
            }
            else {
                return false;
            }
        }
    });

    return {
        ...query,
    };
};

export const useSubscribeToEntity = (handleSuccess?: () => void) => {
    const queryClient = useQueryClient(); // Added queryClient
    const mutation = useMutation({
        mutationFn: ({
            entityId,
            workerId,
            entityType
        }: {
            entityId: number,
            workerId: number,
            entityType: NotificationOwnerEnum
        }) => subscribeToEntity(workerId, entityId, entityType),
        onSuccess: (isSuccess: boolean, variables) => {
            if (isSuccess) {
                notification.success({ message: "Успех" });
                // Invalidate relevant queries
                queryClient.invalidateQueries({ queryKey: ['subscriptions'] });
                queryClient.invalidateQueries({ queryKey: ['isSubscribed', variables.entityId, variables.entityType] });
                handleSuccess?.();
            } else {
                notification.error({ message: "Ошибка" });
            }
        },
        onError: () => {
            notification.error({ message: "Ошибка" });
        }
    });

    return mutation.mutateAsync;
};

export const useUnsubscribeFromEntity = (handleSuccess?: () => void) => {
    const queryClient = useQueryClient(); // Added queryClient
    const mutation = useMutation({
        mutationFn: ({
            entityId,
            workerId,
            entityType
        }: {
            entityId: number,
            workerId: number,
            entityType: NotificationOwnerEnum
        }) => unsubscribeFromEntity(workerId, entityId, entityType),
        onSuccess: (isSuccess: boolean, variables) => {
            if (isSuccess) {
                notification.success({ message: "Успех" });
                // Invalidate relevant queries
                queryClient.invalidateQueries({ queryKey: ['subscriptions'] });
                queryClient.invalidateQueries({ queryKey: ['isSubscribed', variables.entityId, variables.entityType] });
                handleSuccess?.();
            } else {
                notification.error({ message: "Ошибка" });
            }
        },
        onError: () => {
            notification.error({ message: "Ошибка" });
        }
    });

    return mutation.mutateAsync;
};

export const useMarkNotificationAsRead = (handleSuccess?: () => void) => {
    const queryClient = useQueryClient(); // Added queryClient
    const mutation = useMutation({
        mutationFn: ({ workerId, notificationId }: { workerId: number, notificationId: number }) => markNotificationAsRead(workerId, notificationId),
        onSuccess: (isSuccess: boolean) => {
            if (isSuccess) {
                notification.success({ message: "Уведомление отмечено как прочитанное" });
                // Invalidate notifications query
                queryClient.invalidateQueries({ queryKey: ['notifications'] });
                handleSuccess?.();
            } else {
                notification.error({ message: "Не удалось отметить уведомление как прочитанное" });
            }
        },
        onError: () => {
            notification.error({ message: "Ошибка при обновлении статуса уведомления" });
        }
    });

    return mutation.mutateAsync;
};

export const useDeleteNotification = (handleSuccess?: () => void) => {
    const queryClient = useQueryClient(); // Added queryClient
    const mutation = useMutation({
        mutationFn: ({ workerId, notificationId }: { workerId: number, notificationId: number }) => deleteNotification(workerId, notificationId),
        onSuccess: (isSuccess: boolean) => {
            if (isSuccess) {
                notification.success({ message: "Уведомление удалено" });
                // Invalidate notifications query
                queryClient.invalidateQueries({ queryKey: ['notifications'] });
                handleSuccess?.();
            } else {
                notification.error({ message: "Не удалось удалить уведомление" });
            }
        },
        onError: () => {
            notification.error({ message: "Ошибка при удалении уведомления" });
        }
    });

    return mutation.mutateAsync;
};