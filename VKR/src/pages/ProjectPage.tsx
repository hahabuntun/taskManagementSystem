import { useParams } from "react-router-dom";
import { ProjectFeature } from "../features/projects/project/ProjectFeautre";

export const ProjectPage = () => {
  const { id } = useParams();

  if (id) {
    return <ProjectFeature projectId={+id} />;
  }
};
