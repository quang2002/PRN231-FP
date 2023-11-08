import { useRouter } from "next/router";
import RouteGuard from "@/components/RouteGuard";
import { useEffect, useState } from "react";
import { enrollStudent, getGroupById, unenrollStudent } from "@/api/fap-api/group";
import { Button, Chip, Spinner } from "@nextui-org/react";
import { Box } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import { getAllUsers } from "@/api/fap-api/user";
import { CheckIcon } from "@/components/Icons/CheckIcon";
import { TimeIcon } from "@/components/Icons/TimeIcon";
import { assignFeedback } from "@/api/fap-api/feedback";
import Swal from "sweetalert2";

function AuthenticatedComponent({ session, id }) {
    const [refresh, setRefresh] = useState(false);
    const [group, setGroup] = useState(null);
    const [selectedStudents, setSelectedStudents] = useState([]);

    useEffect(() => {
        getGroupById(session.accessToken, id).then(setGroup);
    }, [session.accessToken, id, refresh]);

    if (!group) {
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

    const feedbacks = group.feedbacks.reduce((acc, feedback) => {
        acc[feedback.studentId] = feedback;
        return acc;
    }, {});

    const onClickAssignSelected = async () => {
        const result = await assignFeedback(session.accessToken, group.id, selectedStudents);

        if (result) {
            Swal.fire({
                title: 'Success',
                text: 'Assign feedback success',
                icon: 'success',
                confirmButtonText: 'OK'
            });
        } else {
            Swal.fire({
                title: 'Error',
                text: 'Assign feedback failed',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }

        setRefresh(!refresh);
    };

    return (
        <>
            <div className="w-full flex items-center flex-col mt-3">
                <div className="container p-3">
                    <h1 className="text-3xl font-bold">Feedbacks of group {group.name} - {group.id}</h1>

                    <div className="flex flex-row justify-end mt-3">
                        <Button color="success" variant="bordered" onClick={onClickAssignSelected}>Assign selected</Button>
                    </div>
                </div>

                <div className="container p-3">
                    <Box sx={{ height: 600, width: '100%' }}>
                        <DataGrid
                            columns={[
                                { field: 'id', headerName: 'ID', width: '100' },
                                { field: 'email', headerName: 'Email', width: '400' },
                                {
                                    field: 'enrolled', headerName: 'Status', width: '200',
                                    renderCell: ({ value }) => {
                                        if (value === 'done') return <Chip color="success" variant="bordered" startContent={<CheckIcon size={18} />}>Done</Chip>;
                                        if (value === 'assigned') return <Chip color="warning" variant="bordered" startContent={<TimeIcon size={18} />}>Waiting</Chip>
                                        if (value === 'unassign') return <Chip color="danger" variant="bordered" startContent={<TimeIcon size={18} />}>Unassigned</Chip>
                                    }
                                }
                            ]}

                            rows={group.enrolls.map(({ student }) => ({
                                id: student.id,
                                email: student.email,
                                enrolled: feedbacks[student.id] ? feedbacks[student.id].isDone ? "done" : "assigned" : "unassign"
                            }))}

                            onRowSelectionModelChange={setSelectedStudents}

                            checkboxSelection
                        />
                    </Box>
                </div>
            </div>
        </>
    );
}

export default function AdminFeedbackPage() {
    const { id } = useRouter().query;

    return (
        <RouteGuard>
            <AuthenticatedComponent id={id} />
        </RouteGuard>
    );
}
