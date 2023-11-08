import { baseURL } from ".";

export async function generateToken(googleAccessToken: string): Promise<{
    success: boolean,
    message: string,
    token: string | null,
}> {
    try {
        const response = await fetch(
            `${baseURL()}/auth/generate-token`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    "google-access-token": googleAccessToken
                }),
            }).then(res => res.text());

        return {
            success: true,
            message: "Token generated successfully",
            token: response,
        };
    } catch (error: any) {
        return {
            success: false,
            message: error,
            token: null,
        };
    }
}
