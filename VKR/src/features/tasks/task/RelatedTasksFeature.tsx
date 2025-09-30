import { Collapse, Divider, Tabs, Typography, Space, Popover } from "antd";
import {
  useGetRelatedTasks,
  useDeleteRelationBetweenTasks,
  useAddRelatedTask,
} from "../../../api/hooks/tasks";
import { TaskRelationshipTypeEnum } from "../../../enums/TaskRelationshipTypeEnum";
import { AddButton } from "../../../components/buttons/AddButton";
import { FilterTasksForm } from "../FilterTasksForm";
import { ITaskFilterOptions } from "../../../api/options/filterOptions/ITaskFilterOptions";
import { TasksTable } from "../TasksTable";
import { useState } from "react";
import { useGetAvailableRelatedTasks } from "../../../api/hooks/tasks"; // Import the new hook
import useApplicationStore from "../../../stores/applicationStore";
import { ITask } from "../../../interfaces/ITask";

interface IRelatedTasksFeatureProps {
  projectId: number; // Added projectId
  taskId: number;
}

const AddRelationshipForm: React.FC<{
  projectId: number;
  taskId: number;
}> = ({ projectId, taskId }) => {
  const addRelatedTask = useAddRelatedTask();
  const [selectedType, setSelectedType] = useState<TaskRelationshipTypeEnum>(
    TaskRelationshipTypeEnum.FINISH_START // Default type
  );

  const { user } = useApplicationStore.getState();

  // Get available tasks for the selected relationship type
  const {
    data: availableTasks = [],
    isLoading,
    setFilters,
  } = useGetAvailableRelatedTasks(projectId, taskId);

  const handleFilterApply = (newFilters: ITaskFilterOptions) => {
    setFilters(newFilters);
  };

  const handleTaskAdded = async (task: ITask) => {
    await addRelatedTask({
      taskId,
      relatedTaskId: task.id,
      relType: selectedType,
      lag: 0,
    });
  };

  return (
    <div style={{ maxWidth: "800px", maxHeight: "60vh", overflow: "scroll" }}>
      {/* Add a type selector */}
      <Space direction="vertical" style={{ width: "100%", marginBottom: 16 }}>
        <Tabs
          activeKey={selectedType}
          onChange={(key) => setSelectedType(key as TaskRelationshipTypeEnum)}
          items={Object.values(TaskRelationshipTypeEnum).map((type) => ({
            key: type,
            label: type,
          }))}
        />
      </Space>
      {user && (
        <Collapse
          style={{ margin: "1rem auto" }}
          items={[
            {
              key: "filters",
              label: "Фильтры",
              children: (
                <FilterTasksForm
                  workerId={user.id}
                  onFilterApply={handleFilterApply}
                />
              ),
            },
          ]}
        />
      )}

      <Divider />

      {isLoading ? (
        <Typography.Text>Loading...</Typography.Text>
      ) : (
        <TasksTable
          onSelect={(task: ITask) => handleTaskAdded(task)}
          tasks={availableTasks}
        />
      )}
    </div>
  );
};

export const RelatedTasksFeature = ({
  projectId,
  taskId,
}: IRelatedTasksFeatureProps) => {
  const relationshipTypes = Object.values(TaskRelationshipTypeEnum);

  // Fetch related tasks for each relationship type
  const relatedTasksQueries = relationshipTypes.map((type) =>
    useGetRelatedTasks(taskId, type)
  );

  const deleteRelatedAsync = useDeleteRelationBetweenTasks();

  const { user } = useApplicationStore.getState();

  const handleFilterApply =
    (typeIndex: number) => (filters: ITaskFilterOptions) => {
      relatedTasksQueries[typeIndex].setFilters(filters);
    };

  const tabItems = relationshipTypes.map((type, index) => ({
    key: type,
    label: type,
    children: (
      <>
        {user && (
          <Collapse
            style={{ margin: "1rem auto" }}
            items={[
              {
                key: `filters-${type}`,
                label: "Фильтры",
                children: (
                  <FilterTasksForm
                    workerId={user.id}
                    onFilterApply={handleFilterApply(index)}
                  />
                ),
              },
            ]}
          />
        )}

        <Divider />
        <TasksTable
          onDelete={(relTask: ITask) =>
            deleteRelatedAsync({ relatedTaskId: relTask.id, taskId })
          }
          tasks={relatedTasksQueries[index].data || []}
        />
      </>
    ),
  }));

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Связанные задачи
        </Typography.Title>
        <Popover
          trigger="click"
          title="Добавление связанной задачи"
          content={
            <AddRelationshipForm projectId={projectId} taskId={taskId} />
          }
        >
          <AddButton text="Добавить задачу" />
        </Popover>
      </Space>

      <Tabs
        defaultActiveKey={relationshipTypes[0]}
        items={tabItems}
        style={{ marginTop: "1rem" }}
      />
    </>
  );
};
