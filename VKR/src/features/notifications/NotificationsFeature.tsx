import {
  Button,
  Space,
  List,
  Tag,
  Typography,
  Popconfirm,
  Badge,
  Collapse,
  Divider,
} from "antd";
import {
  CloseOutlined,
  CheckOutlined,
  DeleteOutlined,
} from "@ant-design/icons";
import {
  useGetNotifications,
  useGetSubscriptions,
  useUnsubscribeFromEntity,
  useMarkNotificationAsRead,
  useDeleteNotification,
} from "../../api/hooks/notifications";
import { NotificationOwnerEnum } from "../../enums/ownerEntities/NotificationOwnerEnum";
import { FilterNotificationsForm } from "./FilterNotificationsForm";
import { INotificationFilterOptions } from "../../api/options/filterOptions/INotificationFilterOptions";
import useApplicationStore from "../../stores/applicationStore";

export const NotificationsFeature = () => {
  const { user } = useApplicationStore.getState();
  const { data: subscriptions = [] } = useGetSubscriptions(user?.id!, !!user);
  const { data: notifications = [], setFilters } = useGetNotifications(
    user?.id!,
    !!user
  );

  const availableWorkers = Array.from(
    new Map(
      notifications.map((n) => [n.responsibleWorker.id, n.responsibleWorker])
    ).values()
  );

  const unsubscribe = useUnsubscribeFromEntity();

  const markAsRead = useMarkNotificationAsRead();

  const deleteNotification = useDeleteNotification();

  const handleUnsubscribe = (id: number, entityType: NotificationOwnerEnum) => {
    unsubscribe({ workerId: user?.id!, entityId: id, entityType });
  };

  const handleFilterApply = (newFilters: INotificationFilterOptions) => {
    setFilters(newFilters);
  };

  return (
    <Space
      direction="vertical"
      style={{
        width: "100%",
        padding: 16,
      }}
    >
      <Typography.Title level={4}>Управление уведомлениями</Typography.Title>

      <Collapse
        style={{ margin: "1rem 0" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: (
              <FilterNotificationsForm
                onFilterApply={handleFilterApply}
                availableWorkers={availableWorkers}
              />
            ),
          },
        ]}
      />

      <Space direction="vertical" style={{ marginBottom: 16 }}>
        <Typography.Text strong>Подписки</Typography.Text>
        <Space wrap>
          {subscriptions.map((sub) => (
            <Tag key={sub.id} color="success">
              {sub.relatedEntity.type === NotificationOwnerEnum.ORGANIZATION
                ? "Организация"
                : sub.relatedEntity.type}
              : {sub.relatedEntity.name}
              <CloseOutlined
                style={{ marginLeft: 8 }}
                onClick={(e) => {
                  e.stopPropagation();
                  handleUnsubscribe(
                    sub.relatedEntity.id,
                    sub.relatedEntity.type
                  );
                }}
              />
            </Tag>
          ))}
        </Space>
      </Space>
      <Divider />

      <Typography.Text strong>Уведомления</Typography.Text>
      <List
        dataSource={notifications}
        renderItem={(item) => (
          <List.Item
            actions={[
              !item.isRead && (
                <Button
                  key="mark-read"
                  icon={<CheckOutlined />}
                  type="primary"
                  onClick={() =>
                    markAsRead({ workerId: user?.id!, notificationId: item.id })
                  }
                >
                  Прочитано
                </Button>
              ),
              <Popconfirm
                placement="topLeft"
                title="Вы уверены, что хотите удалить это уведомление?"
                onConfirm={() =>
                  deleteNotification({
                    workerId: user?.id!,
                    notificationId: item.id,
                  })
                }
                okText="Да"
                cancelText="Нет"
              >
                <Button key="delete" icon={<DeleteOutlined />} danger>
                  Удалить
                </Button>
              </Popconfirm>,
            ].filter(Boolean)}
          >
            <List.Item.Meta
              title={
                <Space>
                  <Tag>{item.relatedEntity.name}</Tag>
                  {!item.isRead && <Badge dot />}
                </Space>
              }
              description={
                <Typography.Text>
                  {item.message} (Создано:{" "}
                  {item.createdAt.format("DD.MM.YYYY HH:mm")})
                </Typography.Text>
              }
            />
          </List.Item>
        )}
      />
    </Space>
  );
};
