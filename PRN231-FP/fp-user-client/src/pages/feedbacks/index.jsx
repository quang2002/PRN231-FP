import Link from 'next/link';
import RouteGuard from '@/components/RouteGuard';
import { useEffect, useState } from 'react';
import { getUserFeedbacks } from '@/api/fap-api/user';
import { Spinner } from '@nextui-org/react';

function AuthenticatedComponent({ session }) {
    const [feedbacks, setFeedbacks] = useState(null);

    useEffect(() => {
        getUserFeedbacks(session.accessToken).then(setFeedbacks);
    }, [session.accessToken]);

    if (!feedbacks) {
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

    if (session.userInfo.role === 'teacher') {
        return (
            <div className='h-full flex justify-center py-4'>
                <div>
                    <h1 className='text-3xl font-thin py-2'>List feedbacks to <b>{session.user.name}</b></h1>

                    <hr className='my-4' />

                    <table className='table-auto w-full'>
                        <thead>
                            <tr>
                                <th className='border px-4 py-2'>Group Name</th>
                                <th className='border px-4 py-2'>Student</th>
                                <th className='border px-4 py-2'>Subject</th>
                                <th className='border px-4 py-2'>Actions</th>
                            </tr>
                        </thead>

                        <tbody>
                            {
                                feedbacks.map(item => (
                                    <tr key={item.id}>
                                        <td className='border px-4 py-2'>{item.group.name}</td>
                                        <td className='border px-4 py-2'>{item.student.email}</td>
                                        <td className='border px-4 py-2'>{item.group.subject.code}</td>
                                        <td className='border px-2 py-2'>
                                            {
                                                <Link
                                                    href={`/feedbacks/${item.id}`}
                                                    className='bg-blue-500 hover:bg-blue-700 text-white font-bold p-2 block text-center rounded'>
                                                    View
                                                </Link>
                                            }
                                        </td>
                                    </tr>
                                ))
                            }
                        </tbody>
                    </table>
                </div>
            </div>
        );
    }

    return (
        <div className='h-full flex justify-center py-4'>
            <div>
                <h1 className='text-3xl font-thin py-2'>List feedbacks of <b>{session.user.name}</b></h1>

                <hr className='my-4' />

                <table className='table-auto w-full'>
                    <thead>
                        <tr>
                            <th className='border px-4 py-2'>Group Name</th>
                            <th className='border px-4 py-2'>Lecture</th>
                            <th className='border px-4 py-2'>Subject</th>
                            <th className='border px-4 py-2'>Actions</th>
                        </tr>
                    </thead>

                    <tbody>
                        {
                            feedbacks.map(item => (
                                <tr key={item.id}>
                                    <td className='border px-4 py-2'>{item.group.name}</td>
                                    <td className='border px-4 py-2'>{item.group.teacher.email}</td>
                                    <td className='border px-4 py-2'>{item.group.subject.code}</td>
                                    <td className='border px-2 py-2'>
                                        {
                                            item.isDone ? (
                                                <Link
                                                    href={`/feedbacks/${item.id}`}
                                                    className='bg-green-500 hover:bg-green-700 text-white font-bold p-2 block text-center rounded'>
                                                    Retake
                                                </Link>
                                            ) : (
                                                <Link
                                                    href={`/feedbacks/${item.id}`}
                                                    className='bg-blue-500 hover:bg-blue-700 text-white font-bold p-2 block text-center rounded'>
                                                    Feedback
                                                </Link>
                                            )
                                        }
                                    </td>
                                </tr>
                            ))
                        }
                    </tbody>
                </table>
            </div>
        </div>
    );
}


export default function FeedbacksPage() {
    return (
        <RouteGuard role={"student,teacher"}>
            <AuthenticatedComponent />
        </RouteGuard>
    );
}
