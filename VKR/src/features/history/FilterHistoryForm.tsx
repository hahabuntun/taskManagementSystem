import { Button, DatePicker, Form, Input, List, Modal, Select } from "antd";
import { useState } from "react";
import { WorkerAvatar } from "../../components/WorkerAvatar";
import { IHistoryFilterOptions } from "../../api/options/filterOptions/IHistoryFilterOptions";
import { useGetAvailableResponsibleWorkerForHistoryFiltering } from "../../api/hooks/workers";

import dayjs, { Dayjs } from "dayjs";
import { HistoryOwnerEnum } from "../../enums/ownerEntities/HistoryOwnerEnum";
import { IWorkerFields } from "../../interfaces/IWorkerFields";

interface IFilterHistoryFormProps {
  onFilterApply: (filters: IHistoryFilterOptions) => void;
  ownerType: HistoryOwnerEnum;
  ownerId: number;
}

interface IFormValues {
  text?: string;
  workerId?: number;
  createdFrom?: Dayjs;
  createdTill?: Dayjs;
}

const availableFields = [
  { key: "text", label: "Название", type: "input" },
  { key: "workerId", label: "Ответственный", type: "select" },
  { key: "createdFrom", label: "От какого числа", type: "datePicker" },
  { key: "createdTill", label: "До какого числа", type: "datePicker" },
];

export const FilterHistoryForm = ({
  ownerId,
  ownerType,
  onFilterApply,
}: IFilterHistoryFormProps) => {
  const [form] = Form.useForm();
  const [selectedFields, setSelectedFields] = useState<string[]>([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const { data: workers } = useGetAvailableResponsibleWorkerForHistoryFiltering(
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
    const newFilters: IHistoryFilterOptions = {
      responsibleWorker: workers?.find(
        (worker: IWorkerFields) => worker.id === values.workerId
      ),
      text: values.text,
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
