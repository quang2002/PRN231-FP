import './global.css';
import { SessionProvider } from 'next-auth/react';
import HeaderComponent from '../components/Header'
import { Fragment } from 'react';


function App({ Component, pageProps }) {
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

export default App;
