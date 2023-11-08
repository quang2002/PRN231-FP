import { baseURL } from ".";

export async function getAllSubjects(accessToken: string): Promise<Array<{
    id: number,
    code: string,
    name: string,
}>> {
    try {
        const response = await fetch(
            `${baseURL()}/subject`,
            {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
            }).then((res) => res.json());
        return response;
    } catch (error: any) {
        return [];
    }
}
