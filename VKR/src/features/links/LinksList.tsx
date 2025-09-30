import { ILink } from "../../interfaces/ILink";
import { Button, Flex, List, Popover } from "antd";
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { EditButton } from "../../components/buttons/EditButton";
import { EditLinkForm } from "./EditLinkForm";
import { useDeleteLink } from "../../api/hooks/links";

interface ILinksListProps {
  items: ILink[];
}

export const LinksList = ({ items }: ILinksListProps) => {
  const deleteAsync = useDeleteLink();
  return (
    <Flex style={{ minWidth: "300px" }} vertical>
      <List
        itemLayout="horizontal"
        locale={{ emptyText: "Нет ссылок" }}
        dataSource={items}
        renderItem={(item: ILink) => (
          <List.Item key={item.id}>
            <Button
              size="small"
              type="link"
              href={item.link}
              style={{ padding: "0", marginRight: "1.5rem" }}
            >
              {item.name || item.link}
            </Button>
            <Flex gap={"0.5rem"}>
              <DeleteButton
                itemId={item.id}
                onClick={() =>
                  deleteAsync({
                    linkId: item.id,
                    ownerId: item.owner.id,
                    ownerType: item.owner.type,
                  })
                }
              />
              <Popover
                trigger="click"
                content={
                  <EditLinkForm
                    linkId={item.id}
                    linkName={item.name}
                    linkText={item.link}
                    ownerId={item.owner.id}
                    ownerType={item.owner.type}
                  />
                }
              >
                <EditButton itemId={item.id} />
              </Popover>
            </Flex>
          </List.Item>
        )}
      />
    </Flex>
  );
};
