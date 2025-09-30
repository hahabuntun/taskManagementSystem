import { Button } from "antd";
import { DrawerEntityEnum } from "../../../../enums/DrawerEntityEnum";
import { ITask } from "../../../../interfaces/ITask";
import useApplicationStore from "../../../../stores/applicationStore";

interface CalendarCellProps {
  tasks: ITask[];
}
function CalendarCell({ tasks }: CalendarCellProps) {
  const { showDrawer } = useApplicationStore.getState();
  const handleClick = (taskId: number, event: React.MouseEvent) => {
    event.stopPropagation();
    showDrawer(DrawerEntityEnum.TASK, taskId);
  };

  return (
    <ul
      style={{
        listStyleType: "none",
        display: "flex", // Используем flexbox для удобного центрирования
        flexDirection: "column", // Располагаем элементы по вертикали
        margin: 0,
        padding: 0,
        overflowX: "hidden",
      }}
    >
      {tasks.map((task) => (
        <li key={task.id}>
          {" "}
          {/* Дополнительный отступ между элементами */}
          <Button
            onClick={(event) => handleClick(task.id, event)}
            type="link"
            size="small"
            style={{
              fontSize: "0.75rem",
              textAlign: "center",
              display: "block", // Гарантирует, что содержимое занимает всю ширину
            }}
          >
            {task.name}
          </Button>
        </li>
      ))}
    </ul>
  );
}

export default CalendarCell;
