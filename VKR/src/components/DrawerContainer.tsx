import React, { useState, useEffect, useRef } from "react";
import { Drawer, Spin, Typography } from "antd";
import "./ResizableDrawer.css"; // External CSS for shadows
import { useShallow } from "zustand/react/shallow";
import { WorkerFeature } from "../features/workers/worker/WorkerFeature";
import { TaskFeature } from "../features/tasks/task/TaskFeature";
import { useGetTask } from "../api/hooks/tasks";
import { DrawerEntityEnum } from "../enums/DrawerEntityEnum";
import { TemplateFeature } from "../features/tasks/taskTemplates/TemplateFeature";
import useApplicationStore from "../stores/applicationStore";

const ResizableDrawer = () => {
  const [drawerWidth, setDrawerWidth] = useState(600); // Initial width
  const resizeHandleRef = useRef<HTMLDivElement | null>(null);
  const [isResizing, setIsResizing] = useState(false);
  const [startX, setStartX] = useState(0);
  const [initialWidth, setInitialWidth] = useState(drawerWidth);

  // Select state from the unified store
  const { isDrawerVisible, hideDrawer, drawerEntityId, drawerEntityType } =
    useApplicationStore(
      useShallow((state) => ({
        isDrawerVisible: state.isDrawerVisible,
        hideDrawer: state.hideDrawer,
        drawerEntityId: state.drawerEntityId,
        drawerEntityType: state.drawerEntityType,
      }))
    );

  // Fetch task data only if entityType is TASK and entityId is valid
  const { data: task, isLoading: isTaskLoading } = useGetTask(
    drawerEntityId ?? 0,
    drawerEntityType === DrawerEntityEnum.TASK && drawerEntityId !== null
  );

  // Handle resize start
  const handleResizeStart = (e: React.MouseEvent) => {
    setIsResizing(true);
    setStartX(e.clientX);
    setInitialWidth(drawerWidth);
    document.body.style.userSelect = "none";
    document.body.style.cursor = "col-resize";
  };

  // Handle resize move
  const handleResizeMove = (e: MouseEvent) => {
    if (!isResizing) return;
    const newWidth = initialWidth - (e.clientX - startX);
    // Enforce min/max width
    if (newWidth >= 600 && newWidth <= 1500) {
      setDrawerWidth(newWidth);
    }
  };

  // Handle resize end
  const handleResizeEnd = () => {
    setIsResizing(false);
    document.body.style.userSelect = "";
    document.body.style.cursor = "";
  };

  // Add global event listeners for resizing
  useEffect(() => {
    if (!isResizing) return;

    document.addEventListener("mousemove", handleResizeMove);
    document.addEventListener("mouseup", handleResizeEnd);

    return () => {
      document.removeEventListener("mousemove", handleResizeMove);
      document.removeEventListener("mouseup", handleResizeEnd);
    };
  }, [isResizing, initialWidth, startX]);

  // Determine component to render
  let Component = null;
  if (drawerEntityType === DrawerEntityEnum.WORKER && drawerEntityId !== null) {
    Component = <WorkerFeature workerId={drawerEntityId} />;
  } else if (
    drawerEntityType === DrawerEntityEnum.TASK &&
    drawerEntityId !== null
  ) {
    if (isTaskLoading) {
      Component = (
        <div style={{ textAlign: "center", padding: "20px" }}>
          <Spin tip="Загрузка задачи..." />
        </div>
      );
    } else if (task) {
      Component = <TaskFeature task={task} />;
    } else {
      Component = <Typography.Text>Задача не найдена</Typography.Text>;
    }
  } else if (
    drawerEntityType === DrawerEntityEnum.TASK_TEMPLATE &&
    drawerEntityId !== null
  ) {
    Component = <TemplateFeature templateId={drawerEntityId} />;
  }

  return (
    <Drawer
      placement="right"
      closable
      onClose={hideDrawer}
      open={isDrawerVisible}
      width={drawerWidth}
    >
      {Component}
      {/* Resize handle */}
      <div
        ref={resizeHandleRef}
        className="resize-handle"
        onMouseDown={handleResizeStart}
        role="separator"
        aria-label="Resize drawer"
        style={{
          position: "absolute",
          top: 0,
          left: -5,
          width: 5,
          height: "100%",
          background: "#d9d9d9",
          cursor: "col-resize",
          zIndex: 10,
        }}
      />
    </Drawer>
  );
};

export default ResizableDrawer;
