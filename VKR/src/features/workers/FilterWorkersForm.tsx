import {
  Button,
  DatePicker,
  Form,
  Input,
  List,
  Modal,
  Select,
  Checkbox,
} from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import { IWorkerFilterOptions } from "../../api/options/filterOptions/IWorkerFilterOptions";
import { useState } from "react";
import { useGetWorkerPositions } from "../../api/hooks/workerPositions";
import dayjs from "dayjs";
import { workerStatuses } from "../../sync/workerStatuses";

interface IFilterWorkersFormProps {
  onFilterApply: (filters: IWorkerFilterOptions) => void;
}

const availableFields = [
  { key: "firstName", label: "Имя", type: "input" },
  { key: "secondName", label: "Фамилия", type: "input" },
  { key: "thirdName", label: "Отчество", type: "input" },
  { key: "email", label: "Email", type: "input" },
  { key: "status", label: "Статус", type: "selectStatus" },
  { key: "workerPosition", label: "Должность", type: "selectPosition" },
  { key: "createdFrom", label: "От какого числа добавлен", type: "datePicker" },
  { key: "createdTill", label: "До какого числа добавлен", type: "datePicker" },
  { key: "isAdmin", label: "Является администратором", type: "Checkbox" },
  { key: "isManager", label: "Является менеджером", type: "Checkbox" },
];

interface IFormValues {
  firstName?: string;
  secondName?: string;
  thirdName?: string;
  email?: string;
  status?: string;
  workerPosition?: string;
  isAdmin?: boolean;
  isManager?: boolean;
  createdFrom?: Date;
  createdTill?: Date;
}

export const FilterWorkersForm = ({
  onFilterApply,
}: IFilterWorkersFormProps) => {
  const { data: workerPositions } = useGetWorkerPositions();
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
    const newFilters: IWorkerFilterOptions = {
      firstName: values.firstName,
      secondName: values.secondName,
      thirdName: values.thirdName,
      email: values.email,
      isAdmin: values.isAdmin,
      isManager: values.isManager,
      status: workerStatuses.find((status) => status.name === values.status),
      workerPosition: workerPositions?.find(
        (position) => position.title === values.workerPosition
      ),
      createdFrom: values.createdFrom ? dayjs(values.createdFrom) : undefined,
      createdTill: values.createdTill ? dayjs(values.createdTill) : undefined,
    };
    const cleanedFilters = Object.fromEntries(
      Object.entries(newFilters).filter(
        ([_, value]) => value !== undefined && value !== ""
      )
    );
    onFilterApply(cleanedFilters as IWorkerFilterOptions);
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
                  case "selectStatus":
                    return (
                      <Form.Item
                        style={{ marginBottom: "0", flex: 1 }}
                        label={field.label}
                        name={fieldKey as keyof IFormValues}
                      >
                        <Select
                          showSearch
                          allowClear
                          options={workerStatuses?.map((status) => ({
                            value: status.name,
                            label: status.name,
                          }))}
                          placeholder={`Выберите ${field.label}`}
                        />
                      </Form.Item>
                    );
                  case "selectPosition":
                    return (
                      <Form.Item
                        style={{ marginBottom: "0", flex: 1 }}
                        label={field.label}
                        name={fieldKey as keyof IFormValues}
                      >
                        <Select
                          showSearch
                          allowClear
                          options={workerPositions?.map((position) => ({
                            value: position.title,
                            label: position.title,
                          }))}
                          placeholder={`Выберите ${field.label}`}
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
                        <DatePicker style={{ width: "100%" }} allowClear />
                      </Form.Item>
                    );
                  case "Checkbox":
                    return (
                      <Form.Item
                        style={{ marginBottom: "0", flex: 1 }}
                        valuePropName="checked"
                        name={fieldKey as keyof IFormValues}
                      >
                        <Checkbox>{field.label}</Checkbox>
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
            danger
            icon={<DeleteOutlined />}
            style={{ marginBottom: "1rem" }}
            onClick={handleResetAll}
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
