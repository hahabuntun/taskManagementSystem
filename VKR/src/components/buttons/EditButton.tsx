import { Button } from "antd";
import { EditOutlined } from "@ant-design/icons";
import React, { forwardRef } from "react";

interface IEditButtonProps {
  onClick?: () => void;
  itemId: number | string;
  text?: string;
  size?: "small" | "large" | "middle";
  style?: React.CSSProperties | undefined;
}

export const EditButton = forwardRef(
  (
    { onClick, itemId, text, style, size = "small" }: IEditButtonProps,
    ref: React.Ref<HTMLButtonElement>
  ) => {
    return (
      <Button
        ref={ref}
        key={itemId}
        style={style}
        size={size}
        icon={<EditOutlined style={{ color: "#096dd9" }} />}
        onClick={onClick}
      >
        {text}
      </Button>
    );
  }
);
