import './global.css';
import { SessionProvider, useSession } from 'next-auth/react';
import HeaderComponent from '../components/Header'
import { Fragment } from 'react';
import { NextUIProvider } from '@nextui-org/react';
import { ThemeProvider } from 'next-themes';


export default function App({ Component, pageProps }) {
    return (
        <NextUIProvider>
            <ThemeProvider attribute='class' defaultTheme='light'>
                <SessionProvider session={pageProps.session}>
                    <Fragment>
                        <div className='h-screen flex flex-col pb-0'>
                            <HeaderComponent title={"FP - FAP"} />
                            <div className='h-full overflow-auto fill-background'>
                                <Component {...pageProps} />
                            </div>
                        </div>
                    </Fragment>
                </SessionProvider>
            </ThemeProvider>
        </NextUIProvider>
    );
}
