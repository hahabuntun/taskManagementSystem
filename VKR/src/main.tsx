import { StrictMode, useEffect } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { OrganizationPage } from "./pages/OrganizationPage.tsx";
import { ProjectsPage } from "./pages/ProjectsPage.tsx";
import { ProjectPage } from "./pages/ProjectPage.tsx";
import { TasksPage } from "./pages/TasksPage.tsx";
import { BoardsPage } from "./pages/BoardsPage.tsx";
import { BoardPage } from "./pages/BoardPage.tsx";

import { ConfigProvider, notification } from "antd";
import ruRU from "antd/locale/ru_RU";
import { SprintsPage } from "./pages/SprintsPage.tsx";
import { SprintPage } from "./pages/SprintPage.tsx";
import { NotificationsPage } from "./pages/NotificationsPage.tsx";
import { TaskTemplatesPage } from "./pages/TaskTemplatesPage.tsx";
import TagSearchPage from "./features/tasks/TagSearch.tsx";
import TagTasksPage from "./features/tasks/TagTasks.tsx";
import useApplicationStore from "./stores/applicationStore.ts";
import { getThemeConfig } from "./config/themeConfig.ts";
// import { getThemeConfig } from "./config/themeConfig.ts";

const queryClient = new QueryClient();

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "/organization",
        element: <OrganizationPage />,
      },
      {
        path: "/projects",
        element: <ProjectsPage />,
      },
      {
        path: "/projects/:id",
        element: <ProjectPage />,
      },
      {
        path: "/tasks",
        element: <TasksPage />,
      },
      {
        path: "/taskTemplates",
        element: <TaskTemplatesPage />,
      },
      {
        path: "/taskTags",
        element: <TagSearchPage />,
      },
      {
        path: "/tags/:tagName/tasks",
        element: <TagTasksPage />,
      },
      {
        path: "/boards",
        element: <BoardsPage />,
      },
      {
        path: "/boards/:boardId",
        element: <BoardPage />,
      },
      {
        path: "/sprints",
        element: <SprintsPage />,
      },
      {
        path: "/sprints/:sprintId",
        element: <SprintPage />,
      },
      {
        path: "/notifications",
        element: <NotificationsPage />,
      },
    ],
  },
]);

function RootComponent() {
  const { isDarkMode } = useApplicationStore();
  useEffect(() => {
    notification.config({
      placement: "bottomRight",
      duration: 3, // Auto-close after 3 seconds (optional)
    });
  }, []);
  return (
    <ConfigProvider theme={getThemeConfig(isDarkMode)} locale={ruRU}>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </ConfigProvider>
  );
}

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <RootComponent />
  </StrictMode>
);
