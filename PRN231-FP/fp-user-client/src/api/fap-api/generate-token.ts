import { client } from "./index";

export async function generateToken(googleAccessToken: string): Promise<{
    success: boolean,
    message: string,
    token: string | null,
}> {
    try {
        const response = await client.get("/auth/generate-token", {
            headers: {
                "Content-Type": "application/json",
            },
            data: {
                "google-access-token": googleAccessToken,
            }
        });
        return response.data;
    } catch (error: any) {
        return error.response.data;
    }
}
