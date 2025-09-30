import { ISprint } from "../../interfaces/ISprint";
import { Flex, Popover, Table, TableProps, Tag, Typography } from "antd";
import { LinkButton } from "../../components/LinkButton";
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { EditButton } from "../../components/buttons/EditButton";
import { IEditSprintOptions } from "../../api/options/editOptions/IEditSprintOptions";
import { useNavigate } from "react-router-dom";
import { EditSprintForm } from "./sprint/EditSprintForm";
import { useDeleteSprint, useEditSprint } from "../../api/hooks/sprints";

interface ISprintTableProps {
  sprints: ISprint[];
}

export const SprintsTable = ({ sprints }: ISprintTableProps) => {
  const navigate = useNavigate();

  const editAsync = useEditSprint();
  const deleteAsync = useDeleteSprint();

  const columns: TableProps<ISprint>["columns"] = [
    {
      title: "Название",
      key: "sprintName",
      render: (_, record) => (
        <LinkButton handleClicked={() => navigate(`/sprints/${record.id}`)}>
          {record.name}
        </LinkButton>
      ),
    },
    {
      title: "Статус",
      key: "status",
      render: (_, record) => (
        <Flex>
          <Tag color={record.status.color}>
            <Typography.Text style={{ color: "black" }}>
              {record.status.name}
            </Typography.Text>
          </Tag>
        </Flex>
      ),
    },
    {
      title: "Дата начала",
      key: "startDate",
      render: (_, record) => (
        <Flex>{record.startDate?.format("DD.MM.YYYY")}</Flex>
      ),
    },
    {
      title: "Дата окончания",
      key: "endDate",
      render: (_, record) => (
        <Flex>{record.endDate?.format("DD.MM.YYYY")}</Flex>
      ),
    },
    {
      title: "Действия",
      key: "action",
      render: (_, record) => (
        <Flex style={{ maxWidth: "150px" }} vertical gap={4}>
          <DeleteButton
            onClick={() => deleteAsync({ sprintId: record.id })}
            itemId={record.id}
            text="Удалить"
          />
          <Popover
            trigger="click"
            content={() => (
              <EditSprintForm
                sprint={record}
                onEdited={(options: IEditSprintOptions) =>
                  editAsync({ sprintId: record.id, options })
                }
              />
            )}
          >
            <EditButton itemId={record.id} text="Изменить" />
          </Popover>
        </Flex>
      ),
    },
  ];

  return (
    <Table
      rowKey={"id"}
      columns={columns}
      dataSource={sprints}
      pagination={false}
    />
  );
};
