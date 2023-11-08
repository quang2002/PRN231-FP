import { baseURL } from ".";

export async function assignFeedback(accessToken: string, groupId: number, students: Array<number>): Promise<boolean> {
    try {
        const response = await fetch(
            `${baseURL()}/feedback/assign`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
                body: JSON.stringify({
                    groupId,
                    students,
                }),
            }).then((res) => res.ok);
        return response;
    } catch (error: any) {
        return false;
    }
}

export async function doFeedback(accessToken: string, feedback: {
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
}): Promise<boolean> {
    try {
        const response = await fetch(
            `${baseURL()}/feedback`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
                body: JSON.stringify(feedback),
            }).then((res) => res.ok);
        return response;
    } catch (error: any) {
        return false;
    }
}

export async function getFeedback(accessToken: string, id: number): Promise<{
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
} | null> {
    try {
        const response = await fetch(
            `${baseURL()}/feedback/${id}`,
            {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
            }).then((res) => res.json());
        return response;
    } catch (error: any) {
        return null;
    }
}