import { IAddFileOptions } from "../../api/options/createOptions/IAddFileOptions";
import { Button, Form, Input, message, Upload } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import { useState } from "react";
import {
  RcFile,
  UploadFile,
  UploadChangeParam,
} from "antd/es/upload/interface";

interface IAddFileFormProps {
  onAddFile: (options: IAddFileOptions) => void;
}

interface IFormValues {
  name: string;
  description: string;
}

export const AddFileForm = ({ onAddFile }: IAddFileFormProps) => {
  const [form] = Form.useForm();
  const [file, setFile] = useState<RcFile | null>(null);
  const [fileList, setFileList] = useState<UploadFile[]>([]);

  const onFormFinish = (values: IFormValues) => {
    if (!file) {
      message.warning("Пожалуйста, выберите файл для загрузки!");
      return;
    }

    const options: IAddFileOptions = {
      file,
      name: values.name,
      description: values.description,
    };

    try {
      onAddFile(options);
      form.resetFields();
      setFile(null);
      setFileList([]);
    } catch (error) {}
  };

  const handleUploadChange = (info: UploadChangeParam<UploadFile<any>>) => {
    // Directly use info.fileList to avoid type issues
    const updatedFileList = info.fileList.map((file) => ({
      ...file,
      status: file.status || ("success" as const), // Ensure status is a valid UploadFileStatus
    })) as UploadFile[];

    setFileList(updatedFileList);

    // Update the file state if a file is selected
    if (updatedFileList.length > 0) {
      const selectedFile = updatedFileList[0].originFileObj as RcFile;
      setFile(selectedFile);
    } else {
      setFile(null);
    }
  };

  return (
    <Form form={form} onFinish={onFormFinish}>
      <Form.Item
        name="name"
        label="Название файла"
        rules={[{ required: true, message: "Введите название файла" }]}
      >
        <Input placeholder="Введите название" />
      </Form.Item>
      <Form.Item
        name="description"
        label="Описание"
        rules={[{ required: true, message: "Введите описание файла" }]}
      >
        <Input placeholder="Введите описание" />
      </Form.Item>
      <Form.Item label="Файл" required>
        <Upload
          name="file"
          beforeUpload={() => false}
          onChange={handleUploadChange}
          fileList={fileList}
          maxCount={1}
        >
          <Button icon={<UploadOutlined />}>Выбрать файл</Button>
        </Upload>
      </Form.Item>
      <Form.Item>
        <Button
          htmlType="submit"
          style={{ width: "100%", background: "green" }}
          type="primary"
        >
          Добавить
        </Button>
      </Form.Item>
    </Form>
  );
};
