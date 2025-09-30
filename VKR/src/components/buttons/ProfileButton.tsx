import { Button } from "antd";
import { ProfileOutlined } from "@ant-design/icons";
import React, { forwardRef } from "react";

interface IProfileButtonProps {
  itemId: number;
  text?: string;
  style?: React.CSSProperties | undefined;
  onClick?: () => void;
}

export const ProfileButton = forwardRef(
  (
    { onClick, text, style }: IProfileButtonProps,
    ref: React.Ref<HTMLButtonElement>
  ) => {
    return (
      <Button
        ref={ref}
        style={{ ...style }}
        onClick={onClick}
        size="small"
        icon={<ProfileOutlined style={{ color: "#08979c" }} />}
      >
        {text}
      </Button>
    );
  }
);
