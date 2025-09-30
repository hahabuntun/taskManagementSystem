import { useLogin } from "../../api/hooks/login";
import { Button, Flex, Form, Input } from "antd";
import { EyeInvisibleOutlined, EyeTwoTone } from "@ant-design/icons";

interface ILoginFormValues {
  email: string;
  password: string;
}

export const LoginForm = () => {
  const login = useLogin();

  const onFormFinish = async (values: ILoginFormValues) => {
    try {
      await login({
        email: values.email,
        password: values.password,
      });
    } catch (error) {
      console.error("Ошибка при входе:", error);
    }
  };

  return (
    <Flex justify="center" align="center">
      <div
        style={{
          width: "100%",
          maxWidth: "400px",
          padding: "2rem",
          borderRadius: "8px",
          boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)",
        }}
      >
        <h2 style={{ textAlign: "center", marginBottom: "1.5rem" }}>Вход</h2>
        <Form<ILoginFormValues> onFinish={onFormFinish} layout="vertical">
          <Form.Item
            name="email"
            label="Email"
            rules={[{ required: true, message: "Пожалуйста, введите Email" }]}
          >
            <Input title="" allowClear placeholder="Введите Email" />
          </Form.Item>
          <Form.Item
            tooltip={null}
            name="password"
            label="Пароль"
            rules={[{ required: true, message: "Пожалуйста, введите пароль" }]}
          >
            <Input.Password
              placeholder="Введите пароль"
              iconRender={(visible) =>
                visible ? (
                  <EyeTwoTone data-testid="password-toggle-icon" />
                ) : (
                  <EyeInvisibleOutlined data-testid="password-toggle-icon" />
                )
              }
            />
          </Form.Item>
          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              size="large"
              style={{ width: "100%" }}
            >
              Войти
            </Button>
          </Form.Item>
        </Form>
      </div>
    </Flex>
  );
};
