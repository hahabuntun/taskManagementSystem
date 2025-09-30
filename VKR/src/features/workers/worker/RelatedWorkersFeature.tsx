import { Collapse, Divider, Popover, Space, Typography } from "antd";
import {
  useAddSubordinateToWorker,
  useDeleteSubordinateFromWorker,
  useGetWorkerRelatedWorkers,
} from "../../../api/hooks/workers";
import { FilterWorkersForm } from "../FilterWorkersForm";
import { WorkersTable } from "../WorkersTable";
import { IWorkerFilterOptions } from "../../../api/options/filterOptions/IWorkerFilterOptions";
import { AddButton } from "../../../components/buttons/AddButton";
import { AddSubordinateFeature } from "./AddSubordinateFeature";
import { IAddSubordinateOptions } from "../../../api/options/createOptions/IAddSubordinatesOptions";
import useApplicationStore from "../../../stores/applicationStore";

interface IRelatedWorkersFeatureProps {
  relationType: "directors" | "subordinates";
  workerId: number;
}

export const RelatedWorkersFeature = ({
  relationType,
  workerId,
}: IRelatedWorkersFeatureProps) => {
  const { data, refetch, setFilters } = useGetWorkerRelatedWorkers(
    workerId,
    relationType
  );

  const { user } = useApplicationStore.getState();

  const addSubordinateAsync = useAddSubordinateToWorker(() => {
    refetch();
  });

  const deleteSubordinateAsync = useDeleteSubordinateFromWorker(() => {
    refetch();
  });

  const handleFilterApply = (filters: IWorkerFilterOptions) => {
    setFilters(filters);
  };

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          {relationType === "subordinates"
            ? "Подчиненные сотрудника"
            : "Начальники сотрудника"}
        </Typography.Title>
        {relationType === "subordinates" && user?.isAdmin && (
          <Popover
            trigger="click"
            title="Добавление подчиненного"
            placement="top"
            style={{ marginBottom: "1rem" }}
            content={() => (
              <AddSubordinateFeature
                workerId={workerId}
                onAdded={(options: IAddSubordinateOptions) =>
                  addSubordinateAsync({ workerId: workerId, options: options })
                }
              />
            )}
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
            children: <FilterWorkersForm onFilterApply={handleFilterApply} />,
          },
        ]}
      />

      <Divider />

      <WorkersTable
        onDeleteItem={(itemId: number) => {
          deleteSubordinateAsync({ subordinateId: itemId, workerId: workerId });
        }}
        isEditable={false}
        isDeletable={relationType === "subordinates"}
        items={data || []}
      />
    </>
  );
};
