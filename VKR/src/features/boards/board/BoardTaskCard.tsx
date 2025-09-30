import { useDrag } from "react-dnd";
import { ITask } from "../../../interfaces/ITask";
import { TaskCard } from "../../tasks/task/TaskCard";
import useApplicationStore from "../../../stores/applicationStore";

interface IBoardTaskCardProps {
  task: ITask;
  isDragDisabled: boolean;
  moveTask: (taskId: number, toColumn: string) => void;
  columnName: string;
  onDelete?: (taskId: number) => void;
}

export const BoardTaskCard = ({
  task,
  columnName,
  isDragDisabled,
  onDelete,
}: IBoardTaskCardProps) => {
  const { fieldsToShow } = useApplicationStore();

  const [{ isDragging }, drag] = useDrag(() => ({
    type: "TASK",
    item: { id: task.id, columnName }, // Define the dragged item
    collect: (monitor) => ({
      isDragging: !!monitor.isDragging(), // Monitor drag state
    }),
    canDrag: !isDragDisabled,
  }));

  return (
    <div
      ref={drag}
      style={{
        margin: "0 auto",
        marginBottom: "1rem",
        opacity: isDragging ? 0.5 : 1, // Visual feedback
      }}
    >
      <TaskCard
        onDelete={onDelete}
        task={task}
        maxWidth={"280px"}
        minWidth={"270px"}
        fieldsToShow={fieldsToShow}
      />
    </div>
  );
};
