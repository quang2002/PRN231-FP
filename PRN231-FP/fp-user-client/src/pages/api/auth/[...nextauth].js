import NextAuth from 'next-auth';
import GoogleProvider from 'next-auth/providers/google';
import { generateToken } from "../../../api/fap-api/generate-token";
import { getUserInfo } from '@/api/fap-api/user';

export const authOptions = {
    providers: [
        GoogleProvider({
            clientId: process.env.GOOGLE_OAUTH_CLIENT_ID,
            clientSecret: process.env.GOOGLE_OAUTH_CLIENT_SECRET,
        }),
    ],
    session: {
        strategy: 'jwt',
    },
};

export default NextAuth({
    ...authOptions,
    callbacks: {
        jwt: async ({ token, account }) => {
            if (account) {
                token.response = await generateToken(account["access_token"]);

                if (token.response.success) {
                    token.userInfo = await getUserInfo(token.response.token);
                }
            }
            return token;
        },
        session: async ({ session, token }) => {
            session.userInfo = token?.userInfo;
            session.accessToken = token?.response?.token;
            session.errorToken = token?.response?.message;
            return session;
        },
    },
    secret: process.env.NEXT_PUBLIC_SECRET,
});
