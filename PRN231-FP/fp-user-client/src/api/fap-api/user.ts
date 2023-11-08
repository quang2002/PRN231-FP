import { baseURL } from ".";

export async function getUserInfo(accessToken: string): Promise<{
    id: number,
    email: string,
    role: "admin" | "teacher" | "student",
} | string> {
    try {
        const response = await fetch(
            `${baseURL()}/user/me`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${accessToken}`,
            },
        }).then((res) => res.json());

        return response;
    } catch (error: any) {
        return error;
    }
}

export async function getUserFeedbacks(accessToken: string): Promise<Array<{
    id: number,
    studentId: number,
    groupId: number,
    isDone: boolean,
    punctuality: number,
    skills: number,
    adequately: number,
    support: number,
    response: number,
    comment: string,
    group: Array<{
        id: number,
        name: string,
        subjectId: number,
        teacherId: number,
        semester: string,
        subject: {
            id: number,
            name: string,
            code: string,
        },
        teacher: {
            id: number,
            email: string,
            role: "admin" | "teacher" | "student",
        },
    }>
}>> {
    try {
        const response = await fetch(
            `${baseURL()}/user/feedbacks`, {
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

export async function getAllUsers(accessToken: string): Promise<Array<{
    id: number,
    email: string,
    role: "admin" | "teacher" | "student",
}>> {
    try {
        const response = await fetch(
            `${baseURL()}/user/all`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${accessToken}`,
            },
        }).then((res) => res.json());

        return response;
    } catch (error: any) {
        return [];
    }
}