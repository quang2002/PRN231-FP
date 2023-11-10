import { Spinner } from "@nextui-org/react";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import React, { useEffect } from "react";
import Swal from "sweetalert2";


export default function RouteGuard({ children, role }) {
    const { data, status } = useSession();
    const router = useRouter();

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
    }, [data, status]);

    if (status === 'loading') {
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

    if (status === 'unauthenticated' || !data?.accessToken) {
        router.replace('/');
        return <></>;
    }

    if (role && role.split(',').indexOf(data?.userInfo?.role) === -1) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Yout are not permitted to access this page!',
            showConfirmButton: false,
            timer: 2000,
        });

        router.replace('/');
        return <></>;
    }

    console.log(data)

    children = React.cloneElement(children, { session: data });

    return children;
}