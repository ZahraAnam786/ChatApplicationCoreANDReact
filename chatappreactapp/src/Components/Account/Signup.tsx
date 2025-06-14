import './Account.css';
import { useState } from "react";
import addImage from '../../assets/Images/add.png';
import defaultImage from '../../assets/Images/defaultuserimage.png';
import { Link, useNavigate } from "react-router-dom";
import { useDispatch } from 'react-redux';
import { authService } from '../../Services/authService'
import { login as LoginSlice } from '../../Features/AuthSlice';
import { validateFields } from "../utils/validation"; 


const Signup = () => {

    const dispatch = useDispatch();
    const navigate = useNavigate();
    const fileBaseURL = String(import.meta.env.VITE_API_FILE_PATH_URL);

    const [sUser, setsUser] = useState({
        UserName: "", Image: "" , Email: "", Password: "", ConfirmPassword: ""
    });

    const [errors, setErrors] = useState<Record<string, string>>({});

    const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setsUser({ ...sUser, [e.target.name]: e.target.value })
    }

    const validationRules = {
        UserName: (val: string) => (!val ? "Username is required" : ""),
        Image: (val: string) => (!val ? "Profile Image is required" : ""),
        Email: (val: string) =>
            !val ? "Email is required" : !/\S+@\S+\.\S+/.test(val) ? "Invalid email" : "",
        Password: (val: string) =>
            !val ? "Password is required" : val.length < 6 ? "Must be 6+ chars" : "",
        ConfirmPassword: (val: string, all: any) =>
            !val ? "Confirm Password required" : val !== all.Password ? "Passwords do not match" : "",
    };



    const onSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        const validationErrors = validateFields(sUser, validationRules);

        setErrors(validationErrors);
        if (Object.keys(validationErrors).length == 0) {
            authService.RegisterSevice(sUser).then((res) => {
                if (res) {
                    dispatch(LoginSlice({ userData: res }))
                    navigate('/')
                }
            })
        }    
    }


    //-------------------File Upload--------------------

    const handleFileChange =  (e: React.ChangeEvent<HTMLInputElement>) => {
        const selectedFile = e.target.files?.[0];
        if (selectedFile) {
            const formData = new FormData();
            formData.append('file', selectedFile);
            authService.FileUploadSevice(formData).then((res) => {
                if (res.filePath != "")
                    setsUser({
                        ...sUser, image: res.fileName
                    })
            });
        }
    };



    return (
        <>
            <div className="loginbody">
            <div className="container right-panel-active" id="container">
                <div className="form-container sign-up-container">

                    <form>
                        <h2 style={{ marginTop: "0px" }}>Create Account</h2>

                        <div className="text-center upload">
                                <img src={sUser.image ? fileBaseURL + sUser.image : defaultImage} id="targetimg" className="img-rounded float-end imge" width="70px" />
                            <div className="round">
                                    <input type="file" onChange={handleFileChange} required accept="image/*" />
                                    <img src={addImage} style={{ width: "25px", height: "25px" }} />
                            </div>
                            <input type="text" hidden id="image" value={sUser.image} readOnly name="Image" />
                        </div>

                        <input type="text" name="UserName" value={sUser.UserName} onChange={onChange} placeholder="Enter Name" />
                        {errors.UserName && <div style={{ color: "red" }}>{errors.UserName}</div>}

                        <input type="text" name="Email" value={sUser.Email} onChange={onChange} placeholder="Enter Email" required />
                        {errors.Email && <div style={{ color: "red" }}>{errors.Email}</div>}

                        <input type="password" name="Password" value={sUser.Password} onChange={onChange} placeholder="Enter Password" required />
                        {errors.Password && <div style={{ color: "red" }}>{errors.Password}</div>}

                        <input type="password" name="ConfirmPassword" value={sUser.ConfirmPassword} onChange={onChange} placeholder="Enter Confirm Password" required />
                        {errors.ConfirmPassword && <div style={{ color: "red" }}>{errors.ConfirmPassword}</div>}

                        <button type="submit" onClick={onSubmit} className="but">Sign Up</button>
                    </form>
                </div>
                <div className="overlay-container">
                    <div className="overlay">
                        <div className="overlay-panel overlay-left">
                            <h1>Welcome Back!</h1>
                            <p>To keep connected with us please login with your personal info</p>
                            <Link to="/login" className="but ghost" id="signIn" style={{ color: "white" }}>Sign In</Link>

                        </div>

                    </div>
                </div>
                </div >
            </div>
        </>
    )
}

export default Signup;