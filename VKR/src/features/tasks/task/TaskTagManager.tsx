import { useState, useEffect } from "react";
import {
  Select,
  Space,
  Tag,
  Input,
  Button,
  notification,
  Typography,
} from "antd";
import { PlusOutlined } from "@ant-design/icons";
import {
  useGetAllTags,
  useAddExistingTaskTag,
  useAddNewTaskTag,
  useDeleteTaskTag,
  useUpdateTaskTags,
} from "../../../api/hooks/tags";
import { TagOwnerEnum } from "../../../enums/ownerEntities/TagOwnerEnum";
import { AddTagDTO, AddTagsDTO, CreateTagDTO } from "../../../interfaces/ITag";
import { useGetTask } from "../../../api/hooks/tasks";

interface TaskTagManagerProps {
  taskId: number;
}

export const TaskTagManager = ({ taskId }: TaskTagManagerProps) => {
  const [selectedExistingTagIds, setSelectedExistingTagIds] = useState<
    number[]
  >([]);
  const [selectedExistingTag, setSelectedExistingTag] = useState<number | null>(
    null
  );
  const [newTagName, setNewTagName] = useState<string>("");
  const [newTagColor, setNewTagColor] = useState<string>("#1890ff");
  const [newTags, setNewTags] = useState<{ name: string; color: string }[]>([]);
  const { data: task, isLoading: taskLoading } = useGetTask(taskId);
  const { data: existingTags = [], isLoading: tagsLoading } = useGetAllTags(
    TagOwnerEnum.TASK
  );
  const addExistingTagMutation = useAddExistingTaskTag();
  const addNewTagMutation = useAddNewTaskTag();
  const deleteTagMutation = useDeleteTaskTag();
  const updateTagsMutation = useUpdateTaskTags();

  useEffect(() => {
    if (task?.tags) {
      setSelectedExistingTagIds(task.tags.map((tag) => tag.id));
      setNewTags([]);
    }
  }, [task]);

  const isTagNameExists = (name: string) =>
    newTags.some((tag) => tag.name.toLowerCase() === name.toLowerCase()) ||
    existingTags.some((tag) => tag.name.toLowerCase() === name.toLowerCase());

  const handleAddExistingTag = async () => {
    if (
      selectedExistingTag &&
      !selectedExistingTagIds.includes(selectedExistingTag)
    ) {
      const tag: AddTagDTO = { tagId: selectedExistingTag };
      try {
        const result = await addExistingTagMutation.mutateAsync({
          taskId,
          tag,
        });
        if (result.success) {
          setSelectedExistingTagIds([
            ...selectedExistingTagIds,
            selectedExistingTag,
          ]);
          setSelectedExistingTag(null);
          notification.success({ message: "Тег добавлен" });
        } else {
          notification.error({ message: "Ошибка при добавлении тега" });
        }
      } catch {
        notification.error({ message: "Ошибка при добавлении тега" });
      }
    }
  };

  const handleAddNewTag = async () => {
    if (newTagName.trim() && !isTagNameExists(newTagName.trim())) {
      const tag: CreateTagDTO = { name: newTagName.trim(), color: newTagColor };
      try {
        const result = await addNewTagMutation.mutateAsync({ taskId, tag });
        if (result.id) {
          setNewTags([
            ...newTags,
            { name: newTagName.trim(), color: newTagColor },
          ]);
          setNewTagName("");
          setNewTagColor("#1890ff");
          notification.success({ message: "Новый тег добавлен" });
        } else {
          notification.error({ message: "Ошибка при добавлении нового тега" });
        }
      } catch {
        notification.error({ message: "Ошибка при добавлении нового тега" });
      }
    }
  };

  const handleRemoveExistingTag = async (tagId: number) => {
    try {
      const result = await deleteTagMutation.mutateAsync({ taskId, tagId });
      if (result.success) {
        setSelectedExistingTagIds(
          selectedExistingTagIds.filter((id) => id !== tagId)
        );
        notification.success({ message: "Тег удален" });
      } else {
        notification.error({ message: "Ошибка при удалении тега" });
      }
    } catch {
      notification.error({ message: "Ошибка при удалении тега" });
    }
  };

  const handleRemoveNewTag = async (tagName: string) => {
    const updatedNewTags = newTags.filter((tag) => tag.name !== tagName);
    const tags: AddTagsDTO = {
      existingTagIds: selectedExistingTagIds,
      newTags: updatedNewTags.map((tag) => ({
        name: tag.name,
        color: tag.color,
      })),
    };
    try {
      const result = await updateTagsMutation.mutateAsync({ taskId, tags });
      if (result.success) {
        setNewTags(updatedNewTags);
        notification.success({ message: "Новый тег удален" });
      } else {
        notification.error({ message: "Ошибка при удалении нового тега" });
      }
    } catch {
      notification.error({ message: "Ошибка при удалении нового тега" });
    }
  };

  if (taskLoading || tagsLoading) {
    return <div>Загрузка...</div>;
  }

  return (
    <Space direction="vertical" style={{ width: "100%" }}>
      <Space wrap>
        <Select
          placeholder="Выберите существующий тег"
          loading={tagsLoading}
          style={{ width: "300px" }}
          value={selectedExistingTag}
          onChange={setSelectedExistingTag}
        >
          {existingTags
            .filter((tag) => !selectedExistingTagIds.includes(tag.id))
            .map((tag) => (
              <Select.Option key={tag.id} value={tag.id}>
                <Tag color={tag.color}>
                  <Typography.Text style={{ color: "black" }}>
                    {tag.name}
                  </Typography.Text>
                </Tag>
              </Select.Option>
            ))}
        </Select>
        <Button
          type="primary"
          size="small"
          icon={<PlusOutlined />}
          onClick={handleAddExistingTag}
          disabled={!selectedExistingTag}
        >
          Добавить
        </Button>
      </Space>

      <Space wrap>
        <Input
          value={newTagName}
          onChange={(e) => setNewTagName(e.target.value)}
          placeholder="Новый тег"
          style={{ width: "300px" }}
          onPressEnter={handleAddNewTag}
        />
        <Input
          type="color"
          value={newTagColor}
          onChange={(e) => setNewTagColor(e.target.value)}
          style={{ width: "50px" }}
        />
        <Button
          type="primary"
          size="small"
          icon={<PlusOutlined />}
          onClick={handleAddNewTag}
          disabled={
            newTagName.trim() === "" || isTagNameExists(newTagName.trim())
          }
        >
          Добавить
        </Button>
      </Space>

      <Space wrap style={{ marginTop: "10px" }}>
        {selectedExistingTagIds.map((tagId) => {
          const tag = existingTags.find((t) => t.id === tagId);
          return tag ? (
            <Tag
              key={tag.id}
              color={tag.color}
              closable
              onClose={() => handleRemoveExistingTag(tag.id)}
            >
              <Typography.Text style={{ color: "black" }}>
                {tag.name}
              </Typography.Text>
            </Tag>
          ) : null;
        })}
        {newTags.map((tag) => (
          <Tag
            key={tag.name}
            color={tag.color}
            closable
            onClose={() => handleRemoveNewTag(tag.name)}
          >
            <Typography.Text style={{ color: "black" }}>
              {tag.name}
            </Typography.Text>
          </Tag>
        ))}
      </Space>
    </Space>
  );
};
