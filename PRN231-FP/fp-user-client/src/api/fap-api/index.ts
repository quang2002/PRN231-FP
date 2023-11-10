export function baseURL() {
    return process.env.FP_FAP_API_BASE_URL || process.env.NEXT_PUBLIC_FP_FAP_API_BASE_URL;
}