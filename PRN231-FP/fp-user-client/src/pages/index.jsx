import RouteGuard from '@/components/RouteGuard';
import { useSession } from 'next-auth/react';
import Link from 'next/link';

function LoadingComponent() {
    return (
        <div className='h-full flex justify-center items-center'>
            <div>
                <h1 className='text-5xl font-thin py-2'>Welcome to</h1>

                <h1 className='text-5xl font-thin py-2'>
                    <b>FPT University Academic Portal</b>
                </h1>

                <hr className='my-4' />

                <div className='font-mono text-center'>
                    Loading... Please wait
                </div>
            </div>
        </div>
    );
}

function AuthenticatedComponent({ session }) {
    return (
        <div className='h-full flex justify-center items-center'>
            <div>
                <h1 className='text-5xl font-thin py-2'>Welcome to</h1>

                <h1 className='text-5xl font-thin py-2'>
                    <b>FPT University Academic Portal</b>
                </h1>

                <hr className='my-4' />

                <div className='font-mono text-center'>
                    You are signed in as <b>{session.user.email}</b>

                    <div className='my-4'>
                        {
                            (session.userInfo.role !== 'admin')
                                ? < Link href='/feedbacks' className='text-blue-500 hover:underline'>View your feedbacks</Link>
                                : < Link href='/admin' className='text-blue-500 hover:underline'>Go to admin page</Link>

                        }
                    </div>
                </div>
            </div>
        </div >
    );
}

function UnauthenticatedComponent() {
    return (
        <div className='h-full flex justify-center items-center'>
            <div>
                <h1 className='text-5xl font-thin py-2'>Welcome to</h1>

                <h1 className='text-5xl font-thin py-2'>
                    <b>FPT University Academic Portal</b>
                </h1>

                <hr className='my-4' />

                <div className='font-mono text-center'>
                    Sign-in first to see your profile information
                </div>
            </div>
        </div>
    );
}


export default function IndexPage() {
    const { data, status } = useSession();

    if (status === 'loading') {
        return <LoadingComponent />;
    }

    if (status === 'authenticated') {
        return (
            <RouteGuard>
                <AuthenticatedComponent />
            </RouteGuard>
        );
    }

    return <UnauthenticatedComponent />;
}
