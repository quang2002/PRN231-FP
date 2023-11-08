import { createNewGroup, deleteGroup as deleteGroups, getAllGroups } from "@/api/fap-api/group";
import RouteGuard from "@/components/RouteGuard";
import { DataGrid } from "@mui/x-data-grid";
import { useEffect, useState } from "react";
import { Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, Button, useDisclosure, RadioGroup, Radio, Input, Checkbox, Select, SelectItem, Spinner, Spacer } from "@nextui-org/react";
import { getAllSubjects} from "@/api/fap-api/subject";
import { getAllUsers } from "@/api/fap-api/user";
import Swal from "sweetalert2";
import { Box } from "@mui/material";
import Link from "next/link";


function AuthorizedComponent({ session }) {
    const [refresh, setRefresh] = useState();

    const [groups, setGroups] = useState([]);
    const [subjects, setSubjects] = useState([]);
    const [teachers, setTeachers] = useState([]);

    const [selectedGroup, setSelectedGroup] = useState([]);
    const [groupName, setGroupName] = useState();
    const [groupTeacher, setGroupTeacher] = useState();
    const [groupSubject, setGroupSubject] = useState();
    const [groupSemester, setGroupSemester] = useState('FA23');

    const { isOpen, onOpen, onOpenChange } = useDisclosure();

    useEffect(() => {
        getAllGroups(session.accessToken).then(setGroups);

        getAllSubjects(session.accessToken).then(setSubjects);

        getAllUsers(session.accessToken).then(users => users.filter(user => user.role === "teacher")).then(setTeachers);
    }, [session, refresh]);

    if (!groups || !subjects || !teachers) {
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

    const onClickSubmitCreateGroup = async () => {
        // create
        console.log(groupName, groupTeacher, groupSubject, groupSemester);

        if (groupName && groupTeacher && groupSubject && groupSemester) {
            const result = await createNewGroup(session.accessToken, {
                name: groupName,
                teacherId: groupTeacher,
                subjectId: groupSubject,
                semester: groupSemester,
            });

            Swal.fire({
                icon: result ? 'success' : 'error',
                title: result ? 'Success' : 'Oops...',
                text: result ? 'Create group successfully!' : 'Something went wrong!',
                showConfirmButton: false,
                timer: 2000,
            });
        }

        // close
        onOpenChange();
        setRefresh(!refresh);
    };

    const onClickSubmitDeleteGroup = async () => {
        if (selectedGroup.length === 0) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please select group to delete!',
                showConfirmButton: false,
                timer: 2000,
            });

            return;
        }

        const result = await deleteGroups(session.accessToken, selectedGroup);

        Swal.fire({
            icon: result ? 'success' : 'error',
            title: result ? 'Success' : 'Oops...',
            text: result ? 'Create group successfully!' : 'Something went wrong!',
            showConfirmButton: false,
            timer: 2000,
        });

        setRefresh(!refresh);
    };

    return (
        <>
            <div className="w-full flex items-center flex-col mt-3">
                <div className="container p-3">
                    <h1 className="text-3xl font-bold">Groups</h1>

                    <div className="flex justify-end">
                        <Button
                            color="primary"
                            onClick={onOpen}
                            className="ml-2"
                        >Add</Button>

                        <Button
                            color="danger"
                            onClick={onClickSubmitDeleteGroup}
                            className="ml-2"
                        >Delete</Button>
                    </div>
                </div>

                <div className="container p-3">
                    <Box sx={{ height: 600, width: '100%' }}>
                        <DataGrid
                            rows={
                                groups.map((group) => ({
                                    id: group.id,
                                    name: group.name,
                                    subject: group.subject.code,
                                    teacher: group.teacher.email,
                                    semester: group.semester
                                }))
                            }
                            columns={[
                                { field: 'id', headerName: 'ID', width: 100 },
                                { field: 'name', headerName: 'Group name', width: 200 },
                                { field: 'subject', headerName: 'Subject', width: 200 },
                                { field: 'teacher', headerName: 'Teacher', width: 300 },
                                { field: 'semester', headerName: 'Semester', width: 200 },
                                {
                                    headerName: 'Actions', type: 'actions', width: 200,
                                    renderCell: ({ row }) => (
                                        <>
                                            <Link href={`/admin/group/${row.id}`}>
                                                <Button
                                                    color="primary"
                                                    variant="ghost"
                                                >View</Button>
                                            </Link>

                                            <Spacer x={1} />

                                            <Link href={`/admin/feedback/${row.id}`}>
                                                <Button
                                                    color="secondary"
                                                    variant="ghost"
                                                >Feedbacks</Button>
                                            </Link>
                                        </>
                                    ),
                                }
                            ]}
                            initialState={{
                                pagination: {
                                    paginationModel: { page: 0, pageSize: 10 },
                                },
                            }}
                            pageSizeOptions={[10, 20]}
                            checkboxSelection
                            disableRowSelectionOnClick
                            onRowSelectionModelChange={(e) => {
                                setSelectedGroup(e);
                            }}
                        />
                    </Box>
                </div>
            </div>

            <Modal
                isOpen={isOpen}
                onOpenChange={onOpenChange}
                placement="top-center"
            >
                <ModalContent>
                    {(onClose) => (
                        <>
                            <ModalHeader className="flex flex-col gap-1">Add new group</ModalHeader>
                            <ModalBody>
                                <Input
                                    autoFocus
                                    label="Name"
                                    placeholder="Enter group name"
                                    variant="bordered"
                                    onChange={(e) => setGroupName(e.target.value)}
                                    value={groupName}
                                />

                                <Select
                                    label="Subject"
                                    placeholder="Select subject"
                                    variant="bordered"
                                    items={subjects.map((subject) => ({
                                        value: subject.id,
                                        label: subject.code,
                                    }))}
                                    onChange={(e) => setGroupSubject(e.target.value)}
                                    selectedKeys={groupSubject}
                                >
                                    {(item) => (
                                        <SelectItem key={item.value} value={item.value}>
                                            {item.label}
                                        </SelectItem>
                                    )}
                                </Select>

                                <Select
                                    label="Teacher"
                                    placeholder="Select teacher"
                                    variant="bordered"
                                    items={teachers.map((teacher) => ({
                                        value: teacher.id,
                                        label: teacher.email,
                                    }))}
                                    onChange={(e) => setGroupTeacher(e.target.value)}
                                    selectedKeys={groupTeacher}
                                >
                                    {(item) => (
                                        <SelectItem key={item.value} value={item.value}>
                                            {item.label}
                                        </SelectItem>
                                    )}
                                </Select>
                            </ModalBody>
                            <ModalFooter>
                                <Button color="danger" variant="flat" onPress={onClose}>
                                    Close
                                </Button>
                                <Button color="primary" onPress={onClickSubmitCreateGroup}>
                                    Submit
                                </Button>
                            </ModalFooter>
                        </>
                    )}
                </ModalContent>
            </Modal>
        </>
    );
}

export default function AdminGroup() {
    return (
        <RouteGuard role={"admin"}>
            <AuthorizedComponent />
        </RouteGuard>
    );
}