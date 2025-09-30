import { useAddFile, useGetFiles } from "../../api/hooks/files";
import InfiniteScroll from "react-infinite-scroll-component";
import { FilesTable } from "./FilesTable";
import { Collapse, Divider, Popover, Space, Typography } from "antd";
import { AddFileForm } from "./AddFileForm";
import { IAddFileOptions } from "../../api/options/createOptions/IAddFileOptions";
import { IFileFilterOptions } from "../../api/options/filterOptions/IFileFilterOptions";
import { FilterFilesForm } from "./FilterFilesForm";
import { AddButton } from "../../components/buttons/AddButton";
import useApplicationStore from "../../stores/applicationStore";
import { FileOwnerEnum } from "../../enums/ownerEntities/FileOwnerEnum";

interface IFilesFeatureProps {
  ownerType: FileOwnerEnum;
  ownerId: number;
}

export const FilesFeature = ({ ownerType, ownerId }: IFilesFeatureProps) => {
  const { data, fetchNextPage, hasNextPage, setFilters } = useGetFiles(
    ownerId,
    ownerType
  );
  const items = data?.pages.flat() ?? [];

  const { user } = useApplicationStore.getState();

  const addItemAsync = useAddFile();

  const handleFilterApply = (filters: IFileFilterOptions) => {
    setFilters(filters);
  };

  return (
    <>
      <Space align="baseline" wrap>
        <Typography.Title level={3} style={{ margin: 0, alignSelf: "center" }}>
          Файлы
        </Typography.Title>
        {user && user.isAdmin && (
          <Popover
            trigger="click"
            title="Добавление файла"
            content={
              <AddFileForm
                onAddFile={(options: IAddFileOptions) => {
                  addItemAsync({
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
        )}
      </Space>

      <Collapse
        style={{ margin: "1rem auto" }}
        items={[
          {
            key: "filters",
            label: "Фильтры",
            children: (
              <FilterFilesForm
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
        <FilesTable items={items} />
      </InfiniteScroll>
    </>
  );
};
