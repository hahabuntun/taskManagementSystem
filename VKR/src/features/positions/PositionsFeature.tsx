import {
  useAddWorkerPosition,
  useGetWorkerPositions,
} from "../../api/hooks/workerPositions";
import { Collapse, Divider, Popover, Space, Typography } from "antd";
import { AddPositionForm } from "./position/AddPositionForm";
import { IAddWorkerPositionOptions } from "../../api/options/createOptions/IAddWorkerPositionOptions";
import { AddButton } from "../../components/buttons/AddButton";
import { FilterPositionsForm } from "./FilterPositionsForm";
import { useState } from "react";
import { IPositionFilterOptions } from "../../api/options/filterOptions/IPositionFIlterOptions";
import { PositionsList } from "./PositionsList";
import { IWorkerPosition } from "../../interfaces/IWorkerPosition";
import useApplicationStore from "../../stores/applicationStore";

export const PositionsFeature = () => {
  const [filters, setFilters] = useState<IPositionFilterOptions>({});
  const { data: allPositions } = useGetWorkerPositions();

  const { user } = useApplicationStore.getState();

  const addAsync = useAddWorkerPosition();

  const handleFilterApply = (position: IWorkerPosition | null) => {
    setFilters(position ? { title: position.title } : {});
  };

  const filteredPositions =
    allPositions?.filter(
      (pos) => !filters.title || pos.title === filters.title
    ) || [];

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Должности
        </Typography.Title>
        {user && user.isAdmin && (
          <Popover
            trigger="click"
            title="Создание должности"
            destroyTooltipOnHide={true}
            content={
              <AddPositionForm
                onAdded={(options: IAddWorkerPositionOptions) => {
                  addAsync({
                    options: options,
                  });
                }}
              />
            }
          >
            <AddButton text="Добавить" />
          </Popover>
        )}
      </Space>

      <Collapse
        style={{ margin: "1rem auto" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: (
              <FilterPositionsForm
                positions={allPositions || []}
                onSearch={handleFilterApply}
              />
            ),
          },
        ]}
      />

      <Divider />

      <PositionsList items={filteredPositions} />
    </>
  );
};
