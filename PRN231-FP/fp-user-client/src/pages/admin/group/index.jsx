import RouteGuard from "@/components/RouteGuard";

export default function AdminGroup() {
    return (
        <RouteGuard role={"admin"}>
            <div>
                
            </div>
        </RouteGuard>
    );
}