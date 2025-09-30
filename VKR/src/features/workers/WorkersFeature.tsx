import { Collapse, Divider, Popover, Space, Typography } from "antd";
import {
  useAddWorker,
  useDeleteWorker,
  useEditWorker,
  useGetWorkers,
} from "../../api/hooks/workers";
import { AddWorkerForm } from "./worker/AddWorkerForm";
import { IAddWorkerOptions } from "../../api/options/createOptions/IAddWorkerOptions";
import { AddButton } from "../../components/buttons/AddButton";
import { FilterWorkersForm } from "./FilterWorkersForm";
import { IWorkerFilterOptions } from "../../api/options/filterOptions/IWorkerFilterOptions";
import { WorkersTable } from "./WorkersTable";
import { IEditWorkerOptions } from "../../api/options/editOptions/IEditWorkerOptions";
import useApplicationStore from "../../stores/applicationStore";

export const WorkersFeature = () => {
  const { data: items, refetch, setFilters } = useGetWorkers();

  const { user } = useApplicationStore.getState();

  const addAsync = useAddWorker(() => {
    refetch();
  });

  const deleteAsync = useDeleteWorker(() => {
    refetch();
  });

  const editAsync = useEditWorker(() => {
    refetch();
  });

  const handleFilterApply = (filters: IWorkerFilterOptions) => {
    setFilters(filters);
  };

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Сотрудники
        </Typography.Title>
        {user && user.isAdmin && (
          <Popover
            trigger="click"
            title="Добавление сотрудника"
            content={() => (
              <AddWorkerForm
                onAddItem={(options: IAddWorkerOptions) => {
                  addAsync({
                    options: options,
                  });
                }}
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
        isEditable={true}
        isDeletable={true}
        items={items ?? []}
        onDeleteItem={(itemId: number) =>
          deleteAsync({
            workerId: itemId,
          })
        }
        onEditItem={(itemId: number, options: IEditWorkerOptions) => {
          editAsync({
            workerId: itemId,
            options: options,
          });
        }}
      />
    </>
  );
};
