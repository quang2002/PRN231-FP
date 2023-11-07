import { client } from "./index";

export async function getUserInfo(jwtToken: string): Promise<{
    id: any,
    email: string,
    role: "admin" | "teacher" | "student",
} | string> {
    try {
        const response = await client.get("/user/me", {
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${jwtToken}`,
            },
        });
        return response.data;
    } catch (error: any) {
        return error.response.data;
    }
}
