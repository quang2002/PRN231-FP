import Link from 'next/link';
import RouteGuard from '../../components/RouteGuard';

function AuthenticatedComponent({ data }) {
    const feedbacks = [
        {
            '_id': '1',
            'group_name': 'GD1608-AD',
            'open_date': '01/10/2023',
            'lecture': 'Anhpn',
            'subjects': 'MLN111(Philosophy of Marxism – Leninism)',
            'close_date': '',
            'is_feedback': true,
        },
        {
            '_id': '2',
            'group_name': 'SE1622-NET',
            'open_date': '01/10/2023',
            'lecture': 'DuongTB',
            'subjects': 'PMG202c(Project management)',
            'close_date': '',
            'is_feedback': true,
        },
        {
            '_id': '3',
            'group_name': 'SE1622-NET',
            'open_date': '01/10/2023',
            'lecture': 'SangNV',
            'subjects': 'WDU203c(UI/UX Design)',
            'close_date': '',
            'is_feedback': false,
        },
        {
            '_id': '4',
            'group_name': 'SE1618-NET',
            'open_date': '29/09/2023',
            'lecture': 'HongND5',
            'subjects': 'EXE201(Experiential Entrepreneurship 2)',
            'close_date': '',
            'is_feedback': false,
        },
        {
            '_id': '5',
            'group_name': 'SE1618-NET',
            'open_date': '29/09/2023',
            'lecture': 'TienTD17',
            'subjects': 'PRN231(Building Cross-Platform Back-End Application With .NET)',
            'close_date': '',
            'is_feedback': false,
        },
        {
            '_id': '6',
            'group_name': 'TRS601.4.P2',
            'open_date': '13/04/2021',
            'lecture': 'Kmulligan',
            'subjects': 'TRS601(Transition)',
            'close_date': '',
            'is_feedback': true,
        },
    ];

    return (
        <div className='h-full flex justify-center py-4'>
            <div>
                <h1 className='text-3xl font-thin py-2'>List feedbacks of <b>{data.user.name}</b></h1>

                <hr className='my-4' />

                <table className='table-auto w-full'>
                    <thead>
                        <tr>
                            <th className='border px-4 py-2'>Group Name</th>
                            <th className='border px-4 py-2'>Open Date</th>
                            <th className='border px-4 py-2'>Lecture</th>
                            <th className='border px-4 py-2'>Subjects</th>
                            <th className='border px-4 py-2'>Close Date</th>
                            <th className='border px-4 py-2'>Actions</th>
                        </tr>
                    </thead>

                    <tbody>
                        {
                            feedbacks.map(item => (
                                <tr key={item._id}>
                                    <td className='border px-4 py-2'>{item.group_name}</td>
                                    <td className='border px-4 py-2'>{item.open_date}</td>
                                    <td className='border px-4 py-2'>{item.lecture}</td>
                                    <td className='border px-4 py-2'>{item.subjects}</td>
                                    <td className='border px-4 py-2'>{item.close_date}</td>
                                    <td className='border px-2 py-2'>
                                        {
                                            item.is_feedback ? (
                                                <Link
                                                    href={`/feedbacks/${item._id}`}
                                                    className='bg-green-500 hover:bg-green-700 text-white font-bold p-2 block text-center rounded'>
                                                    Retake
                                                </Link>
                                            ) : (
                                                <Link
                                                    href={`/feedbacks/${item._id}`}
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
        <RouteGuard>
            <AuthenticatedComponent />
        </RouteGuard>
    );
}
