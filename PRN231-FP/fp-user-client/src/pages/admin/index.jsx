import RouteGuard from "@/components/RouteGuard";
import { Button, Card, CardHeader, Image, Spacer } from "@nextui-org/react";
import Link from "next/link";


function AuthorizedComponent({ session }) {

    return (
        <div className="h-full flex justify-center p-6">
            <div className="bg-zinc-900 rounded-lg  h-full max-h-full overflow-y-auto overflow-scroll" style={{ width: '80%' }}>
                <div className="gap-2 grid grid-cols-12 grid-rows-2 p-8">
                       
                </div>
            </div>
        </div>
    );
}

export default function AdminPage() {
    return (
        <RouteGuard role={"admin"}>
            <AuthorizedComponent />
        </RouteGuard>
    );
}