import useApplicationStore from "../stores/applicationStore";
import { SprintsFeature } from "../features/sprints/SprintsFeature";
import { PageOwnerEnum } from "../enums/ownerEntities/PageOwnerEnum";

export const SprintsPage = () => {
  const { user } = useApplicationStore.getState();
  if (user) {
    return (
      <>
        <SprintsFeature entityId={user.id} entityType={PageOwnerEnum.WORKER} />
      </>
    );
  }
};
