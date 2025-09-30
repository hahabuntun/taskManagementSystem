import { getOrganizationQuery } from "../queries/organizationQueries";
import { useQuery } from "@tanstack/react-query";

export const useGetOrganizationData = (enabled: boolean = true) => {
  const query = useQuery({
    queryKey: ["organization"],
    enabled: enabled,
    queryFn: () => {
      return getOrganizationQuery();
    },
  });

  return {
    ...query,
  };
};
