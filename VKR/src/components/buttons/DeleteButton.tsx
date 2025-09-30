import { Button } from "antd";
import { DeleteOutlined } from "@ant-design/icons";
import React, { forwardRef } from "react";

interface IDeleteButtonProps {
  handleClicked?: (itemId: number) => void;
  itemId: number;
  text?: string;
  style?: React.CSSProperties | undefined;
  onClick?: () => void;
}

export const DeleteButton = forwardRef(
  (
    { onClick, handleClicked, itemId, text, style }: IDeleteButtonProps,
    ref: React.Ref<HTMLButtonElement>
  ) => {
    return (
      <Button
        ref={ref}
        style={{ ...style }}
        size="small"
        icon={<DeleteOutlined style={{ color: "#dc4446" }} />}
        onClick={() => {
          handleClicked?.(itemId);
          onClick?.();
        }}
      >
        {text}
      </Button>
    );
  }
);
