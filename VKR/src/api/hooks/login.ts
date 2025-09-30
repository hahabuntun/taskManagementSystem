import { useNavigate } from 'react-router-dom';
import { useMutation } from "@tanstack/react-query";
import { notification } from "antd";
import { loginWorkerQuery } from "../queries/loginWorkerQueries";
import useApplicationStore from "../../stores/applicationStore";
import { IWorker } from "../../interfaces/IWorker";

export const useLogin = (handleSuccess?: () => void) => {
    const navigate = useNavigate();

    const mutation = useMutation({
        mutationFn: ({ email, password }: { email: string; password: string }) =>
            loginWorkerQuery(email, password),
        onSuccess: (user: IWorker) => {
            handleSuccess?.();
            const { setUser } = useApplicationStore.getState();
            setUser(user);
            navigate("/tasks");
            notification.success({ message: "Вы вошли в систему" });
        },
        onError: (error: any) => {
            notification.error({ message: `Ошибка при входе: ${error.message || "Неизвестная ошибка"}` });
        },
    });

    return mutation.mutateAsync;
};

export const useLogout = () => {

}

