import axios, { AxiosError, AxiosInstance, InternalAxiosRequestConfig } from "axios";

export const apiClient: AxiosInstance = axios.create({
    baseURL: 'http://localhost/api', // Use Nginx port 80
    // OR 'http://vkr-api:8080/api' if frontend is in Docker and bypassing Nginx
    timeout: 10000, // Increased timeout
});

apiClient.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = localStorage.getItem("authToken");
        if (token) {
            config.headers.Authorization = `${token}`; // Add Bearer prefix
        }
        // Set Content-Type dynamically
        if (config.data instanceof FormData) {
            delete config.headers["Content-Type"]; // Let Axios handle FormData
        } else {
            config.headers["Content-Type"] = "application/json"; // Default to JSON
        }
        return config;
    },
    (error: AxiosError) => {
        return Promise.reject(error);
    }
);