// components/analytics/SprintAnalytics.tsx
import { useRef, useEffect } from "react";
import { Card, Flex, Progress, Typography } from "antd";
import { Bar, Pie } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend,
  PointElement,
  LineElement,
  Chart,
} from "chart.js";
import { useSprintAnalytics } from "../../api/hooks/analytics";
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
  Legend,
  PointElement,
  LineElement
);

const { Text } = Typography;

interface SprintAnalyticsProps {
  sprintId: number;
}

const SprintAnalytics = ({ sprintId }: SprintAnalyticsProps) => {
  const { isLoading, error, chartData, totalTasks, completedPercentage } =
    useSprintAnalytics(sprintId);

  const statusChartRef = useRef<Chart<"bar"> | null>(null);
  const priorityChartRef = useRef<Chart<"pie"> | null>(null);
  const tagChartRef = useRef<Chart<"bar"> | null>(null);
  const employeeChartRef = useRef<Chart<"bar"> | null>(null);
  const burnDownChartRef = useRef<Chart<"line"> | null>(null);

  useEffect(() => {
    const handleResize = () => {
      [
        statusChartRef,
        priorityChartRef,
        tagChartRef,
        employeeChartRef,
        burnDownChartRef,
      ].forEach((ref) => ref.current?.resize());
    };
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  if (isLoading) return <div>Загрузка аналитики спринта...</div>;
  if (error) return <div>Ошибка: {error.message}</div>;
  if (!chartData) return null;

  const pieChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { position: "top" as const },
      tooltip: { enabled: true },
    },
  };

  return (
    <div style={{ padding: "20px" }}>
      <Flex gap="16px" wrap>
        <Card
          title="Прогресс спринта"
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
          title="Статус задач"
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
          title="Приоритеты задач"
          style={{ flex: "1 1 400px", overflow: "auto" }}
        >
          <div style={{ minHeight: "400px" }}>
            <Pie
              ref={priorityChartRef}
              data={chartData.priority}
              options={pieChartOptions}
            />
          </div>
        </Card>
      </Flex>
      <Flex gap="16px" wrap style={{ marginTop: "20px" }}>
        <Card
          title="Распределение задач по сотрудникам"
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
      </Flex>
      <Flex gap="16px" wrap style={{ marginTop: "20px" }}>
        <Card
          title="Теги задач"
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

export default SprintAnalytics;
