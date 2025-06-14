import { useSelector, useDispatch } from 'react-redux';
import './chat.css'
import { Link, useNavigate } from 'react-router-dom';
import defaultImage from '../../assets/Images/defaultuserimage.png';
import { logout as LogoutSlice } from '../../Features/AuthSlice';


function Navbar() {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const fileBaseURL = String(import.meta.env.VITE_API_FILE_PATH_URL);
    const authStatus = useSelector((state) => state.auth.status);
    const userdata = useSelector((state) => state.auth.userData);

    const Logout = () => {
        localStorage.setItem("AuthenticationToken", "");
        dispatch(LogoutSlice());
    }

    return (
        <div className="app-navbar" style={{ background: "linear-gradient(to right, #2ba7ff, #41bdff)", color: "white" }}>
            <div className="navbar-container">
                <div className="navbar-header-section">
                    <h1 className="navbar-brand-text" style={{ color: "white", fontSize: "23px" }}>CHITCHAT</h1>
                </div>
                <div className="navbar-collapse-section">
                    <ul className="navbar-nav-list" style={{ color: "white" }}>
                        {authStatus ?
                            <>
                                <li><img src={userdata.image ? fileBaseURL + userdata.image : defaultImage} className="profile-img" style={{ width: "40px", height: "40px", borderRadius: "50%", marginTop: "6px" }} /></li>
                                <li><h4 style={{ color: "white", fontSize: "18px" }}>{userdata.userName}</h4></li>
                                <li><button onClick={() => Logout()} className="logoff-button">Log off</button></li>
                            </>
                            :
                            <>
                                <li><Link to="/signup" className="nav-link-register">Register</Link></li>
                                <li><Link to="/login" className="nav-link-login">Login</Link></li>
                            </>
                        }
                    </ul>
                </div>
            </div>
        </div>
    );
}

export default Navbar;