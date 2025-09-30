import { Card, Tag, List, Spin, Typography } from "antd";
import { useGetNumTasksForEachTag } from "../../api/hooks/tags";
import { useNavigate } from "react-router-dom";

const TagSearchPage = () => {
  const navigate = useNavigate();
  const { data, isLoading, isError } = useGetNumTasksForEachTag(true);

  if (isLoading) {
    return <Spin tip="Загрузка тегов..." />;
  }

  if (isError) {
    return <div>Ошибка загрузки тегов</div>;
  }

  return (
    <Card title="Поиск по тегам">
      <List
        grid={{ gutter: 16, xs: 1, sm: 2, md: 3 }}
        dataSource={data || []}
        renderItem={({ tag, numTasks }) => (
          <List.Item>
            <Tag
              color={tag.color || "blue"}
              style={{
                cursor: "pointer",
                padding: "8px 12px",
                fontSize: "14px",
              }}
              onClick={() => navigate(`/tags/${tag.name}/tasks`)}
            >
              <Typography.Text style={{ color: "black" }}>
                {tag.name} ({numTasks})
              </Typography.Text>
            </Tag>
          </List.Item>
        )}
      />
    </Card>
  );
};

export default TagSearchPage;
