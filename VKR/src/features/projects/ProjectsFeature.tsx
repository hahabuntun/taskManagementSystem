import { ProjectsTable } from "./ProjectsTable";
import { useAddProject, useGetProjects } from "../../api/hooks/projects";
import { Collapse, Divider, Modal, Space, Typography } from "antd";
import { AddButton } from "../../components/buttons/AddButton";
import useApplicationStore from "../../stores/applicationStore";
import { FilterProjectsForm } from "./FilterProjectsForm";
import { IAddProjectOptions } from "../../api/options/createOptions/IAddProjectOptions";
import { IProjectFilterOptions } from "../../api/options/filterOptions/IProjectFilterOptions";
import { PageOwnerEnum } from "../../enums/ownerEntities/PageOwnerEnum";
import { useState } from "react";
import { AddProjectForm } from "./project/AddProjectForm";

interface IProjectsFeatureProps {
  entityType: PageOwnerEnum.ORGANIZATION | PageOwnerEnum.WORKER;
  entityId: number;
}

export const ProjectsFeature = ({
  entityType,
  entityId,
}: IProjectsFeatureProps) => {
  const { user } = useApplicationStore.getState();

  const [isAddModalOpen, setIsAddModalOpen] = useState<boolean>(false);

  if (user) {
    const { data: items, setFilters } = useGetProjects(entityId, entityType);

    const addAsync = useAddProject();

    const handleFilterApply = (filters: IProjectFilterOptions) => {
      setFilters(filters);
    };

    return (
      <>
        <Modal
          onCancel={() => setIsAddModalOpen(false)}
          onClose={() => setIsAddModalOpen(false)}
          onOk={() => setIsAddModalOpen(false)}
          destroyOnClose
          footer={null}
          open={isAddModalOpen}
          title="Добавление проекта"
          style={{ minWidth: "800px", marginTop: "1rem" }}
        >
          <AddProjectForm
            onAdded={(options: IAddProjectOptions) =>
              addAsync({ managerId: user.id, options: options })
            }
          />
        </Modal>

        <Space align="baseline" wrap>
          <Typography.Title
            level={3}
            style={{ margin: 0, alignSelf: "center" }}
          >
            Проекты
          </Typography.Title>
          {(user.isManager || user.isAdmin) && (
            <AddButton
              onClick={() => setIsAddModalOpen(true)}
              text="Добавить"
            />
          )}
        </Space>

        <Collapse
          style={{ margin: "1rem auto" }}
          items={[
            {
              key: "filters",
              label: "Фильтры",
              children: (
                <FilterProjectsForm onFilterApply={handleFilterApply} />
              ),
            },
          ]}
        />

        <Divider />

        <ProjectsTable items={items ?? []} />
      </>
    );
  }
};
