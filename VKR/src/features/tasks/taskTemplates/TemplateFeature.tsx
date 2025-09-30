import { Tabs, TabsProps } from "antd";
import { TemplateDetails } from "./TemplateDetails";
import { LinksFeature } from "../../links/LinksFeature";
import { LinkOwnerEnum } from "../../../enums/ownerEntities/LinkOwnerEnum";

interface ITemplateFeatureProps {
  templateId: number;
}

export const TemplateFeature = ({ templateId }: ITemplateFeatureProps) => {
  const items: TabsProps["items"] = [
    {
      key: "main",
      label: "Главная",
      children: <TemplateDetails templateId={templateId} />,
    },
    {
      key: "links",
      label: "Ссылки",
      children: (
        <LinksFeature
          ownerId={templateId}
          ownerType={LinkOwnerEnum.TASK_TEMPLATE}
        />
      ),
    },
  ];
  return (
    <>
      <Tabs items={items} />
    </>
  );
};
