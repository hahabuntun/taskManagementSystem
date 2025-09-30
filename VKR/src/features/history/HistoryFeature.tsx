import { useDeleteHistory, useGetHistory } from "../../api/hooks/history";
import InfiniteScroll from "react-infinite-scroll-component";
import { HistoryTable } from "./HistoryTable";
import { Collapse, Divider, Space, Typography } from "antd";
import { FilterHistoryForm } from "./FilterHistoryForm";
import { DeleteButton } from "../../components/buttons/DeleteButton";
import { IHistoryFilterOptions } from "../../api/options/filterOptions/IHistoryFilterOptions";
import useApplicationStore from "../../stores/applicationStore";
import { HistoryOwnerEnum } from "../../enums/ownerEntities/HistoryOwnerEnum";

interface IHistoryFeatureProps {
  ownerType: HistoryOwnerEnum;
  ownerId: number;
}

export const HistoryFeature = ({
  ownerType,
  ownerId,
}: IHistoryFeatureProps) => {
  const { data, fetchNextPage, hasNextPage, setFilters } = useGetHistory(
    ownerId,
    ownerType
  );
  const items = data?.pages.flat() || [];

  const { user } = useApplicationStore.getState();

  const deleteAllItems = useDeleteHistory();

  const handleFilterApply = (filters: IHistoryFilterOptions) => {
    setFilters(filters);
  };

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          История
        </Typography.Title>
        {(user &&
          (ownerType === HistoryOwnerEnum.WORKER ||
            ownerType === HistoryOwnerEnum.ORGANIZATION) &&
          user.isAdmin) ||
          ownerType === HistoryOwnerEnum.PROJECT ||
          (ownerType === HistoryOwnerEnum.TASK && (
            <DeleteButton
              itemId={ownerId}
              handleClicked={(itemId: number) =>
                deleteAllItems({ ownerType: ownerType, ownerId: itemId })
              }
              text="Очистить историю"
            />
          ))}
      </Space>

      <Collapse
        style={{ margin: "1rem auto" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: (
              <FilterHistoryForm
                ownerId={ownerId}
                ownerType={ownerType}
                onFilterApply={handleFilterApply}
              />
            ),
          },
        ]}
      />

      <Divider />

      <InfiniteScroll
        dataLength={items.length}
        next={fetchNextPage}
        hasMore={!!hasNextPage}
        loader={null}
      >
        <HistoryTable items={items} />
      </InfiniteScroll>
    </>
  );
};
