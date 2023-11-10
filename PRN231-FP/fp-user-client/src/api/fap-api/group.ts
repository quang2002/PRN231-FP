import { baseURL } from ".";

export async function getAllGroups(accessToken: string, semester?: string | null): Promise<Array<{
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
        role: string,
    }
}>> {
    try {
        const response = await fetch(
            `${baseURL()}/group?semester=${semester ?? ""}`,
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

export async function getGroupById(accessToken: string, id: number) {
    try {
        const response = await fetch(
            `${baseURL()}/group/${id}`,
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

export async function createNewGroup(accessToken: string, group: {
    name: string,
    teacherId: number,
    subjectId: number,
    semester: string
}): Promise<boolean> {
    try {
        const response = await fetch(
            `${baseURL()}/group`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
                body: JSON.stringify(group)
            }).then((res) => res.ok);
        return response;
    } catch (error: any) {
        return false;
    }
}

export async function deleteGroup(accessToken: string, groups: Array<number>): Promise<boolean> {
    try {
        const response = await fetch(
            `${baseURL()}/group`,
            {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
                body: JSON.stringify(groups)
            }).then((res) => res.ok);
        return response;
    } catch (error: any) {
        return false;
    }
}

export async function enrollStudent(accessToken: string, groupId: number, students: Array<number>): Promise<boolean> {
    try {
        const response = await fetch(
            `${baseURL()}/group/enroll`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
                body: JSON.stringify({
                    groupId,
                    students
                })
            }).then((res) => res.ok);
        return response;
    } catch (error: any) {
        return false;
    }
}

export async function unenrollStudent(accessToken: string, groupId: number, students: Array<number>): Promise<boolean> {
    try {
        const response = await fetch(
            `${baseURL()}/group/unenroll`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${accessToken}`,
                },
                body: JSON.stringify({
                    groupId,
                    students
                })
            }).then((res) => res.ok);
        return response;
    } catch (error: any) {
        return false;
    }
}