import { useRouter } from "next/router";
import RouteGuard from "../../components/RouteGuard";
import Rating from 'react-star-rating-component';
import { useState } from "react";
import { useQuill } from "react-quilljs";
import 'quill/dist/quill.snow.css';

function AuthenticatedComponent({ data, id }) {
    const feedback = {
        '_id': id,
        'group_name': 'GD1608-AD',
        'open_date': '01/10/2023',
        'lecture': 'Anhpn',
        'subjects': 'MLN111(Philosophy of Marxism – Leninism)',
        'close_date': '',
        'is_feedback': true,
    };

    const renderStarIcon = () => <span className="px-1">★</span>;

    const [ratingPunctuality, setRatingPunctuality] = useState(0);
    const [ratingSkill, setRatingSkill] = useState(0);
    const [ratingAdequately, setRatingAdequately] = useState(0);
    const [ratingSupport, setRatingSupport] = useState(0);
    const [ratingResponse, setRatingResponse] = useState(0);

    const { quill, quillRef } = useQuill();

    const submitFeedback = () => {
        console.log(quill.getText());
        console.log(ratingPunctuality);
        console.log(ratingSkill);
        console.log(ratingAdequately);
        console.log(ratingSupport);
        console.log(ratingResponse);
    };

    return (
        <div className='h-full flex justify-center py-4'>
            <div>
                <h1 className='text-3xl font-thin py-2'>Do Feedback</h1>

                <hr className='my-4' />

                <table className="table-auto w-full">
                    <tbody>
                        <tr>
                            <td className='border p-2'><b>Group name:</b> {feedback.group_name}</td>
                            <td className='border p-2'><b>Teacher:</b> {feedback.lecture}</td>
                        </tr>

                        <tr>
                            <td className="border p-2" colSpan={2}>
                                <b>Subject:</b> {feedback.subjects}
                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td></td>
                        </tr>

                        {
                            [
                                {
                                    'title': "Regarding the teacher's punctuality",
                                    'name': 'punctuality',
                                    'value': ratingPunctuality,
                                    'setter': setRatingPunctuality,
                                },
                                {
                                    'title': "Teaching skills of teacher",
                                    'name': 'skill',
                                    'value': ratingSkill,
                                    'setter': setRatingSkill,
                                },
                                {
                                    'title': "The teacher adequately covers the topics required by the syllabus",
                                    'name': 'adequately',
                                    'value': ratingAdequately,
                                    'setter': setRatingAdequately,
                                },
                                {
                                    'title': "Support from the teacher - guidance for practical exercises, answering questions out side of class",
                                    'name': 'support',
                                    'value': ratingSupport,
                                    'setter': setRatingSupport,
                                },
                                {
                                    'title': "Teacher's response to student's questions in class",
                                    'name': 'response',
                                    'value': ratingResponse,
                                    'setter': setRatingResponse,
                                },
                            ].map(item => (
                                <tr key={item.name}>
                                    <td className="border p-2 font-semibold">{item.title}</td>

                                    <td className="border p-2">
                                        <div className="flex items-center justify-center">
                                            <Rating
                                                name={item.name}
                                                value={item.value}
                                                onStarClick={(value) => item.setter(value)}
                                                starCount={4}
                                                renderStarIcon={renderStarIcon}
                                            />
                                        </div>
                                    </td>
                                </tr>
                            ))
                        }
                    </tbody>
                </table>


                <div className="mb-10">
                    <h2 className="my-2 font-semibold">Your comment:</h2>

                    <div style={{ height: 250 }} className="w-full">
                        <div ref={quillRef} />
                    </div>
                </div>

                <hr className='my-4' />

                <button
                    onClick={submitFeedback}
                    className="bg-blue-500 hover:bg-blue-700 text-white rounded py-2 px-4"
                    type="button">
                    Submit
                </button>
            </div>
        </div>
    );
}

export default function FeedbacksPage() {
    const { id } = useRouter().query;

    return (
        <RouteGuard>
            <AuthenticatedComponent id={id} />
        </RouteGuard>
    );
}
