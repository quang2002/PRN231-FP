import './global.css';
import { SessionProvider, useSession } from 'next-auth/react';
import HeaderComponent from '../components/Header'
import { Fragment } from 'react';


export default function App({ Component, pageProps }) {
    return (
        <SessionProvider session={pageProps.session}>
            <Fragment>
                <div className='h-screen flex flex-col pb-4'>
                    <HeaderComponent title={"FP - FAP"} />
                    <div className='h-full overflow-auto'>
                        <Component {...pageProps} />
                    </div>
                </div>
            </Fragment>
        </SessionProvider>
    );
}
