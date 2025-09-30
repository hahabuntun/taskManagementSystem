import { useState } from "react";
import { PageOwnerEnum } from "../../../../enums/ownerEntities/PageOwnerEnum";
import { ITask } from "../../../../interfaces/ITask";
import { useGetTasks } from "../../../../api/hooks/tasks";

import dayjs, { Dayjs } from "dayjs";
import { Calendar, CalendarProps, Modal } from "antd";
import { TasksTable } from "../../TasksTable";
import CalendarCell from "./CalendarCell";
import { CustomCalendarHeader } from "./CustomCalendarHeader";

interface CalendarTasksProps {
  ownerType: PageOwnerEnum.PROJECT;
  ownerId: number;
}

export const CalendarFeature = ({ ownerId, ownerType }: CalendarTasksProps) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modalTasks, setModalTasks] = useState<ITask[]>([]);

  const [isCalendarDisabled, setIsCalendarDisabled] = useState(false);

  const { data } = useGetTasks({
    entityId: ownerId,
    pageType: ownerType,
    enabled: true,
  });

  const tasks = data ?? [];

  const handleCellClicked = (date: Dayjs) => {
    const modalTasks =
      tasks?.filter((task) => {
        if (task.endDate) {
          const taskDeadline = dayjs(task.endDate);
          return taskDeadline.isSame(date, "day"); // Сравниваем дату задачи с выбранной датой
        }
        return false; // Если у задачи нет дедлайна, она не отображается
      }) || [];
    setModalTasks(modalTasks);
    setIsModalOpen(true);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
    setModalTasks([]);
  };

  const filterTasks = (date: Dayjs): ITask[] => {
    return (
      tasks?.filter((task) => {
        if (task.endDate) {
          const taskDeadline = dayjs(task.endDate);
          return taskDeadline.isSame(date, "day"); // Сравниваем дату задачи с выбранной датой
        }
        return false; // Если у задачи нет дедлайна, она не отображается
      }) || []
    );
  };

  const cellRender: CalendarProps<Dayjs>["cellRender"] = (
    currentDate: Dayjs
  ) => {
    const tasksForDate = filterTasks(currentDate);

    return <CalendarCell tasks={tasksForDate} />;
  };

  return (
    <>
      <Modal
        open={isModalOpen}
        footer={null}
        onCancel={handleCancel}
        styles={{
          body: {
            overflowX: "auto",
          },
        }}
        width={800}
        title="Дедлайн в этот день"
      >
        <TasksTable tasks={modalTasks} />
      </Modal>
      <Calendar
        className="customCalendar"
        mode="month"
        onSelect={(date) => {
          if (isCalendarDisabled) {
            return;
          }
          handleCellClicked(date);
        }}
        style={{ minWidth: "500px" }}
        cellRender={cellRender}
        headerRender={({ value, onChange }) => (
          <CustomCalendarHeader
            isCalendarDisabled={isCalendarDisabled}
            setIsCalendarDisabled={setIsCalendarDisabled}
            value={value}
            onChange={onChange}
          />
        )}
      />
    </>
  );
};
