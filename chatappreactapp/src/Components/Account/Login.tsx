
import { useState } from 'react';
import './Account.css'
import { Link, useNavigate } from "react-router-dom";
import { useDispatch } from 'react-redux';
import { authService } from '../../Services/authService'
import { login as LoginSlice} from '../../Features/AuthSlice'

function Login() {

    const dispatch = useDispatch();
    const navigate = useNavigate();
    const [lUser, setlUser] = useState({
        Email : "", Password : ""
    });

    const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setlUser({...lUser, [e.target.name] : e.target.value})
    }

    const onSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        authService.LoginSevice(lUser).then((res) => {
            if (res) {
                dispatch(LoginSlice({ userData: res }))
                navigate('/')
            }
        })
    }


    return (
        <div className="loginbody">
        <div className="container" id="container">
            <div className="form-container sign-in-container">
                <form>
                    <h2>Sign in</h2>
                    <span>or use your account</span>
                    <input type="text" name="Email" value={lUser.Email} onChange={onChange} placeholder="Enter Email" required />
                    <input type="password" name="Password" value={lUser.Password} onChange={onChange} placeholder="Enter Password" required />
                    <a href="#">Forgot your password?</a>
                    {/*<a href="#">Forgot your password?</a>*/}

                    <button disabled={lUser.Email.length<1 || lUser.Password.length<1}  type="submit" onClick={onSubmit} className="but">Sign In</button>
                </form>
            </div>
            <div className="overlay-container">
                <div className="overlay">

                    <div className="overlay-panel overlay-right">
                        <h1>Hello, Friend!</h1>
                        <p>Enter your personal details and start journey with us</p>

                        <Link to="/signup" className="but ghost"  style={{ color: "white" }}>Sign Up</Link>

                    </div>
                </div>
            </div>
            </div>
        </div>
    );
}

export default Login;