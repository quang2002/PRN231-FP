import Image from 'next/image';
import styles from './style.module.css';
import { signIn, signOut, useSession } from "next-auth/react";
import Link from 'next/link';
import { useEffect } from 'react';
import Swal from 'sweetalert2';

export default function HeaderComponent({ title, }) {
    const { data, status } = useSession();

    useEffect(() => {
        if (status != 'loading' && !data?.accessToken) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: data?.errorToken || 'You are not logged in!',
                showConfirmButton: false,
                timer: 2000,
            });
        }
    });

    return (
        <header className={styles["fp-header"]}>
            <div className='flex'>
                <h1 className='text-2xl font-normal text-white'>
                    <Link href={'/'}>{title}</Link>
                </h1>
            </div>

            {
                status === 'authenticated' ?
                    <div className='flex items-center'>
                        <h1 className='font-normal text-white px-3'>{data.user.name}</h1>

                        <Image
                            width={48}
                            height={48}
                            src={data.user.image}
                            alt={data.user.name}
                            className='rounded-full border'
                        />

                        <h1 onClick={() => signOut()} className='font-normal text-white px-3 hover:text-red-500 select-none'>Sign out</h1>
                    </div> :
                    <div className='flex items-center'>
                        <h1 onClick={() => signIn('google')} className='font-normal text-white px-3 hover:font-normal hover:text-blue-500 select-none'>Sign in</h1>
                    </div>
            }
        </header>
    );
}
