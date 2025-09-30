import { PageOwnerEnum } from "../enums/ownerEntities/PageOwnerEnum";
import { ProjectsFeature } from "../features/projects/ProjectsFeature";
import useApplicationStore from "../stores/applicationStore";

export const ProjectsPage = () => {
  const { user } = useApplicationStore.getState();

  if (user) {
    return (
      <>
        <ProjectsFeature entityType={PageOwnerEnum.WORKER} entityId={user.id} />
      </>
    );
  }
};
