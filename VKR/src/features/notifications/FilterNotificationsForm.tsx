import { useState } from "react";
import { INotificationFilterOptions } from "../../api/options/filterOptions/INotificationFilterOptions";
import {
  Button,
  DatePicker,
  Form,
  Input,
  Select,
  Modal,
  List,
  Switch,
} from "antd";
import { WorkerAvatar } from "../../components/WorkerAvatar";
import dayjs, { Dayjs } from "dayjs";
import { NotificationOwnerEnum } from "../../enums/ownerEntities/NotificationOwnerEnum";
import { IWorkerFields } from "../../interfaces/IWorkerFields";
import { DeleteOutlined } from "@ant-design/icons";

interface IFilterNotificationsFormProps {
  onFilterApply: (filters: INotificationFilterOptions) => void;
  availableWorkers: IWorkerFields[];
}

interface IFormValues {
  type?: NotificationOwnerEnum;
  name?: string;
  message?: string;
  createdFrom?: Dayjs;
  createdTill?: Dayjs;
  isRead?: boolean;
  responsibleWorkerId?: number;
}

const availableFields = [
  { key: "type", label: "Тип уведомления", type: "select" },
  { key: "name", label: "Название сущности", type: "input" },
  { key: "message", label: "Сообщение", type: "input" },
  { key: "createdFrom", label: "Создано от", type: "datePicker" },
  { key: "createdTill", label: "Создано до", type: "datePicker" },
  { key: "isRead", label: "Прочитано", type: "switch" },
  { key: "responsibleWorkerId", label: "Ответственный", type: "select" },
];

export const FilterNotificationsForm = ({
  onFilterApply,
  availableWorkers,
}: IFilterNotificationsFormProps) => {
  const [form] = Form.useForm();
  const [selectedFields, setSelectedFields] = useState<string[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);

  const handleAddField = (fieldKey: string) => {
    if (!selectedFields.includes(fieldKey)) {
      setSelectedFields((prev) => [...prev, fieldKey]);
    }
    setIsModalVisible(false);
  };

  const handleResetAll = () => {
    form.resetFields();
    setSelectedFields([]);
    onFilterApply({});
  };

  const onFormFinish = (values: IFormValues) => {
    const newFilters: INotificationFilterOptions = {
      type: values.type,
      name: values.name,
      message: values.message,
      createdFrom: values.createdFrom ? dayjs(values.createdFrom) : undefined,
      createdTill: values.createdTill ? dayjs(values.createdTill) : undefined,
      isRead: values.isRead,
      responsibleWorker: values.responsibleWorkerId
        ? availableWorkers.find(
            (worker) => worker.id === values.responsibleWorkerId
          )
        : undefined,
    };
    onFilterApply(newFilters);
  };

  return (
    <div>
      <Form
        form={form}
        style={{ marginBottom: "1rem" }}
        onFinish={onFormFinish}
        initialValues={{}}
      >
        {selectedFields.map((fieldKey) => {
          const field = availableFields.find((f) => f.key === fieldKey);

          if (!field) return null;

          return (
            <div
              key={fieldKey}
              style={{
                display: "flex",
                alignItems: "center",
                marginBottom: "1rem",
              }}
            >
              {(() => {
                switch (field.type) {
                  case "input":
                    return (
                      <Form.Item
                        style={{ marginBottom: "0", flex: 1 }}
                        label={field.label}
                        name={fieldKey as keyof IFormValues}
                      >
                        <Input allowClear />
                      </Form.Item>
                    );
                  case "select":
                    if (field.key === "type") {
                      return (
                        <Form.Item
                          style={{ marginBottom: "0", flex: 1 }}
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                        >
                          <Select
                            allowClear
                            options={Object.values(NotificationOwnerEnum).map(
                              (value) => ({
                                value,
                                label: value,
                              })
                            )}
                          />
                        </Form.Item>
                      );
                    }
                    if (field.key === "responsibleWorkerId") {
                      return (
                        <Form.Item
                          style={{ marginBottom: "0", flex: 1 }}
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                        >
                          <Select
                            showSearch
                            allowClear
                            options={availableWorkers.map((worker) => ({
                              value: worker.id,
                              label: (
                                <WorkerAvatar size="small" worker={worker} />
                              ),
                            }))}
                          />
                        </Form.Item>
                      );
                    }
                    return null;
                  case "datePicker":
                    return (
                      <Form.Item
                        style={{ marginBottom: "0", flex: 1 }}
                        label={field.label}
                        name={fieldKey as keyof IFormValues}
                      >
                        <DatePicker style={{ width: "100%" }} allowClear />
                      </Form.Item>
                    );
                  case "switch":
                    return (
                      <Form.Item
                        style={{ marginBottom: "0", flex: 1 }}
                        label={field.label}
                        name={fieldKey as keyof IFormValues}
                        valuePropName="checked"
                      >
                        <Switch />
                      </Form.Item>
                    );
                  default:
                    return null;
                }
              })()}
            </div>
          );
        })}

        <Button
          style={{ width: "100%", marginBottom: "1rem" }}
          onClick={() => setIsModalVisible(true)}
        >
          Добавить поле
        </Button>

        <div style={{ display: "flex", gap: "1rem", justifyContent: "center" }}>
          <Form.Item style={{ marginBottom: "1rem", margin: 0 }}>
            <Button style={{ width: "100%" }} htmlType="submit" type="primary">
              Применить фильтры
            </Button>
          </Form.Item>
          <Button
            icon={<DeleteOutlined />}
            style={{ marginBottom: "1rem" }}
            onClick={handleResetAll}
            danger
          >
            Сбросить все фильтры
          </Button>
        </div>
      </Form>

      <Modal
        title="Добавить поле"
        open={isModalVisible}
        onCancel={() => setIsModalVisible(false)}
        footer={null}
      >
        <List
          dataSource={availableFields.filter(
            (field) => !selectedFields.includes(field.key)
          )}
          renderItem={(field) => (
            <List.Item
              actions={[
                <Button
                  key="add"
                  type="link"
                  onClick={() => handleAddField(field.key)}
                >
                  Добавить
                </Button>,
              ]}
            >
              {field.label}
            </List.Item>
          )}
        />
      </Modal>
    </div>
  );
};
