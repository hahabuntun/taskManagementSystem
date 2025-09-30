import { Select, Row, Col } from "antd";
import dayjs, { Dayjs } from "dayjs";

interface CustomHeaderProps {
  value: Dayjs;
  onChange: (value: Dayjs) => void;
  setIsCalendarDisabled: (a: boolean) => void;
  isCalendarDisabled: boolean;
}

export const CustomCalendarHeader = ({
  isCalendarDisabled,
  setIsCalendarDisabled,
  value,
  onChange,
}: CustomHeaderProps) => {
  const currentYear = value.year();
  const currentMonth = value.month();
  const years = Array.from({ length: 20 }, (_, i) => currentYear - 10 + i); // Генерируем список лет
  const months = Array.from({ length: 12 }, (_, i) => i); // Месяцы от 0 до 11

  const handleYearChange = (newYear: number) => {
    onChange(value.year(newYear)); // Update the year
  };

  // Handle month change
  const handleMonthChange = (newMonth: number) => {
    onChange(value.month(newMonth)); // Update the month
  };

  return (
    <Row justify="end" align="middle" style={{ padding: "8px 16px" }}>
      <Col style={{ marginRight: "0.5rem" }}>
        <Select
          value={currentYear}
          style={{ width: 100 }}
          onDropdownVisibleChange={() =>
            setIsCalendarDisabled(!isCalendarDisabled)
          }
          onChange={(value) => handleYearChange(value)} // Use handleYearChange directly
        >
          {years.map((year) => (
            <Select.Option key={year} value={year}>
              {year}
            </Select.Option>
          ))}
        </Select>
      </Col>
      <Col>
        <Select
          value={currentMonth}
          onDropdownVisibleChange={() =>
            setIsCalendarDisabled(!isCalendarDisabled)
          }
          style={{ width: 100 }}
          onChange={(value) => handleMonthChange(value)} // Use handleMonthChange directly
        >
          {months.map((month) => (
            <Select.Option key={month} value={month}>
              {dayjs().month(month).format("MMMM")}
            </Select.Option>
          ))}
        </Select>
      </Col>
    </Row>
  );
};
