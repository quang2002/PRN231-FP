import { useRouter } from "next/router";
import RouteGuard from "@/components/RouteGuard";
import { useEffect, useState } from "react";
import { enrollStudent, getGroupById, unenrollStudent } from "@/api/fap-api/group";
import { Button, Spinner } from "@nextui-org/react";
import { Box } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import { getAllUsers } from "@/api/fap-api/user";

function AuthenticatedComponent({ session, id }) {
    const [group, setGroup] = useState(null);
    const [students, setStudents] = useState([]);

    useEffect(() => {
        getGroupById(session.accessToken, id).then(setGroup);
        getAllUsers(session.accessToken).then(users => users.filter(user => user.role === "student")).then(setStudents);
    }, [session.accessToken, id]);

    if (!group || !students) {
        return (
            <Spinner
                size="lg"
                color="primary"
                strokeWidth={4}
                style={{
                    position: "absolute",
                    top: "50%",
                    left: "50%",
                    transform: "translate(-50%, -50%)"
                }}
                label="Loading..."
            />
        );
    }

    const onClickEnroll = async (studentId) => {
        const result = await enrollStudent(session.accessToken, group.id, [studentId]);

        if (result) {
            setGroup({
                ...group,
                enrolls: [...group.enrolls, {
                    studentId,
                    groupId: group.id,
                }]
            });
        }
    }

    const onClickUnenroll = async (studentId) => {
        const result = await unenrollStudent(session.accessToken, group.id, [studentId]);

        if (result) {
            setGroup({
                ...group,
                enrolls: group.enrolls.filter(e => e.studentId !== studentId)
            });
        }
    }

    return (
        <>
            <div className="w-full flex items-center flex-col mt-3">
                <div className="container p-3">
                    <h1 className="text-3xl font-bold">Group {group.name} - {group.id}</h1>
                </div>

                <div className="container p-3">
                    <Box sx={{ height: 600, width: '100%' }}>
                        <DataGrid
                            columns={[
                                { field: 'id', headerName: 'ID', width: '100' },
                                { field: 'email', headerName: 'Email', width: '400' },
                                {
                                    field: 'enrolled', headerName: 'Status', width: '200',
                                    renderCell: ({ value, row }) => (
                                        value
                                            ? <Button color="success" variant="ghost" onClick={() => onClickUnenroll(row.id)}>Enrolled</Button>
                                            : <Button color="danger" variant="ghost" onClick={() => onClickEnroll(row.id)}>Not enrolled</Button>
                                    )
                                }
                            ]}

                            rows={students.map((student) => ({
                                id: student.id,
                                email: student.email,
                                enrolled: group.enrolls.map(e => e.studentId).includes(student.id)
                            }))}
                        />
                    </Box>
                </div>
            </div>
        </>
    );
}

export default function AdminGroupPage() {
    const { id } = useRouter().query;

    return (
        <RouteGuard>
            <AuthenticatedComponent id={id} />
        </RouteGuard>
    );
}
