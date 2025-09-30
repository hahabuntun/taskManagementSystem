// components/analytics/ProjectAnalytics.tsx
import { useRef, useEffect } from "react";
import { Card, Flex, Progress, Typography } from "antd";
import { Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
  Chart,
} from "chart.js";
import { TaskStatusEnum } from "../../enums/statuses/TaskStatusEnum";
import { taskStatuses } from "../../sync/taskStatuses";
import {
  horizontalBarChartOptions,
  horizontalBarChartWithoutHeadingOptions,
} from "../../config/chartConfig";
import { useProjectAnalytics } from "../../api/hooks/analytics";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

const { Text } = Typography;

interface ProjectAnalyticsProps {
  projectId: number;
}

const ProjectAnalytics = ({ projectId }: ProjectAnalyticsProps) => {
  const { isLoading, error, chartData, totalTasks, completedPercentage } =
    useProjectAnalytics(projectId);

  const statusChartRef = useRef<Chart<"bar"> | null>(null);
  const priorityChartRef = useRef<Chart<"bar"> | null>(null);
  const employeeChartRef = useRef<Chart<"bar"> | null>(null);
  const sprintChartRef = useRef<Chart<"bar"> | null>(null);
  const tagChartRef = useRef<Chart<"bar"> | null>(null);

  useEffect(() => {
    const handleResize = () => {
      [
        statusChartRef,
        priorityChartRef,
        employeeChartRef,
        sprintChartRef,
        tagChartRef,
      ].forEach((ref) => ref.current?.resize());
    };
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  if (isLoading) return <div>Загрузка...</div>;
  if (error) return <div>Ошибка: {error.message}</div>;
  if (!chartData) return null;

  return (
    <div style={{ padding: "20px" }}>
      <Flex gap="16px" wrap>
        <Card
          title="Прогресс проекта"
          style={{ flex: "1 1 300px", minWidth: "200px" }}
        >
          <Progress
            type="circle"
            percent={completedPercentage}
            strokeColor={
              taskStatuses.find((s) => s.name === TaskStatusEnum.COMPLETED)
                ?.color || "#0000FF"
            }
          />
          <Text
            style={{ display: "block", marginTop: "10px", textAlign: "center" }}
          >
            {
              chartData.status.datasets[0].data[
                taskStatuses.findIndex(
                  (s) => s.name === TaskStatusEnum.COMPLETED
                )
              ]
            }{" "}
            из {totalTasks} задач завершено ({completedPercentage}%)
          </Text>
        </Card>
      </Flex>
      <Flex gap="16px" wrap style={{ marginTop: "20px" }}>
        <Card
          title="Задачи по статусам"
          style={{ flex: "1 1 400px", overflow: "auto" }}
        >
          <div style={{ minHeight: "400px" }}>
            <Bar
              ref={statusChartRef}
              data={chartData.status}
              options={horizontalBarChartWithoutHeadingOptions}
            />
          </div>
        </Card>
        <Card
          title="Задачи по приоритетам"
          style={{ flex: "1 1 400px", overflow: "auto" }}
        >
          <div style={{ minHeight: "400px" }}>
            <Bar
              ref={priorityChartRef}
              data={chartData.priority}
              options={horizontalBarChartWithoutHeadingOptions}
            />
          </div>
        </Card>
      </Flex>
      <Flex gap="16px" wrap style={{ marginTop: "20px" }}>
        <Card
          title="Задачи по сотрудникам"
          style={{ flex: "1 1 400px", overflow: "auto" }}
        >
          <div style={{ minHeight: "400px" }}>
            <Bar
              ref={employeeChartRef}
              data={chartData.employee}
              options={horizontalBarChartOptions}
            />
          </div>
        </Card>
        <Card
          title="Количество выполненных задач по спринтам"
          style={{ flex: "1 1 400px", overflow: "auto" }}
        >
          <div style={{ minHeight: "400px" }}>
            <Bar
              ref={sprintChartRef}
              data={chartData.sprint}
              options={horizontalBarChartWithoutHeadingOptions}
            />
          </div>
        </Card>
      </Flex>
      <Flex gap="16px" wrap style={{ marginTop: "20px" }}>
        <Card
          title="Задачи по тегам"
          style={{ flex: "1 1 400px", overflow: "auto" }}
        >
          <div
            style={{ minHeight: `${100 + chartData.tag.labels.length * 35}px` }}
          >
            <Bar
              ref={tagChartRef}
              data={chartData.tag}
              options={horizontalBarChartWithoutHeadingOptions}
            />
          </div>
        </Card>
      </Flex>
    </div>
  );
};

export default ProjectAnalytics;
