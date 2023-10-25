import NextAuth from 'next-auth';
import GoogleProvider from 'next-auth/providers/google';

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
        jwt: ({ token, account }) => {
            if (account) {
                token.accessToken = account["access_token"];
            }
            return token;
        },
        session: ({ session, token }) => {
            session.token = token;
            console.log(session);
            return session;
        },
    }
});