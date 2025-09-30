import { LinksList } from "./LinksList";
import { useAddLink, useGetLinks } from "../../api/hooks/links";
import { LinkOwnerEnum } from "../../enums/ownerEntities/LinkOwnerEnum";
import { Popover } from "antd";
import { AddButton } from "../../components/buttons/AddButton";
import { AddLinkForm } from "./AddLinkForm";
import { IAddLinkOptions } from "../../api/options/createOptions/IAddLinkOptions";

interface ILinksFeatureProps {
  ownerType: LinkOwnerEnum;
  ownerId: number;
}

export const LinksFeature = ({ ownerType, ownerId }: ILinksFeatureProps) => {
  const { data: items } = useGetLinks(ownerId, ownerType);

  const addAsync = useAddLink();

  return (
    <>
      <Popover
        trigger="click"
        title="Добавление ссылки"
        content={
          <AddLinkForm
            onAddLink={(options: IAddLinkOptions) => {
              addAsync({
                ownerId: ownerId,
                ownerType: ownerType,
                options: options,
              });
            }}
          />
        }
      >
        <AddButton text="Добавить" />
      </Popover>

      <LinksList items={items ?? []} />
    </>
  );
};
