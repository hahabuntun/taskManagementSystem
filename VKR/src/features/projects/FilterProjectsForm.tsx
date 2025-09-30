import { useState } from "react";
import { IProjectFilterOptions } from "../../api/options/filterOptions/IProjectFilterOptions";
import {
  Button,
  DatePicker,
  Form,
  Input,
  Select,
  Modal,
  List,
  Tag,
  Typography,
} from "antd";
import { WorkerAvatar } from "../../components/WorkerAvatar";
import dayjs, { Dayjs } from "dayjs";
import { useGetAllTags } from "../../api/hooks/tags";
import { IWorker } from "../../interfaces/IWorker";
import { useGetManagers } from "../../api/hooks/workers";
import { TagOwnerEnum } from "../../enums/ownerEntities/TagOwnerEnum";
import { projectStatuses } from "../../sync/projectStatuses";
import { DeleteOutlined } from "@ant-design/icons";

interface IFilterProjectsFormProps {
  onFilterApply?: (filters: IProjectFilterOptions) => void;
}

interface IFormValues {
  name?: string;
  status?: string;
  managerId?: number;
  tags?: string[];
  createdFrom?: Dayjs;
  createdTill?: Dayjs;
  startedFrom?: Dayjs;
  startedTill?: Dayjs;
  endDateFrom?: Dayjs;
  endDateTill?: Dayjs;
}

const availableFields = [
  { key: "name", label: "Название проекта", type: "input" },
  { key: "status", label: "Статус", type: "select" },
  { key: "managerId", label: "Менеджер", type: "select" },
  { key: "tags", label: "Теги", type: "select" },
  { key: "createdFrom", label: "Дата создания от", type: "datePicker" },
  { key: "createdTill", label: "Дата создания до", type: "datePicker" },
  { key: "startedFrom", label: "Дата начала от", type: "datePicker" },
  { key: "startedTill", label: "Дата начала до", type: "datePicker" },
  { key: "endDateFrom", label: "Дата окончания от", type: "datePicker" },
  { key: "endDateTill", label: "Дата окончания до", type: "datePicker" },
];

export const FilterProjectsForm = ({
  onFilterApply,
}: IFilterProjectsFormProps) => {
  const { data: projectTags } = useGetAllTags(TagOwnerEnum.PROJECT);
  const [form] = Form.useForm();
  const [selectedFields, setSelectedFields] = useState<string[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const { data: managers } = useGetManagers();

  const handleAddField = (fieldKey: string) => {
    if (!selectedFields.includes(fieldKey)) {
      setSelectedFields((prev) => [...prev, fieldKey]);
    }
    setIsModalVisible(false);
  };

  const handleResetAll = () => {
    form.resetFields();
    setSelectedFields([]);
    onFilterApply?.({});
  };

  const onFormFinish = (values: IFormValues) => {
    const newFilters: IProjectFilterOptions = {
      name: values.name,
      status: projectStatuses.find((status) => status.name === values.status),
      manager: managers?.find(
        (manager: IWorker) => manager.id === values.managerId
      ),
      tags: values.tags
        ?.map((tagName) => projectTags?.find((tag) => tag.name === tagName))
        ?.filter((tag) => tag !== undefined),
      createdFrom: values.createdFrom ? dayjs(values.createdFrom) : undefined,
      createdTill: values.createdTill ? dayjs(values.createdTill) : undefined,
      startedFrom: values.startedFrom ? dayjs(values.startedFrom) : undefined,
      startedTill: values.startedTill ? dayjs(values.startedTill) : undefined,
      endDateFrom: values.endDateFrom ? dayjs(values.endDateFrom) : undefined,
      endDateTill: values.endDateTill ? dayjs(values.endDateTill) : undefined,
    };
    onFilterApply?.(newFilters);
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
                        <Input allowClear placeholder="Введите название" />
                      </Form.Item>
                    );
                  case "select":
                    if (field.key === "status") {
                      return (
                        <Form.Item
                          style={{ marginBottom: "0", flex: 1 }}
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                        >
                          <Select
                            allowClear
                            options={projectStatuses?.map((status) => ({
                              value: status.name,
                              label: status.name,
                            }))}
                          />
                        </Form.Item>
                      );
                    }
                    if (field.key === "managerId") {
                      return (
                        <Form.Item
                          style={{ marginBottom: "0", flex: 1 }}
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                        >
                          <Select
                            showSearch
                            allowClear
                            options={managers?.map((manager) => ({
                              value: manager.id,
                              label: (
                                <WorkerAvatar size="small" worker={manager} />
                              ),
                            }))}
                          />
                        </Form.Item>
                      );
                    }
                    if (field.key === "tags") {
                      return (
                        <Form.Item
                          style={{ marginBottom: "0", flex: 1 }}
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                        >
                          <Select
                            mode="multiple"
                            allowClear
                            options={projectTags?.map((tag) => ({
                              value: tag.name,
                              label: (
                                <Tag color={tag.color}>
                                  <Typography.Text style={{ color: "black" }}>
                                    {tag.name}
                                  </Typography.Text>
                                </Tag>
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
        title="Добавить поле фильтра"
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
