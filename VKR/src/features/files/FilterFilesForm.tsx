import { useState } from "react";
import { IFileFilterOptions } from "../../api/options/filterOptions/IFileFilterOptions";
import { Button, DatePicker, Form, Input, Select, Modal, List } from "antd";
import { useGetAvailableResponsibleWorkerForFilesFiltering } from "../../api/hooks/workers";
import { WorkerAvatar } from "../../components/WorkerAvatar";

import dayjs, { Dayjs } from "dayjs";
import { FileOwnerEnum } from "../../enums/ownerEntities/FileOwnerEnum";

interface IFilterFilesFormProps {
  onFilterApply: (filters: IFileFilterOptions) => void;
  ownerType: FileOwnerEnum;
  ownerId: number;
}

interface IFormValues {
  name?: string;
  workerId?: number;
  email?: string;
  createdFrom?: Dayjs;
  createdTill?: Dayjs;
}

const availableFields = [
  { key: "name", label: "Название", type: "input" },
  { key: "workerId", label: "Создатель", type: "select" },
  { key: "createdFrom", label: "От какого числа добавлен", type: "datePicker" },
  { key: "createdTill", label: "До какого числа добавлен", type: "datePicker" },
];

export const FilterFilesForm = ({
  ownerId,
  ownerType,
  onFilterApply,
}: IFilterFilesFormProps) => {
  const [form] = Form.useForm();
  const [selectedFields, setSelectedFields] = useState<string[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const { data: workers } = useGetAvailableResponsibleWorkerForFilesFiltering(
    ownerType,
    ownerId
  );

  const handleAddField = (fieldKey: string) => {
    if (!selectedFields.includes(fieldKey)) {
      setSelectedFields((prev) => [...prev, fieldKey]);
    }
    setIsModalVisible(false);
  };

  const onFormFinish = (values: IFormValues) => {
    const newFilters: IFileFilterOptions = {
      creator: workers?.find((worker) => worker.id === values.workerId),
      name: values.name,
      createdFrom: values.createdFrom ? dayjs(values.createdFrom) : undefined,
      createdTill: values.createdTill ? dayjs(values.createdTill) : undefined,
    };
    onFilterApply(newFilters);
  };

  return (
    <div>
      {/* Form */}
      <Form
        form={form}
        style={{ marginBottom: "1rem" }}
        onFinish={onFormFinish}
        initialValues={{}}
      >
        {/* Dynamically render selected fields */}
        {selectedFields.map((fieldKey) => {
          const field = availableFields.find((f) => f.key === fieldKey);

          if (!field) return null;

          switch (field.type) {
            case "input":
              return (
                <Form.Item
                  key={fieldKey}
                  style={{ marginBottom: "1rem" }}
                  label={field.label}
                  name={fieldKey as keyof IFormValues}
                >
                  <Input allowClear />
                </Form.Item>
              );
            case "select":
              return (
                <Form.Item
                  key={fieldKey}
                  style={{ marginBottom: "1rem" }}
                  label={field.label}
                  name={fieldKey as keyof IFormValues}
                >
                  <Select
                    showSearch
                    allowClear
                    options={workers?.map((worker) => ({
                      value: worker.id,
                      label: <WorkerAvatar size="small" worker={worker} />,
                    }))}
                  />
                </Form.Item>
              );
            case "datePicker":
              return (
                <Form.Item
                  key={fieldKey}
                  style={{ marginBottom: "1rem" }}
                  label={field.label}
                  name={fieldKey as keyof IFormValues}
                >
                  <DatePicker style={{ width: "100%" }} allowClear />
                </Form.Item>
              );
            default:
              return null;
          }
        })}

        {/* Add Field Button */}
        <Button
          style={{ width: "100%", marginBottom: "1rem" }}
          onClick={() => setIsModalVisible(true)}
        >
          Добавить поле
        </Button>

        {/* Submit Button */}
        <Form.Item style={{ marginBottom: "1rem" }}>
          <Button style={{ width: "100%" }} htmlType="submit" type="primary">
            Применить фильтры
          </Button>
        </Form.Item>
      </Form>

      {/* Modal for Field Selection */}
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
