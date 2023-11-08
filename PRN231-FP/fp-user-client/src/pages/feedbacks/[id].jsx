import { useRouter } from "next/router";
import RouteGuard from "../../components/RouteGuard";
import Rating from 'react-star-rating-component';
import { useEffect, useState } from "react";
import { useQuill } from "react-quilljs";
import 'quill/dist/quill.snow.css';
import { doFeedback, getFeedback } from "@/api/fap-api/feedback";
import { Spinner } from "@nextui-org/react";
import Swal from "sweetalert2";

function AuthenticatedComponent({ session, id }) {
    const router = useRouter();
    const [feedback, setFeedback] = useState();

    const [ratingPunctuality, setRatingPunctuality] = useState(0);
    const [ratingSkill, setRatingSkill] = useState(0);
    const [ratingAdequately, setRatingAdequately] = useState(0);
    const [ratingSupport, setRatingSupport] = useState(0);
    const [ratingResponse, setRatingResponse] = useState(0);

    const { quill, quillRef } = useQuill();

    useEffect(() => {
        getFeedback(session.accessToken, id).then(setFeedback);
    }, [session.accessToken, id]);

    useEffect(() => {
        if (!feedback) return;

        setRatingPunctuality(feedback.punctuality);
        setRatingSkill(feedback.skill);
        setRatingAdequately(feedback.adequately);
        setRatingSupport(feedback.support);
        setRatingResponse(feedback.response);

        quill?.setText(feedback.comment || "");
        quill?.enable(false);
    }, [feedback, quill]);

    if (!feedback) {
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


    const renderStarIcon = () => <span className="px-1">★</span>;

    const submitFeedback = async () => {
        const result = await doFeedback(session.accessToken, {
            id: feedback.id,
            punctuality: ratingPunctuality,
            skill: ratingSkill,
            adequately: ratingAdequately,
            support: ratingSupport,
            response: ratingResponse,
            comment: quill?.getText() || "",
        });

        if (result) {
            Swal.fire({
                icon: 'success',
                title: 'Feedback submitted',
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                router.replace('/feedbacks');
            });
        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
                showConfirmButton: false,
                timer: 1500
            });
        }
    };

    if (session.userInfo.role === 'teacher') {
        return (
            <div className='h-full flex justify-center py-4'>
                <div>
                    <h1 className='text-3xl font-thin py-2'>Feedback of {feedback.student.email}</h1>

                    <hr className='my-4' />

                    <table className="table-auto w-full">
                        <tbody>
                            <tr>
                                <td className='border p-2'><b>Group name:</b> {feedback.group.name}</td>
                                <td className='border p-2'><b>Student:</b> {feedback.student.email}</td>
                            </tr>

                            <tr>
                                <td className="border p-2" colSpan={2}>
                                    <b>Subject:</b> {feedback.group.subject.code}
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
                        <h2 className="my-2 font-semibold">Comment:</h2>

                        <div style={{ height: 250 }} className="w-full">
                            <div ref={quillRef} />
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className='h-full flex justify-center py-4'>
            <div>
                <h1 className='text-3xl font-thin py-2'>Do Feedback</h1>

                <hr className='my-4' />

                <table className="table-auto w-full">
                    <tbody>
                        <tr>
                            <td className='border p-2'><b>Group name:</b> {feedback.group.name}</td>
                            <td className='border p-2'><b>Teacher:</b> {feedback.group.teacher.email}</td>
                        </tr>

                        <tr>
                            <td className="border p-2" colSpan={2}>
                                <b>Subject:</b> {feedback.group.subject.code}
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
