import { Collapse, Divider, Modal, Space, Typography } from "antd";
import { TemplateCard } from "./TemplateCard";
import { useGetTaskTemplates } from "../../../api/hooks/tasks";
import useApplicationStore from "../../../stores/applicationStore";
import { AddButton } from "../../../components/buttons/AddButton";
import { useState } from "react";
import { CreateTemplate } from "./CreateTemplate";
import { FilterTemplatesForm } from "./FilterTemplatesForm";
import { ITaskTemplateFilterOptions } from "../../../api/options/filterOptions/ITaskTemplateFilterOptions";

interface TemplatesFeatureProps {}

export const TemplatesFeature = ({}: TemplatesFeatureProps) => {
  const { data: templates, setFilters } = useGetTaskTemplates();
  const [isCreateTemplateModalOpen, setIsCreateTemplateModalOpen] =
    useState<boolean>(false);
  const [currentFilters, setCurrentFilters] =
    useState<ITaskTemplateFilterOptions | null>(null);

  const { user } = useApplicationStore.getState();

  const handleFilterApply = (newFilters: ITaskTemplateFilterOptions) => {
    setCurrentFilters(newFilters);
    setFilters(newFilters);
  };

  if (user) {
    return (
      <>
        <Space align="baseline" style={{ width: "100%", padding: "16px" }}>
          <Typography.Title level={3}>Шаблоны задач</Typography.Title>
          <AddButton
            onClick={() => setIsCreateTemplateModalOpen(true)}
            text="Добавить шаблон"
          />
        </Space>

        <Collapse
          style={{ margin: "1rem auto" }}
          items={[
            {
              key: "filters",
              label: "Фильтры",
              children: (
                <FilterTemplatesForm
                  workerId={user.id}
                  onFilterApply={handleFilterApply}
                  initialFilters={currentFilters ?? {}}
                />
              ),
            },
          ]}
        />

        <Divider />

        {templates?.map((template) => (
          <TemplateCard key={template.id} template={template} />
        ))}

        <Modal
          title="Создать шаблон"
          open={isCreateTemplateModalOpen}
          onCancel={() => setIsCreateTemplateModalOpen(false)}
          footer={null}
          width={600}
        >
          <CreateTemplate />
        </Modal>
      </>
    );
  }
  return null;
};
