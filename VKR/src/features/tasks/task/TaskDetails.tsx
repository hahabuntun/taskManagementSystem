import { Flex, Card } from "antd";
import { ITask } from "../../../interfaces/ITask";
import { TaskCard } from "./TaskCard";
import ChecklistsFeature from "../../checklists/ChecklistsFeature";
import { TaskTagManager } from "./TaskTagManager";
import { TaskWorkerManager } from "./TaskWorkerManagement";
import { CheckListOwnerEnum } from "../../../enums/ownerEntities/CheckListOwnerEnum";

interface ITaskDetailsProps {
  task: ITask;
}

export const TaskDetails = ({ task }: ITaskDetailsProps) => {
  return (
    <div>
      <Flex
        vertical // Stack cards vertically on small screens
        gap="24px"
        style={{ padding: "24px", margin: "0 auto" }}
      >
        {/* Контейнер для карточек */}
        <Flex gap="24px" vertical>
          {/* Основная информация */}
          <TaskCard minWidth="500px" task={task} />

          <Card
            title="Сотрудники"
            style={{
              minWidth: "500px",
              borderRadius: "8px",
              boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
            }}
          >
            <TaskWorkerManager taskId={task.id} projectId={task.project.id} />
          </Card>

          <Card
            title="Теги"
            style={{
              minWidth: "500px",
              borderRadius: "8px",
              boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
            }}
          >
            <TaskTagManager taskId={task.id} />
          </Card>

          {/* Чек-листы */}
          <Card
            style={{
              minWidth: "500px",
              borderRadius: "8px",
              boxShadow: "0 2px 8px rgba(0, 0, 0, 0.1)",
            }}
          >
            <ChecklistsFeature
              ownerId={task.id}
              ownerType={CheckListOwnerEnum.TASK}
            />
          </Card>
        </Flex>
      </Flex>
    </div>
  );
};
