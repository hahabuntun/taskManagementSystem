import { useRef, useEffect } from "react";
import { Card, Flex, Progress, Typography } from "antd";
import { Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend,
  Chart,
} from "chart.js";
import { useWorkerAnalytics } from "../../api/hooks/analytics";
import { taskStatuses } from "../../sync/taskStatuses";
import { TaskStatusEnum } from "../../enums/statuses/TaskStatusEnum";
import {
  horizontalBarChartOptions,
  horizontalBarChartWithoutHeadingOptions,
} from "../../config/chartConfig";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend
);

const { Text } = Typography;

interface EmployeeAnalyticsProps {
  employeeId: number;
}

const EmployeeAnalytics = ({ employeeId }: EmployeeAnalyticsProps) => {
  const { isLoading, error, chartData, totalTasks, completedPercentage } =
    useWorkerAnalytics(employeeId);

  const statusChartRef = useRef<Chart<"bar"> | null>(null);
  const priorityChartRef = useRef<Chart<"bar"> | null>(null);
  const tagChartRef = useRef<Chart<"bar"> | null>(null);
  const projectStatusChartRef = useRef<Chart<"bar"> | null>(null);

  useEffect(() => {
    const handleResize = () => {
      [
        statusChartRef,
        priorityChartRef,
        tagChartRef,
        projectStatusChartRef,
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
          title="Прогресс сотрудника"
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
        <Card
          title="Задачи по проектам и статусам"
          style={{ flex: "1 1 400px", overflow: "auto" }}
        >
          <div style={{ minHeight: "400px" }}>
            <Bar
              ref={projectStatusChartRef}
              data={chartData.projectStatus}
              options={horizontalBarChartOptions}
            />
          </div>
        </Card>
      </Flex>
    </div>
  );
};

export default EmployeeAnalytics;
