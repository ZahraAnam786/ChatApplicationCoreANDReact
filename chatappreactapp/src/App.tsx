import { useState, useEffect } from 'react'
//import Login from './Components/Account/Login'
import { login, logout } from './Features/AuthSlice'
import { authService } from './Services/authService'
import { useDispatch } from 'react-redux';
import Navbar from './Components/Chat/Navbar';
import { useNavigate } from 'react-router-dom';
import ChatView from './Components/Chat/ChatView';
import './Components/Chat/chat.css';
import 'material-design-iconic-font/dist/css/material-design-iconic-font.min.css';


function App() {

    const [loading, setLoading] = useState(true);
    const dispatch = useDispatch();  // for state change use
    const navigate = useNavigate();

    //For check user exist
    useEffect(() => {
        //const authtoken = localStorage.getItem("AuthenticationToken");
        //if (authtoken == null || authtoken == "")
        //    dispatch(logout());
        authService.getCurrentUser()
            .then((userData) => {
                if (userData) {
                    dispatch(login({ userData })) // state change
                } else {
                    //dispatch(logout())
                    navigate('/')
                }
            })
            .finally(() => {
                setLoading(false);
            })
    }, []);

    return (
        <>
            <Navbar />
            <ChatView/>
        </>
    )
}

export default App
