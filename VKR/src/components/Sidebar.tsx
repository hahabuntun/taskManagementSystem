import { Menu, MenuProps, Layout, theme, Flex, Button } from "antd";
const { Sider } = Layout;
import {
  ProjectOutlined,
  NotificationOutlined,
  ReconciliationOutlined,
  IdcardOutlined,
  CalendarOutlined,
  BlockOutlined,
  ScheduleOutlined,
  MenuFoldOutlined,
  MenuUnfoldOutlined,
} from "@ant-design/icons";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import useApplicationStore from "../stores/applicationStore";
import { DrawerEntityEnum } from "../enums/DrawerEntityEnum";

// Define the navigation items interface
interface INavigationItem {
  key: string;
  target: string;
}

// Define the MenuItem type
type MenuItem = Required<MenuProps>["items"][number];

// Navigation data
const navigation: INavigationItem[] = [
  { key: "1", target: "/profile" },
  { key: "2", target: "/organization" },
  { key: "3", target: "/projects" },
  { key: "4-1", target: "/tasks" }, // Подменю для задач
  { key: "4-2", target: "/taskTemplates" }, // Подменю для шаблонов
  { key: "4-3", target: "/taskTags" },
  { key: "5", target: "/boards" },
  { key: "6", target: "/sprints" },
  { key: "7", target: "/notifications" },
];

// Menu items with icons and labels
const menuItems: MenuItem[] = [
  { icon: <IdcardOutlined />, label: "Профиль", key: "1", title: "Профиль" },
  {
    icon: <ReconciliationOutlined />,
    label: "Организация",
    key: "2",
    title: "Организация",
  },
  { icon: <ProjectOutlined />, label: "Проекты", key: "3", title: "Проекты" },
  {
    icon: <CalendarOutlined />,
    label: "Задачи",
    key: "4",
    children: [
      { label: "Задачи", key: "4-1", title: "Задачи" },
      { label: "Шаблоны", key: "4-2", title: "Шаблоны" },
      { label: "Теги", key: "4-3", title: "Теги" },
    ],
  },
  { icon: <BlockOutlined />, label: "Доски", key: "5", title: "Доски" },
  { icon: <ScheduleOutlined />, label: "Спринты", key: "6", title: "Спринты" },
  {
    icon: <NotificationOutlined />,
    label: "Уведомления",
    key: "7",
    title: "Уведомления",
  },
];

// Sidebar component
interface ISidebarProps {}

export const Sidebar: React.FC<ISidebarProps> = () => {
  const user = useApplicationStore((state) => state.user);
  const showDrawer = useApplicationStore((state) => state.showDrawer);
  const [collapsed, setCollapsed] = useState(false); // Состояние для сворачивания

  const navigate = useNavigate();
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  // Handle menu item click
  const handleMenuItemClicked = ({ key }: { key: string }) => {
    if (key === "1" && user) {
      showDrawer(DrawerEntityEnum.WORKER, user.id);
    } else {
      const navigationItem = navigation.find((item) => item.key === key);
      if (navigationItem?.target) {
        navigate(navigationItem.target);
      } else {
        console.warn(`No navigation target found for key: ${key}`);
      }
    }
  };

  // Toggle collapse state
  const toggleCollapsed = () => {
    setCollapsed(!collapsed);
  };

  return (
    <Sider
      style={{
        height: "100vh",
        background: colorBgContainer,
      }}
      trigger={null}
      collapsible
      collapsed={collapsed}
    >
      <Flex vertical justify="space-between" style={{ height: "100%" }}>
        <Menu
          selectedKeys={[]}
          mode="inline"
          onClick={handleMenuItemClicked}
          items={menuItems}
          style={{ flex: 1 }} // Занимает всё доступное пространство
        />

        <Flex
          vertical
          align="center"
          justify="space-between"
          style={{
            width: "100%",
          }}
        >
          <Button
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={toggleCollapsed}
            style={{
              height: 40,
              width: "100%",
              borderRadius: 0,
              background: colorBgContainer,
              transition: "width 0.2s",
            }}
          />
        </Flex>
      </Flex>
    </Sider>
  );
};
