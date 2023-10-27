import axios from "axios";

export const client = axios.create({
    baseURL: process.env.FP_FAP_API_BASE_URL,
    timeout: 5000,
});