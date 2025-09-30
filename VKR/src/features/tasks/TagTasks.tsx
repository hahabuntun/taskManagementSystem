import { Card } from "antd";
import { useParams } from "react-router-dom";
import { useGetTasksByTag } from "../../api/hooks/tags";
import { TasksTable } from "./TasksTable";

const TagTasksPage = () => {
  const { tagName } = useParams<{ tagName: string }>();
  const { data: tasks } = useGetTasksByTag(tagName ?? "");

  return (
    <Card title={`Задачи с тегом "${tagName}"`}>
      <TasksTable tasks={tasks ?? []} />
    </Card>
  );
};

export default TagTasksPage;
