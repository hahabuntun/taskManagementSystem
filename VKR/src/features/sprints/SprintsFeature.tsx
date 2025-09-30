import { Collapse, Divider, Popover, Space, Typography, Switch } from "antd";
import { useState } from "react";
import { useGetSprints } from "../../api/hooks/sprints";
import { AddButton } from "../../components/buttons/AddButton";
import { SprintsTable } from "./SprintsTable";
import { AddSprintForm } from "./sprint/AddSprintForm";
import { IAddSprintOptions } from "../../api/options/createOptions/IAddSprintOptions";
import { useAddSprintToProject } from "../../api/hooks/projects";
import { ISprintFilterOptions } from "../../api/options/filterOptions/ISprintFilterOptions";
import { FilterSprintsForm } from "./FilterSprintsForm";
import { PageOwnerEnum } from "../../enums/ownerEntities/PageOwnerEnum";
import { SprintBoard } from "../boards/board/SprintBoard";

interface ISprintsFeatureProps {
  entityType: PageOwnerEnum.PROJECT | PageOwnerEnum.WORKER;
  entityId: number;
}

export const SprintsFeature = ({
  entityType,
  entityId,
}: ISprintsFeatureProps) => {
  const { data, setFilters } = useGetSprints(entityType, entityId);

  const [viewType, setViewType] = useState<"table" | "board">("table"); // State to track view type

  const addAsync = useAddSprintToProject();

  const handleFilterApply = (filters: ISprintFilterOptions) => {
    setFilters(filters);
  };

  const handleViewChange = (checked: boolean) => {
    setViewType(checked ? "board" : "table");
  };

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Спринты
        </Typography.Title>
        {entityType === PageOwnerEnum.PROJECT && (
          <Popover
            trigger="click"
            title="Добавление спринта"
            content={() => (
              <AddSprintForm
                onAdded={(options: IAddSprintOptions) =>
                  addAsync({ projectId: entityId, options })
                }
              />
            )}
          >
            <AddButton text="Добавить" />
          </Popover>
        )}
        {/* View toggle switch */}
        {entityType === PageOwnerEnum.PROJECT && (
          <Space>
            <Typography.Text>Таблица</Typography.Text>
            <Switch
              checked={viewType === "board"}
              onChange={handleViewChange}
            />
            <Typography.Text>Доска</Typography.Text>
          </Space>
        )}
      </Space>

      <Collapse
        style={{ margin: "1rem auto" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: <FilterSprintsForm onFilterApply={handleFilterApply} />,
          },
        ]}
      />

      <Divider />

      {/* Conditional rendering based on viewType */}
      {viewType === "table" ? (
        <SprintsTable sprints={data || []} />
      ) : (
        entityType === PageOwnerEnum.PROJECT && (
          <SprintBoard projectId={entityId} />
        )
      )}
    </>
  );
};
