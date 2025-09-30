import { Button } from "antd";
import { CheckOutlined, StopOutlined } from "@ant-design/icons";
import { forwardRef } from "react";
import {
  useGetIsSubscribedToEntity,
  useSubscribeToEntity,
  useUnsubscribeFromEntity,
} from "../../api/hooks/notifications";
import { NotificationOwnerEnum } from "../../enums/ownerEntities/NotificationOwnerEnum";
import useApplicationStore from "../../stores/applicationStore";

interface ISubscriptionButtonProps {
  entityType: NotificationOwnerEnum;
  entityId: number;
  style?: React.CSSProperties | undefined;
}

export const SubscriptionButton = forwardRef(
  (
    { entityType, entityId, style }: ISubscriptionButtonProps,
    ref: React.Ref<HTMLButtonElement>
  ) => {
    const { user } = useApplicationStore.getState();

    const { data: isSubscirbed, refetch } = useGetIsSubscribedToEntity(
      user?.id!,
      entityId,
      entityType,
      !!user
    );

    const subscribeAsync = useSubscribeToEntity(() => {
      refetch();
    });

    const unsubscribeAsync = useUnsubscribeFromEntity(() => {
      refetch();
    });
    return (
      <>
        {!isSubscirbed ? (
          <Button
            ref={ref}
            size="small"
            style={{ ...style }}
            onClick={() =>
              subscribeAsync({
                entityId: entityId,
                workerId: user?.id!,
                entityType: entityType,
              })
            }
            icon={<CheckOutlined style={{ color: "#389e0d" }} />}
          >
            Подписаться
          </Button>
        ) : (
          <Button
            ref={ref}
            size="small"
            style={{ ...style }}
            onClick={() =>
              unsubscribeAsync({
                workerId: user?.id!,
                entityId: entityId,
                entityType: entityType,
              })
            }
            icon={<StopOutlined style={{ color: "#dc4446" }} />}
          >
            Отписаться
          </Button>
        )}
      </>
    );
  }
);
