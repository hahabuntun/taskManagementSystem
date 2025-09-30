import { apiClient } from "../../config/axiosConfig";
import { IWorker } from "../../interfaces/IWorker";
import { getWorkerQuery } from "./workersQueries";
import { jwtDecode } from "jwt-decode";


interface JwtPayload {
    userId: string;
    canManageWorkers: string;
    canManageProjects: string;
    exp: number;
}

export const loginWorkerQuery = async (
    email: string,
    password: string
): Promise<IWorker> => {

    const loginResponse = await apiClient.post("/auth/login", {
        email,
        password,
    });

    const token: string = loginResponse.data.Token;

    if (!token) {
        throw new Error("No token received");
    }

    const decoded: JwtPayload = jwtDecode(token);
    const userId = parseInt(decoded.userId);

    if (!userId) {
        throw new Error("Invalid user ID in token");
    }

    // Сохраняем токен в localStorage
    localStorage.setItem("authToken", token);
    // Токен автоматически добавится к следующим запросам через интерцептор

    // Получаем данные работника
    const worker = await getWorkerQuery(userId);

    return worker;
};