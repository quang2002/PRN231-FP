/** @type {import('next').NextConfig} */
const nextConfig = {
    images: {
        domains: ['lh3.googleusercontent.com'],
    },
    env: {
        NEXT_PUBLIC_FP_FAP_API_BASE_URL: process.env.FP_FAP_API_BASE_URL,
    },
}

module.exports = nextConfig
