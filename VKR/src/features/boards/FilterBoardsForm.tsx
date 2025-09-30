import { useState } from "react";
import { Button, DatePicker, Form, Input, Modal, List } from "antd";
import { Dayjs } from "dayjs";
import { IBoardFilterOptopns } from "../../api/options/filterOptions/IBoardFilterOptions";
import { DeleteOutlined } from "@ant-design/icons";

interface IFilterBoardsFormProps {
  onFilterApply?: (filters: IBoardFilterOptopns) => void;
}

interface IFormValues {
  name?: string;
  creatorId?: number;
  createdFrom?: Dayjs;
  createdTill?: Dayjs;
}

const availableFields = [
  { key: "name", label: "Название доски", type: "input" },
  { key: "createdFrom", label: "Создано с", type: "datePicker" },
  { key: "createdTill", label: "Создано по", type: "datePicker" },
];

export const FilterBoardsForm = ({ onFilterApply }: IFilterBoardsFormProps) => {
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
    const newFilters: IBoardFilterOptopns = {
      name: values.name,
      createdFrom: values.createdFrom,
      createdTill: values.createdTill,
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
                          placeholder="Введите часть названия"
                        />
                      </Form.Item>
                    );
                  case "datePicker":
                    return (
                      <Form.Item
                        style={{ marginBottom: "0", flex: 1 }}
                        label={field.label}
                        name={fieldKey as keyof IFormValues}
                      >
                        <DatePicker
                          style={{ width: "100%" }}
                          format="DD.MM.YYYY"
                          allowClear
                        />
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
