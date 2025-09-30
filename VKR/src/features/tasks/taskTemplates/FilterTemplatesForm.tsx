import { useState, useEffect } from "react";
import {
  Button,
  Form,
  Input,
  Select,
  Modal,
  List,
  Tag,
  Flex,
  Typography,
  DatePicker,
} from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import { Dayjs } from "dayjs";
import { ITaskTemplateFilterOptions } from "../../../api/options/filterOptions/ITaskTemplateFilterOptions";
import { TaskTypeEnum } from "../../../enums/TaskTypeEnum";
import { useGetAllTags } from "../../../api/hooks/tags";
import { TagOwnerEnum } from "../../../enums/ownerEntities/TagOwnerEnum";
import { taskStatuses } from "../../../sync/taskStatuses";
import { taskPriorities } from "../../../sync/taskPriorities";

interface IFilterTemplatesFormProps {
  onFilterApply?: (filters: ITaskTemplateFilterOptions) => void;
  initialFilters?: ITaskTemplateFilterOptions;
  workerId: number;
}

interface IFormValues {
  name?: string;
  taskName?: string;
  description?: string;
  type?: TaskTypeEnum;
  createdFrom?: Dayjs;
  createdTill?: Dayjs;
  startedFrom?: Dayjs;
  startedTill?: Dayjs;
  endDateFrom?: Dayjs;
  endDateTill?: Dayjs;
  status?: string;
  priority?: string;
  tags?: string[];
}

const availableFields = [
  { key: "name", label: "Название шаблона", type: "input" },
  { key: "taskName", label: "Название задачи", type: "input" },
  { key: "description", label: "Описание", type: "input" },
  { key: "type", label: "Тип задачи", type: "select" },
  { key: "status", label: "Статус", type: "select" },
  { key: "priority", label: "Приоритет", type: "select" },
  { key: "tags", label: "Теги", type: "select" },
  { key: "createdFrom", label: "Дата создания от", type: "datePicker" },
  { key: "createdTill", label: "Дата создания до", type: "datePicker" },
  { key: "startedFrom", label: "Дата начала от", type: "datePicker" },
  { key: "startedTill", label: "Дата начала до", type: "datePicker" },
  { key: "endDateFrom", label: "Дата окончания от", type: "datePicker" },
  { key: "endDateTill", label: "Дата окончания до", type: "datePicker" },
];

export const FilterTemplatesForm = ({
  onFilterApply,
  initialFilters,
}: IFilterTemplatesFormProps) => {
  const [form] = Form.useForm();
  const [selectedFields, setSelectedFields] = useState<string[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentFilters, setCurrentFilters] =
    useState<ITaskTemplateFilterOptions>(initialFilters || {});

  const { data: taskTags } = useGetAllTags(TagOwnerEnum.TASK_TEMPLATE);

  const getFormValuesFromFilters = (
    filters: ITaskTemplateFilterOptions
  ): IFormValues => {
    return {
      name: filters.name,
      taskName: filters.taskName,
      description: filters.description,
      type: filters.type,
      createdFrom: filters.createdFrom ? filters.createdFrom : undefined,
      createdTill: filters.createdTill ? filters.createdTill : undefined,
      startedFrom: filters.startedFrom ? filters.startedFrom : undefined,
      startedTill: filters.startedTill ? filters.startedTill : undefined,
      endDateFrom: filters.endDateFrom ? filters.endDateFrom : undefined,
      endDateTill: filters.endDateTill ? filters.endDateTill : undefined,
      status: filters.status?.name,
      priority: filters.priority?.name,
      tags: filters.tags?.map((tag) => tag.name) || [],
    };
  };

  const updateFormFields = (filters: ITaskTemplateFilterOptions) => {
    const filledFields = Object.keys(filters)
      .filter((key) =>
        availableFields.some(
          (field) =>
            field.key === key &&
            filters[key as keyof ITaskTemplateFilterOptions]
        )
      )
      .concat(
        filters.status ? "status" : [],
        filters.priority ? "priority" : [],
        filters.tags?.length ? "tags" : []
      )
      .filter((key, index, self) => self.indexOf(key) === index);

    setSelectedFields(filledFields);
    form.setFieldsValue(getFormValuesFromFilters(filters));
  };

  useEffect(() => {
    if (initialFilters) {
      setCurrentFilters(initialFilters);
      updateFormFields(initialFilters);
    }
  }, [initialFilters, form]);

  const handleAddField = (fieldKey: string) => {
    if (!selectedFields.includes(fieldKey)) {
      setSelectedFields((prev) => [...prev, fieldKey]);
    }
    setIsModalVisible(false);
  };

  const handleResetAll = () => {
    form.resetFields();
    setSelectedFields([]);
    setCurrentFilters({});
    onFilterApply?.({});
  };

  const onFormFinish = (values: IFormValues) => {
    const newFilters: ITaskTemplateFilterOptions = {
      name: values.name,
      taskName: values.taskName,
      description: values.description,
      type: values.type,
      createdFrom: values.createdFrom,
      createdTill: values.createdTill,
      startedFrom: values.startedFrom,
      startedTill: values.startedTill,
      endDateFrom: values.endDateFrom,
      endDateTill: values.endDateTill,
      status: taskStatuses.find((status) => status.name === values.status),
      priority: taskPriorities.find(
        (priority) => priority.name === values.priority
      ),
      tags: values.tags
        ? taskTags
            ?.filter((tag) => values.tags?.includes(tag.name))
            ?.filter(Boolean) || []
        : [],
    };
    setCurrentFilters(newFilters);
    onFilterApply?.(newFilters);
  };

  return (
    <div>
      <Form
        form={form}
        onFinish={onFormFinish}
        initialValues={getFormValuesFromFilters(currentFilters)}
      >
        {selectedFields.map((fieldKey) => {
          const field = availableFields.find((f) => f.key === fieldKey);

          if (!field) return null;

          return (
            <Flex
              key={fieldKey}
              align="center"
              style={{ marginBottom: "1rem" }}
            >
              {(() => {
                switch (field.type) {
                  case "input":
                    return (
                      <Form.Item
                        label={field.label}
                        name={fieldKey as keyof IFormValues}
                        style={{ marginBottom: 0, flex: 1 }}
                      >
                        <Input
                          allowClear
                          placeholder={`Введите ${field.label.toLowerCase()}`}
                        />
                      </Form.Item>
                    );
                  case "select":
                    if (field.key === "type") {
                      return (
                        <Form.Item
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                          style={{ marginBottom: 0, flex: 1 }}
                        >
                          <Select
                            allowClear
                            options={Object.values(TaskTypeEnum).map(
                              (type) => ({
                                value: type,
                                label: type,
                              })
                            )}
                          />
                        </Form.Item>
                      );
                    }
                    if (field.key === "status") {
                      return (
                        <Form.Item
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                          style={{ marginBottom: 0, flex: 1 }}
                        >
                          <Select
                            allowClear
                            options={taskStatuses?.map((status) => ({
                              value: status.name,
                              label: status.name,
                            }))}
                          />
                        </Form.Item>
                      );
                    }
                    if (field.key === "priority") {
                      return (
                        <Form.Item
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                          style={{ marginBottom: 0, flex: 1 }}
                        >
                          <Select
                            allowClear
                            options={taskPriorities?.map((priority) => ({
                              value: priority.name,
                              label: priority.name,
                            }))}
                          />
                        </Form.Item>
                      );
                    }
                    if (field.key === "tags") {
                      return (
                        <Form.Item
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                          style={{ marginBottom: 0, flex: 1 }}
                        >
                          <Select
                            mode="multiple"
                            allowClear
                            options={taskTags?.map((tag) => ({
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
                        label={field.label}
                        name={fieldKey as keyof IFormValues}
                        style={{ marginBottom: 0, flex: 1 }}
                      >
                        <DatePicker style={{ width: "100%" }} allowClear />
                      </Form.Item>
                    );
                  default:
                    return null;
                }
              })()}
            </Flex>
          );
        })}

        <Flex gap={"1rem"} justify="center">
          <Button onClick={() => setIsModalVisible(true)}>Добавить поле</Button>
          <Form.Item style={{ margin: 0 }}>
            <Button htmlType="submit" type="primary">
              Применить фильтры
            </Button>
          </Form.Item>
          <Button icon={<DeleteOutlined />} onClick={handleResetAll} danger>
            Сбросить все фильтры
          </Button>
        </Flex>
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
