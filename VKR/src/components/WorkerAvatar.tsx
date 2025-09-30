import { Avatar, Tooltip, Typography } from "antd";
import md5 from "md5"; // Импорт библиотеки md5
import { DrawerEntityEnum } from "../enums/DrawerEntityEnum";
import { IWorkerFields } from "../interfaces/IWorkerFields";
import useApplicationStore from "../stores/applicationStore";

interface IWorkerAvatarProps {
  worker?: IWorkerFields;
  size: "default" | "large" | "small";
  disableTooltip?: boolean; // Отключает Tooltip
  disableClick?: boolean; // Отключает onClick
}

export const WorkerAvatar = ({
  worker,
  size,
  disableTooltip = false,
  disableClick = false,
}: IWorkerAvatarProps) => {
  const showDrawer = useApplicationStore((state) => state.showDrawer);

  // Функция для получения URL Gravatar
  const getGravatarUrl = (email: string, sizePx: number) => {
    const hash = md5(email.trim().toLowerCase()); // Создаем MD5 хэш из email
    return `https://www.gravatar.com/avatar/${hash}?s=${sizePx}&d=identicon`; // URL с параметром identicon для дефолтного аватара
  };

  // Определяем размер аватара в пикселях в зависимости от size
  const avatarSizePx = size === "large" ? 64 : size === "default" ? 32 : 24;

  // Компонент аватара
  const avatar = (
    <Avatar
      size={size}
      src={worker ? getGravatarUrl(worker.email, avatarSizePx) : undefined}
      style={{
        cursor: worker && !disableClick ? "pointer" : "default", // Курсор зависит от disableClick
        marginRight: "0.25rem",
        ...(worker ? {} : { backgroundColor: "#f56a00" }), // Цвет фона только если нет worker
      }}
      onClick={
        worker && !disableClick
          ? () => showDrawer(DrawerEntityEnum.WORKER, worker.id)
          : undefined
      } // onClick только если не отключен
    >
      {!worker && "Нет данных"}
    </Avatar>
  );

  if (worker) {
    return (
      <div>
        {disableTooltip ? (
          avatar
        ) : (
          <Tooltip title={<Typography.Text>{worker.email}</Typography.Text>}>
            {avatar}
          </Tooltip>
        )}
      </div>
    );
  } else {
    return <div>{avatar}</div>;
  }
};
