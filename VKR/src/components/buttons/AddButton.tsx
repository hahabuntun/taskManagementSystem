import React, { forwardRef } from "react";
import { PlusOutlined } from "@ant-design/icons";
import { Button } from "antd";

interface IAddButtonProps {
  onClick?: () => void;
  text?: string;
  style?: React.CSSProperties | undefined;
}

export const AddButton = forwardRef(
  (
    { onClick, text, style }: IAddButtonProps,
    ref: React.Ref<HTMLButtonElement>
  ) => {
    return (
      <Button
        ref={ref}
        style={{ ...style }}
        size="small"
        onClick={onClick}
        icon={<PlusOutlined style={{ color: "#389e0d" }} />}
      >
        {text}
      </Button>
    );
  }
);
