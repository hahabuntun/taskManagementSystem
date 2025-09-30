import { Flex, Table, TableProps, Tooltip, Typography, Button } from "antd"; // Added Button import
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { WorkerAvatar } from "../../components/WorkerAvatar";
import useApplicationStore from "../../stores/applicationStore";
import { useDeleteFile } from "../../api/hooks/files";
import { useDownloadFile } from "../../api/hooks/files";
import { IFile } from "../../interfaces/IFile";

const { Text } = Typography;

interface IFileTableProps {
  items: IFile[];
}

export const FilesTable = ({ items }: IFileTableProps) => {
  const { user } = useApplicationStore.getState();
  const deleteAsync = useDeleteFile();
  const downloadAsync = useDownloadFile();

  const columns: TableProps<IFile>["columns"] = [
    {
      title: "Создатель",
      key: "creator",
      render: (_, record) => (
        <WorkerAvatar size="small" worker={record.creator} />
      ),
    },
    {
      title: "Название файла",
      key: "text",
      render: (_, record) => (
        <Button
          type="link"
          onClick={(e) => {
            e.preventDefault(); // Prevent default button behavior if any
            downloadAsync({
              fileId: record.id,
              ownerId: record.owner.id,
              ownerType: record.owner.type,
            });
          }}
        >
          {record.name}
        </Button>
      ),
    },
    {
      title: "Описание",
      key: "description",
      render: (_, record) => {
        const maxLength = 50;
        const truncatedDescription =
          record.description.length > maxLength
            ? `${record.description.slice(0, maxLength)}...`
            : record.description;

        return (
          <Tooltip title={record.description}>
            <Text>{truncatedDescription}</Text>
          </Tooltip>
        );
      },
    },
    {
      title: "Дата",
      key: "date",
      render: (_, record) => (
        <Flex style={{ minWidth: "100px" }}>
          {record.createdAt.format("DD.MM.YYYY")}
        </Flex>
      ),
    },
    {
      title: "Действия",
      key: "action",
      render: (_, record) => (
        <Flex>
          {user && user.isAdmin && (
            <>
              <DeleteButton
                style={{ marginRight: "1rem" }}
                itemId={record.id}
                onClick={() =>
                  deleteAsync({
                    fileId: record.id,
                    ownerId: record.owner.id,
                    ownerType: record.owner.type,
                  })
                }
                text="Удалить"
              />
            </>
          )}
        </Flex>
      ),
    },
  ];

  return (
    <Table
      rowKey="id"
      columns={columns}
      dataSource={items}
      pagination={false}
    />
  );
};
