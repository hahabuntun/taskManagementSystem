import { Table, TableProps, Tooltip, Typography } from "antd";
import { IHistoryItem } from "../../interfaces/IHistoryItem";
import { LinkButton } from "../../components/LinkButton";
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { useDeleteHistoryItem } from "../../api/hooks/history";

const { Text } = Typography;

interface IHistoryTableProps {
  items: IHistoryItem[];
}

export const HistoryTable = ({ items }: IHistoryTableProps) => {
  const deleteAsync = useDeleteHistoryItem();
  const columns: TableProps<IHistoryItem>["columns"] = [
    {
      title: "Дата",
      key: "date",
      render: (_, record) => (
        <Text>{record.createdAt.format("DD.MM.YYYY")}</Text>
      ),
    },
    {
      title: "Запись",
      key: "text",
      render: (_, record) => <p>{record.message}</p>,
    },
    {
      title: "Кто выполнил действие",
      key: "creator",
      render: (_, record) => (
        <LinkButton handleClicked={() => console.log("")}>
          {record.responsibleWorker?.email}
        </LinkButton>
      ),
    },
    {
      title: "Действия",
      key: "action",
      render: (_, record) => (
        <Tooltip key={record.id} placement="top" title="Удалить">
          <DeleteButton
            itemId={record.id}
            onClick={() =>
              deleteAsync({ itemId: record.id, ownerType: record.owner.type })
            }
            text="Удалить"
          />
        </Tooltip>
      ),
    },
  ];

  return <Table columns={columns} dataSource={items} pagination={false} />;
};
