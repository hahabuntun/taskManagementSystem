import { Select, Button } from "antd";
import { IWorkerPosition } from "../../interfaces/IWorkerPosition";
import FormItem from "antd/es/form/FormItem";
import { DeleteOutlined } from "@ant-design/icons";

interface IFilterPositionsFormProps {
  positions: IWorkerPosition[];
  onSearch?: (position: IWorkerPosition | null) => void;
}

export const FilterPositionsForm = ({
  positions,
  onSearch,
}: IFilterPositionsFormProps) => {
  const handleSelectChange = (value: number | undefined) => {
    if (value) {
      onSearch?.(positions.find((position) => position.id === value) || null);
    } else {
      onSearch?.(null);
    }
  };

  const handleResetAll = () => {
    onSearch?.(null);
  };

  return (
    <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
      <FormItem
        style={{ marginBottom: "1rem", flex: 1 }}
        label="Должность"
        name="title"
      >
        <Select
          showSearch
          allowClear
          placeholder="Выберите должность"
          onChange={handleSelectChange}
          options={positions.map((position: IWorkerPosition) => ({
            value: position.id,
            label: position.title,
          }))}
        />
      </FormItem>
      <Button onClick={handleResetAll} danger icon={<DeleteOutlined />}>
        Сбросить фильтр
      </Button>
    </div>
  );
};
