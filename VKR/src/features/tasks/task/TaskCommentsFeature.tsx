import {
  Button,
  Divider,
  Flex,
  Form,
  Input,
  List,
  Space,
  Typography,
} from "antd";
import { WorkerAvatar } from "../../../components/WorkerAvatar";
import {
  useAddTaskComment,
  useDeleteTaskComment,
  useGetTaskComments,
} from "../../../api/hooks/tasks";
import { IAddTaskCommentOptions } from "../../../api/options/createOptions/IAddTaskCommentOptions";
import { DeleteButton } from "../../../components/buttons/DeleteButton";
import useApplicationStore from "../../../stores/applicationStore";

// Интерфейс работника
interface IWorker {
  id: number;
  firstName: string;
  lastName: string;
  avatarUrl?: string; // Опционально для аватара
}

// Интерфейс пропсов компонента
interface TaskCommentsSectionProps {
  taskId: number; // ID задачи для загрузки комментариев
  currentUser: IWorker; // Текущий пользователь для добавления комментариев
}

// Компонент секции комментариев
export const TaskCommentsFeature = ({ taskId }: TaskCommentsSectionProps) => {
  const [form] = Form.useForm();

  const { data: items, refetch } = useGetTaskComments(taskId);

  const { user } = useApplicationStore.getState();

  const addAsync = useAddTaskComment(() => {
    refetch();
    form.resetFields();
  });

  const deleteAsync = useDeleteTaskComment(() => {
    refetch();
  });

  // Обработчик отправки формы
  const onFinish = (values: { text: string }) => {
    const options: IAddTaskCommentOptions = {
      text: values.text,
    };
    if (user) {
      addAsync({ taskId: taskId, options, creator: user });
    }
  };

  return (
    <Flex vertical>
      {/* Заголовок */}
      <Typography.Title
        level={4}
        style={{ marginBottom: "16px", padding: "0 16px" }}
      >
        Комментарии
      </Typography.Title>

      {/* Контейнер для списка и формы */}
      <Flex vertical>
        <div
          style={{
            padding: "0 16px",
          }}
        >
          <List
            dataSource={items}
            pagination={{
              pageSize: 10,
            }}
            locale={{ emptyText: "Нет комментариев" }}
            renderItem={(comment) => (
              <List.Item
                style={{
                  padding: "12px 0",
                  borderBottom: "1px solid #f0f0f0",
                }}
                actions={[
                  <DeleteButton
                    onClick={() => deleteAsync({ comment: comment })}
                    itemId={comment.id}
                    text="Удалить"
                  />,
                ]}
              >
                <Space direction="vertical" style={{ width: "100%" }}>
                  <div style={{ display: "flex", alignItems: "center" }}>
                    <WorkerAvatar size="small" worker={user!} />
                    <div style={{ marginLeft: "12px" }}>
                      <Typography.Text strong>
                        {`${comment.creator.firstName} ${comment.creator.secondName}`}
                      </Typography.Text>
                      <Typography.Text
                        type="secondary"
                        style={{ marginLeft: "8px", fontSize: "12px" }}
                      >
                        {comment.createdAt.format("DD.MM.YYYY HH:mm")}
                      </Typography.Text>
                    </div>
                  </div>
                  <Typography.Paragraph style={{ margin: "4px 0 0 52px" }}>
                    {comment.text}
                  </Typography.Paragraph>
                </Space>
              </List.Item>
            )}
          />
        </div>

        <Divider />

        <div
          style={{
            padding: "16px",
          }}
        >
          <Form form={form} onFinish={onFinish} layout="vertical">
            <Form.Item
              name="text"
              rules={[{ required: true, message: "Введите текст комментария" }]}
            >
              <Input.TextArea
                rows={4}
                placeholder="Напишите комментарий..."
                style={{ borderRadius: "6px" }}
              />
            </Form.Item>
            <Form.Item>
              <Button
                type="primary"
                htmlType="submit"
                style={{ borderRadius: "6px", width: "100%" }}
              >
                Добавить комментарий
              </Button>
            </Form.Item>
          </Form>
        </div>
      </Flex>
    </Flex>
  );
};
