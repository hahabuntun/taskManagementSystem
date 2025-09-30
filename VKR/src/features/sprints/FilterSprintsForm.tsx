import { useState } from "react";
import { Button, DatePicker, Form, Input, Select, Modal, List } from "antd";
import dayjs, { Dayjs } from "dayjs";
import isSameOrBefore from "dayjs/plugin/isSameOrBefore";
import { ISprintFilterOptions } from "../../api/options/filterOptions/ISprintFilterOptions";
import { sprintStatuses } from "../../sync/sprintStatuses";
import { DeleteOutlined } from "@ant-design/icons";

dayjs.extend(isSameOrBefore);

interface IFilterSprintsFormProps {
  onFilterApply?: (filters: ISprintFilterOptions) => void;
}

interface IFormValues {
  name?: string;
  startDate?: Dayjs;
  endDate?: Dayjs;
  status?: string;
}

const availableFields = [
  { key: "name", label: "Название спринта", type: "input" },
  { key: "startDate", label: "Дата начала от", type: "datePicker" },
  { key: "endDate", label: "Дата окончания до", type: "datePicker" },
  { key: "status", label: "Статус", type: "select" },
];

export const FilterSprintsForm = ({
  onFilterApply,
}: IFilterSprintsFormProps) => {
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
    onFilterApply?.({});
  };

  const onFormFinish = (values: IFormValues) => {
    const newFilters: ISprintFilterOptions = {
      name: values.name,
      startDate: values.startDate ? dayjs(values.startDate) : undefined,
      endDate: values.endDate ? dayjs(values.endDate) : undefined,
      status: sprintStatuses.find((status) => status.name === values.status),
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
                        <Input
                          allowClear
                          placeholder="Введите название спринта"
                        />
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
                            options={sprintStatuses?.map((status) => ({
                              value: status.name,
                              label: status.name,
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
