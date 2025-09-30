// components/FilterTasksForm.tsx
import { useState, useEffect } from "react";
import {
  Button,
  DatePicker,
  Form,
  Input,
  Select,
  Modal,
  List,
  Tag,
  Flex,
  Popover,
  notification,
} from "antd";
import { CloseOutlined, DeleteOutlined } from "@ant-design/icons";
import { useGetAllTags } from "../../api/hooks/tags";
import { useGetWorkers } from "../../api/hooks/workers";
import { WorkerAvatar } from "../../components/WorkerAvatar";
import dayjs, { Dayjs } from "dayjs";
import { TaskTypeEnum } from "../../enums/TaskTypeEnum";
import { ITaskFilterOptions } from "../../api/options/filterOptions/ITaskFilterOptions";
import { IWorker } from "../../interfaces/IWorker";
import { TagOwnerEnum } from "../../enums/ownerEntities/TagOwnerEnum";
import { taskStatuses } from "../../sync/taskStatuses";
import { taskPriorities } from "../../sync/taskPriorities";
import { ITaskFilter } from "../../interfaces/ITaskFilter";
import { useAddTaskFilter } from "../../api/hooks/tasks";
import { SavedFilters } from "./SavedFilter";

interface IFilterTasksFormProps {
  onFilterApply?: (filters: ITaskFilterOptions) => void;
  initialFilters?: ITaskFilterOptions;
  workerId: number;
}

interface IFormValues {
  name?: string;
  description?: string;
  type?: TaskTypeEnum;
  startedFrom?: Dayjs;
  startedTill?: Dayjs;
  createdFrom?: Dayjs;
  createdTill?: Dayjs;
  endDateFrom?: Dayjs;
  endDateTill?: Dayjs;
  status?: string;
  priority?: string;
  creatorId?: number;
  tags?: string[];
}

const availableFields = [
  { key: "name", label: "Название задачи", type: "input" },
  { key: "description", label: "Описание", type: "input" },
  { key: "type", label: "Тип задачи", type: "select" },
  { key: "status", label: "Статус", type: "select" },
  { key: "priority", label: "Приоритет", type: "select" },
  { key: "creatorId", label: "Создатель", type: "select" },
  { key: "tags", label: "Теги", type: "select" },
  { key: "createdFrom", label: "Дата создания от", type: "datePicker" },
  { key: "createdTill", label: "Дата создания до", type: "datePicker" },
  { key: "startedFrom", label: "Дата начала от", type: "datePicker" },
  { key: "startedTill", label: "Дата начала до", type: "datePicker" },
  { key: "endDateFrom", label: "Дата окончания от", type: "datePicker" },
  { key: "endDateTill", label: "Дата окончания до", type: "datePicker" },
];

export const FilterTasksForm = ({
  onFilterApply,
  initialFilters,
  workerId,
}: IFilterTasksFormProps) => {
  const [form] = Form.useForm();
  const [selectedFields, setSelectedFields] = useState<string[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [currentFilters, setCurrentFilters] = useState<ITaskFilterOptions>(
    initialFilters || {}
  );
  const [filterName, setFilterName] = useState<string>("");
  const [isSavePopoverVisible, setIsSavePopoverVisible] =
    useState<boolean>(false);

  const { data: taskTags } = useGetAllTags(TagOwnerEnum.TASK);
  const { data: workers } = useGetWorkers();
  const saveFilterAsync = useAddTaskFilter(() => {
    setIsSavePopoverVisible(false);
    setFilterName("");
  });

  // Преобразование фильтров в формат формы
  const getFormValuesFromFilters = (
    filters: ITaskFilterOptions
  ): IFormValues => {
    return {
      name: filters.name,
      description: filters.description,
      type: filters.type,
      createdFrom: filters.createdFrom ? dayjs(filters.createdFrom) : undefined,
      createdTill: filters.createdTill ? dayjs(filters.createdTill) : undefined,
      startedFrom: filters.startedFrom ? dayjs(filters.startedFrom) : undefined,
      startedTill: filters.startedTill ? dayjs(filters.startedTill) : undefined,
      endDateFrom: filters.endDateFrom ? dayjs(filters.endDateFrom) : undefined,
      endDateTill: filters.endDateTill ? dayjs(filters.endDateTill) : undefined,
      status: filters.status?.name,
      priority: filters.priority?.name,
      creatorId: filters.creator?.id,
      tags: filters.tags?.map((tag) => tag.name) || [],
    };
  };

  // Установка полей и значений формы на основе фильтров
  const updateFormFields = (filters: ITaskFilterOptions) => {
    const filledFields = Object.keys(filters)
      .filter((key) =>
        availableFields.some(
          (field) =>
            field.key === key && filters[key as keyof ITaskFilterOptions]
        )
      )
      .concat(
        filters.status ? "status" : [],
        filters.priority ? "priority" : [],
        filters.creator ? "creatorId" : [],
        filters.tags?.length ? "tags" : []
      )
      .filter((key, index, self) => self.indexOf(key) === index);

    setSelectedFields(filledFields);
    form.setFieldsValue(getFormValuesFromFilters(filters));
  };

  // Обновление формы при изменении initialFilters
  useEffect(() => {
    if (initialFilters) {
      setCurrentFilters(initialFilters);
      updateFormFields(initialFilters);
    }
  }, [initialFilters, form]);

  // Обработчик добавления поля
  const handleAddField = (fieldKey: string) => {
    if (!selectedFields.includes(fieldKey)) {
      setSelectedFields((prev) => [...prev, fieldKey]);
    }
    setIsModalVisible(false);
  };

  // Обработчик удаления поля
  const handleRemoveField = (fieldKey: string) => {
    setSelectedFields((prev) => prev.filter((key) => key !== fieldKey));
    form.setFieldValue(fieldKey, undefined);
    const updatedFilters = { ...currentFilters };
    delete updatedFilters[fieldKey as keyof ITaskFilterOptions];
    setCurrentFilters(updatedFilters);
  };

  // Обработчик сброса фильтров
  const handleResetFilters = () => {
    form.resetFields();
    setSelectedFields([]);
    setCurrentFilters({});
    onFilterApply?.({});
  };

  // Обработчик отправки формы
  const onFormFinish = (values: IFormValues) => {
    const newFilters: ITaskFilterOptions = {
      name: values.name,
      description: values.description,
      type: values.type,
      createdFrom: values.createdFrom ? dayjs(values.createdFrom) : undefined,
      createdTill: values.createdTill ? dayjs(values.createdTill) : undefined,
      startedFrom: values.startedFrom ? dayjs(values.startedFrom) : undefined,
      startedTill: values.startedTill ? dayjs(values.startedTill) : undefined,
      endDateFrom: values.endDateFrom ? dayjs(values.endDateFrom) : undefined,
      endDateTill: values.endDateTill ? dayjs(values.endDateTill) : undefined,
      status: taskStatuses.find((status) => status.name === values.status),
      priority: taskPriorities.find(
        (priority) => priority.name === values.priority
      ),
      creator: workers?.find(
        (worker: IWorker) => worker.id === values.creatorId
      ),
      tags: values.tags
        ? taskTags
            ?.filter((tag) => values.tags?.includes(tag.name))
            ?.filter(Boolean) || []
        : [],
    };
    setCurrentFilters(newFilters);
    onFilterApply?.(newFilters);
    form.setFieldsValue(values); // Persist form values after submission
  };

  // Обработчик выбора сохранённого фильтра
  const handleFilterSelect = (filter: ITaskFilter) => {
    setCurrentFilters(filter.options);
    updateFormFields(filter.options);
    onFilterApply?.(filter.options);
  };

  // Обработчик сохранения фильтра
  const handleSaveFilter = () => {
    if (!filterName) {
      notification.error({ message: "Введите название фильтра" });
      return;
    }

    const formValues = form.getFieldsValue();
    const newFilters: ITaskFilterOptions = {
      name: formValues.name,
      description: formValues.description,
      type: formValues.type,
      createdFrom: formValues.createdFrom
        ? dayjs(formValues.createdFrom)
        : undefined,
      createdTill: formValues.createdTill
        ? dayjs(formValues.createdTill)
        : undefined,
      startedFrom: formValues.startedFrom
        ? dayjs(formValues.startedFrom)
        : undefined,
      startedTill: formValues.startedTill
        ? dayjs(formValues.startedTill)
        : undefined,
      endDateFrom: formValues.endDateFrom
        ? dayjs(formValues.endDateFrom)
        : undefined,
      endDateTill: formValues.endDateTill
        ? dayjs(formValues.endDateTill)
        : undefined,
      status: taskStatuses.find((status) => status.name === formValues.status),
      priority: taskPriorities.find(
        (priority) => priority.name === formValues.priority
      ),
      creator: workers?.find(
        (worker: IWorker) => worker.id === formValues.creatorId
      ),
      tags: formValues.tags
        ? taskTags
            ?.filter((tag) => formValues.tags.includes(tag.name))
            ?.filter(Boolean) || []
        : [],
    };

    if (Object.keys(newFilters).length === 0) {
      notification.error({
        message: "Выберите хотя бы один фильтр для сохранения",
      });
      return;
    }

    console.log("Saving filter:", {
      workerId,
      filterName,
      options: newFilters,
    });
    saveFilterAsync({
      filterName,
      options: newFilters,
    });
  };

  // Содержимое Popover для сохранения фильтра
  const saveFilterPopoverContent = (
    <Flex gap="0.5rem">
      <Input
        style={{ minWidth: "300px" }}
        placeholder="Введите название фильтра"
        value={filterName}
        onChange={(e) => setFilterName(e.target.value)}
        onPressEnter={handleSaveFilter}
      />
      <Button type="primary" onClick={handleSaveFilter} disabled={!filterName}>
        Сохранить
      </Button>
    </Flex>
  );

  return (
    <div>
      <SavedFilters onFilterSelect={handleFilterSelect} />

      <Form form={form} onFinish={onFormFinish}>
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
                    if (field.key === "creatorId") {
                      return (
                        <Form.Item
                          label={field.label}
                          name={fieldKey as keyof IFormValues}
                          style={{ marginBottom: 0, flex: 1 }}
                        >
                          <Select
                            showSearch
                            allowClear
                            options={workers?.map((worker: IWorker) => ({
                              value: worker.id,
                              label: (
                                <WorkerAvatar size="small" worker={worker} />
                              ),
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
                              label: <Tag color={tag.color}>{tag.name}</Tag>,
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
              <Button
                type="link"
                danger
                icon={<CloseOutlined />}
                onClick={() => handleRemoveField(fieldKey)}
                style={{ marginLeft: "8px" }}
              />
            </Flex>
          );
        })}
        <Flex gap={"1rem"} wrap justify="center">
          <Button
            style={{ width: "150px" }}
            onClick={() => setIsModalVisible(true)}
          >
            Добавить поле
          </Button>
          <Popover
            trigger="click"
            content={saveFilterPopoverContent}
            open={isSavePopoverVisible}
            onOpenChange={(visible) => {
              setIsSavePopoverVisible(visible);
              if (!visible) setFilterName("");
            }}
          >
            <Button style={{ width: "150px" }}>Сохранить фильтр</Button>
          </Popover>
          <Form.Item style={{ margin: 0 }}>
            <Button style={{ width: "150px" }} htmlType="submit" type="primary">
              Применить фильтры
            </Button>
          </Form.Item>
          <Button
            style={{ width: "150px" }}
            onClick={handleResetFilters}
            danger
            icon={<DeleteOutlined />}
          >
            Сбросить фильтры
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
