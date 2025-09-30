import { useParams } from "react-router-dom";
import { useGetBoard } from "../api/hooks/boards";
import { BoardFeature } from "../features/boards/board/BoardFeature";

export const BoardPage = () => {
  const { boardId } = useParams();
  if (boardId) {
    const { data, isLoading } = useGetBoard(+boardId); // Добавляем isLoading
    if (isLoading) return <div>Загрузка...</div>; // Показываем индикатор загрузки
    if (data) {
      return (
        <>
          <BoardFeature key={+boardId} board={data} />
        </>
      );
    }
  }
  return <div>Доска не найдена</div>;
};
