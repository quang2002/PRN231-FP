import { client } from "./index";

export async function generateToken(googleAccessToken: string): Promise<{
    success: boolean,
    message: string,
    token: string | null,
}> {
    try {
        const response = await client.post("/auth/generate-token", {
            headers: {
                "Content-Type": "application/json",
            },
            "google-access-token": googleAccessToken,
        });
        return {
            success: true,
            message: "Token generated successfully",
            token: response.data,
        };
    } catch (error: any) {
        return {
            success: false,
            message: error.response.data,
            token: null,
        };
    }
}
