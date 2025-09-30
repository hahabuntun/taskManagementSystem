import {
  Button,
  Checkbox,
  Divider,
  Flex,
  Form,
  Input,
  Table,
  TableProps,
  Typography,
} from "antd";
import { useEffect, useState } from "react";
import { IWorkerPosition } from "../../../interfaces/IWorkerPosition";
import { IEditWorkerPositionOptions } from "../../../api/options/editOptions/IEditWorkerPositionOptions";
import {
  useEditWorkerPosition,
  useGetWorkerPosition,
  useGetWorkerPositions,
} from "../../../api/hooks/workerPositions";

const { Text } = Typography;

interface IEditPositionFormProps {
  position: IWorkerPosition;
}

export const EditPositionForm = ({ position }: IEditPositionFormProps) => {
  const [form] = Form.useForm();
  const editAsync = useEditWorkerPosition();
  const { data: currentPosition, isLoading: isPositionLoading } =
    useGetWorkerPosition(position.id);
  const { data: allPositions, isLoading: isPositionsLoading } =
    useGetWorkerPositions();

  const [options, setOptions] = useState<IEditWorkerPositionOptions>({
    title: position.title,
    canAssignTasksToIds: [],
    canTakeTasksFromIds: [],
  });

  useEffect(() => {
    if (currentPosition) {
      const newOptions = {
        title: currentPosition.title,
        canAssignTasksToIds:
          currentPosition.canAssignTasksTo.map((p) => p.id) || [],
        canTakeTasksFromIds:
          currentPosition.canTakeTasksFrom.map((p) => p.id) || [],
      };
      setOptions(newOptions);
      form.setFieldsValue({
        title: currentPosition.title,
      });
      console.log("Updated options:", newOptions);
    }
  }, [currentPosition, form]);

  // Function to toggle "Ставить задачи" (Assign Tasks) permission
  const toggleAssignTasks = (pos: IWorkerPosition) => {
    setOptions((prev) => {
      const newIds = prev.canAssignTasksToIds.includes(pos.id)
        ? prev.canAssignTasksToIds.filter((id) => id !== pos.id)
        : [...prev.canAssignTasksToIds, pos.id];
      console.log("Toggled canAssignTasksToIds:", newIds);
      return { ...prev, canAssignTasksToIds: newIds };
    });
  };

  // Function to toggle "Получать задачи" (Receive Tasks) permission
  const toggleReceiveTasks = (pos: IWorkerPosition) => {
    setOptions((prev) => {
      const newIds = prev.canTakeTasksFromIds.includes(pos.id)
        ? prev.canTakeTasksFromIds.filter((id) => id !== pos.id)
        : [...prev.canTakeTasksFromIds, pos.id];
      console.log("Toggled canTakeTasksFromIds:", newIds);
      return { ...prev, canTakeTasksFromIds: newIds };
    });
  };

  const handleSubmit = () => {
    form
      .validateFields()
      .then((values) => {
        const updatedOptions: IEditWorkerPositionOptions = {
          title: values.title,
          canAssignTasksToIds: options.canAssignTasksToIds,
          canTakeTasksFromIds: options.canTakeTasksFromIds,
        };
        console.log("Submitting:", {
          workerPositionId: position.id,
          options: updatedOptions,
        });
        editAsync(
          { workerPositionId: position.id, options: updatedOptions },
          {
            onSuccess: () => {
              console.log("Position updated successfully");
              form.resetFields();
            },
            onError: (error: any) => {
              console.error("Update error:", error);
              form.setFields([
                {
                  name: "title",
                  errors: [error.message || "Failed to update position"],
                },
              ]);
            },
          }
        );
      })
      .catch((info) => {
        console.log("Validation Failed:", info);
      });
  };

  const columns: TableProps<IWorkerPosition>["columns"] = [
    {
      title: "Разрешения",
      key: "permissions",
      render: (_, record) => (
        <Flex key={record.id} vertical>
          <Checkbox
            checked={options.canAssignTasksToIds.includes(record.id)}
            onChange={() => toggleAssignTasks(record)}
            disabled={isPositionLoading || isPositionsLoading}
          >
            Ставить задачи должности
          </Checkbox>
          <Checkbox
            checked={options.canTakeTasksFromIds.includes(record.id)}
            onChange={() => toggleReceiveTasks(record)}
            disabled={isPositionLoading || isPositionsLoading}
          >
            Получать задачи от должности
          </Checkbox>
        </Flex>
      ),
    },
    {
      title: "Должность",
      key: "id",
      render: (_, record) => <Text>{record.title}</Text>,
    },
  ];

  return (
    <Form form={form} layout="vertical" onFinish={handleSubmit}>
      <Form.Item
        name="title"
        label="Название должности"
        rules={[
          { required: true, message: "Введите название должности" },
          {
            max: 100,
            message: "Название должности не должно превышать 100 символов",
          },
        ]}
      >
        <Input placeholder="Название должности" disabled={isPositionLoading} />
      </Form.Item>

      <Divider />

      <Table
        rowKey="id"
        style={{ maxHeight: "75vh", overflowY: "auto", marginBottom: "1rem" }}
        columns={columns}
        dataSource={allPositions}
        pagination={false}
        loading={isPositionLoading || isPositionsLoading}
      />

      <Form.Item>
        <Button
          type="primary"
          htmlType="submit"
          style={{ width: "100%" }}
          disabled={isPositionLoading || isPositionsLoading}
        >
          Изменить
        </Button>
      </Form.Item>
    </Form>
  );
};
