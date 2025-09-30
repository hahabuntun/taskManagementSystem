declare module "wx-react-gantt" {
  import React from "react";
  // Interface for a single dependency
  interface IGanttDependency {
    id: string | number; // ID of the related task
    type: string; // Dependency type (e.g., "start-start", "finish-start")
    lag?: number; // Optional lag in days
  }
  interface IGanttTask {
    id: string | number;
    start: Date;
    end?: Date;
    duration?: number;
    text?: string;
    progress?: number;
    parent?: string | number;
    type?: "task" | "summary" | "milestone" | any; // Can be custom type
    open?: boolean;
    lazy?: boolean;
    base_start?: Date;
    base_end?: Date;
    base_duration?: number;
    dependencies?: IGanttDependency[]; // Add dependencies array
  }
  interface GanttProps {
    tasks?: Array<IGanttTask>;
    readonly?: boolean;
    init?: (api: any) => void;
    [key: string]: any; // Allow additional props
  }
  const Gantt: React.FC<GanttProps>;
  const WillowDark: React.FC<{ children?: React.ReactNode }>;
  const Willow: React.FC<{ children?: React.ReactNode }>;
  export { Gantt, IGanttTask, WillowDark, Willow };
}