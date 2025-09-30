// components/SavedFilters.tsx
import { useState } from "react";
import { Select, Button, Popconfirm, notification } from "antd";
import { useGetTaskFilters, useDeleteTaskFilter } from "../../api/hooks/tasks";
import { ITaskFilter } from "../../interfaces/ITaskFilter";

interface ISavedFiltersProps {
  onFilterSelect: (filter: ITaskFilter) => void;
}

export const SavedFilters = ({ onFilterSelect }: ISavedFiltersProps) => {
  const { data: filters, refetch } = useGetTaskFilters();
  const deleteFilterAsync = useDeleteTaskFilter(() => refetch());

  const [selectedFilterName, setSelectedFilterName] = useState<string | null>(
    null
  );

  const handleSelect = (filterName: string) => {
    const selectedFilter = filters?.find(
      (filter) => filter.name === filterName
    );
    if (selectedFilter) {
      setSelectedFilterName(filterName);
      onFilterSelect(selectedFilter);
    }
  };

  const handleDelete = async (filterName: string) => {
    try {
      await deleteFilterAsync({ filterName });
      if (selectedFilterName === filterName) {
        setSelectedFilterName(null);
      }
    } catch (error) {
      notification.error({ message: "Ошибка при удалении фильтра" });
    }
  };

  return (
    <div style={{ marginBottom: "1rem" }}>
      <Select
        style={{ width: "100%", maxWidth: "300px" }}
        placeholder="Выберите сохранённый фильтр"
        value={selectedFilterName}
        onChange={handleSelect}
        allowClear
        onClear={() => setSelectedFilterName(null)}
        options={filters?.map((filter) => ({
          value: filter.name,
          label: (
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <span>{filter.name}</span>
              <Popconfirm
                title="Удалить фильтр?"
                onConfirm={() => handleDelete(filter.name)}
                okText="Да"
                cancelText="Нет"
              >
                <Button
                  type="link"
                  danger
                  size="small"
                  onClick={(e) => e.stopPropagation()}
                >
                  Удалить
                </Button>
              </Popconfirm>
            </div>
          ),
        }))}
      />
    </div>
  );
};
