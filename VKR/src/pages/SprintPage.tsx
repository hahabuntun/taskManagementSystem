import { useParams } from "react-router-dom";
import { SprintFeature } from "../features/sprints/sprint/SprintFeature";

export const SprintPage = () => {
  const { sprintId } = useParams();
  if (sprintId) {
    return <SprintFeature sprintId={+sprintId} />;
  }
};
