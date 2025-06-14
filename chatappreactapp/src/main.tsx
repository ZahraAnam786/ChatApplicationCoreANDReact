import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import Login from './Components/Account/Login.tsx'
import App from './App.tsx'
import Signup from './Components/Account/Signup.tsx'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import store from './Store/Store.tsx'
import { Provider } from 'react-redux'
import AuthLayout from './Components/Middleware/AuthLayout.tsx'

const router = createBrowserRouter([
    {
        path: "/",
        element: (
            <AuthLayout authentication>
                <App />
            </AuthLayout>
        ) 
    },
    {
        path: "/login",
        element: (
            <AuthLayout authentication={false}>
                <Login />
            </AuthLayout>
        ) 
    },
    {
        path: "/signup",
        element: (
            <AuthLayout authentication={false}>
                <Signup />
            </AuthLayout>
        ) 
    },
])
createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ToastContainer />
        <Provider store={store}>
            <RouterProvider router={router} />
        </Provider>
    </StrictMode>,
)
