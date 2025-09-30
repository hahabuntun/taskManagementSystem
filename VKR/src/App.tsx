import { Layout, theme, Dropdown, Button, Flex } from "antd";
import { Sidebar } from "./components/Sidebar";
import { Outlet } from "react-router-dom";
import ResizableDrawer from "./components/DrawerContainer";
import useApplicationStore from "./stores/applicationStore";
import { LoginForm } from "./features/login/LoginForm";
import {
  LogoutOutlined,
  MoonOutlined,
  SunOutlined,
  UserOutlined,
} from "@ant-design/icons";
import { DrawerEntityEnum } from "./enums/DrawerEntityEnum";
import "./App.css";
import { WorkerAvatar } from "./components/WorkerAvatar";

function App() {
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();

  const user = useApplicationStore((state) => state.user);
  const setUser = useApplicationStore((state) => state.setUser);
  const showDrawer = useApplicationStore((state) => state.showDrawer);
  const isDarkMode = useApplicationStore((state) => state.isDarkMode);
  const toggleTheme = useApplicationStore((state) => state.toggleTheme);

  const handleLogout = () => {
    setUser(null);
  };

  const menuItems = [
    {
      key: "profile",
      icon: <UserOutlined data-testid="menu-item-profile" />,
      label: "Профиль",
      onClick: () => showDrawer(DrawerEntityEnum.WORKER, user?.id),
    },
    {
      key: "logout",
      icon: <LogoutOutlined data-testid="menu-item-logout" />,
      label: "Выйти",
      onClick: handleLogout,
    },
  ];
  const layoutBackground = isDarkMode ? "#374151" : "#f0f0f0";

  return (
    <Layout
      style={{
        display: "flex",
        overflowY: "hidden",
        position: "relative",
        maxHeight: "100vh",
      }}
    >
      {!user && (
        <div
          style={{
            position: "absolute",
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backdropFilter: "blur(5px)",
            zIndex: 1,
          }}
        >
          <LoginForm data-testid="login-form" />
        </div>
      )}

      <Sidebar data-testid="sidebar" />

      <Layout
        style={{
          flex: 1,
          display: "flex",
          flexDirection: "column",
          background: layoutBackground,
        }}
      >
        <Layout.Header
          style={{
            padding: "0 16px",
            height: "5vh",
            display: "flex",
            justifyContent: "flex-end",
            alignItems: "center",
            background: colorBgContainer,
          }}
        >
          <Flex gap="small" align="center">
            {user && (
              <Dropdown menu={{ items: menuItems }} trigger={["hover"]}>
                <div style={{ cursor: "pointer" }}>
                  <WorkerAvatar
                    data-testid="worker-avatar"
                    size="default"
                    worker={user}
                    disableTooltip={true}
                  />
                </div>
              </Dropdown>
            )}
          </Flex>
        </Layout.Header>

        <Layout.Content
          style={{
            margin: "24px 16px 0",
            padding: "0",
            flex: 1,
            overflow: "auto",
          }}
        >
          <div
            data-testid="outlet"
            style={{
              padding: "24px",
              borderRadius: borderRadiusLG,
              background: colorBgContainer,
              minHeight: "85vh",
              minWidth: "500px",
              width: "100%",
              boxSizing: "border-box",
              paddingBottom: "2rem",
            }}
          >
            <Outlet />
          </div>
        </Layout.Content>

        <ResizableDrawer />

        <Layout.Footer
          style={{
            height: "5vh",
            textAlign: "center",
            transition: "0.1s ease-in",
            background: layoutBackground,
          }}
        >
          КНИТУ КАИ ©{new Date().getFullYear()} Создано студентами ПМИ
        </Layout.Footer>

        <Button
          aria-label={isDarkMode ? "sun" : "moon"}
          style={{
            position: "fixed",
            bottom: "20px",
            right: "20px",
            width: "40px",
            height: "40px",
            borderRadius: "50%", // Makes it round
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            cursor: "pointer",
            fontSize: "20px",
            background: isDarkMode ? "#ffffff" : colorBgContainer,
            zIndex: 1000, // Ensures it stays above other content
            border: "none", // Optional: removes default border
            boxShadow: "0 4px 16px rgba(0, 0, 0, 0.25)", // Bigger shadow
          }}
          onClick={toggleTheme}
        >
          {isDarkMode ? (
            <SunOutlined style={{ color: colorBgContainer }} />
          ) : (
            <MoonOutlined style={{ color: "#000000" }} />
          )}
        </Button>
      </Layout>
    </Layout>
  );
}

export default App;
