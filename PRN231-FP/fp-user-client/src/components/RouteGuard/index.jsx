import { useSession } from "next-auth/react";
import { useRouter } from "next/router";
import React from "react";

export default function RouteGuard({ children }) {
    const { data, status } = useSession();
    const router = useRouter();

    if (status === 'loading') {
        return <></>;
    }

    if (status === 'unauthenticated' || !data?.accessToken) {
        router.replace('/');
        return <></>;
    }

    children = React.cloneElement(children, { data });

    return children;
}