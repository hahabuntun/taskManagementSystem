import { Button } from "antd";
import { ReactNode } from "react";

interface ILinkButtonProps {
  id?: number;
  handleClicked: () => void;
  children?: ReactNode;
  style?: React.CSSProperties | undefined;
  icon?: ReactNode;
}

export const LinkButton = ({
  handleClicked,
  id,
  children,
  icon,
  style,
}: ILinkButtonProps) => {
  return (
    <Button
      icon={icon}
      style={{ ...style, margin: 0, padding: 0 }}
      type="link"
      key={id}
      onClick={() => handleClicked()}
    >
      {children}
    </Button>
  );
};
