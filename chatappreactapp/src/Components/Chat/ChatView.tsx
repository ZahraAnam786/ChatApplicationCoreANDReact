import { useSelector } from "react-redux";
import { authService } from '../../Services/authService'
import { useEffect, useState } from "react";
import frontImage from '../../assets/Images/front.jpg';
import ChatImage from '../../assets/Images/Chatimage.jpg';
import * as signalR from '@microsoft/signalr';
import defaultImage from '../../assets/Images/defaultuserimage.png';
//import './css/style.min.css';

function ChatView() {
    const authStatus = useSelector((state) => state.auth.status);
    const userdata = useSelector((state) => state.auth.userData);

    const userID = (userdata != null && userdata != undefined) ? userdata.userID : 0;

    const [users, setUsers] = useState([]);
    const [selectedUser, setSelectedUser] = useState(null);
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [message, setMessage] = useState('');
    const [searchUser, setSearchUser] = useState('');
    const [searchUserList, setSearchUserList] = useState([]);

    const fileURL = String(import.meta.env.VITE_API_FILE_PATH_URL);

    useEffect(() => {
        if (authStatus) {
            authService.getAllUser()
                .then((res) => {
                    if (res.users.length > 0) {
                        setUsers(res.users);
                        setSearchUserList(res.users);
                    }
                });

            const connect = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7263/chathub")
                .withAutomaticReconnect()
                .build();

            connect.start().then(() => {
                console.log("Connected to SignalR hub");
                connect.invoke("RegisterConnectionId", userID).catch(err => {
                  console.error("Invocation error:", err);
               });
            });

            connect.on("ReceiveMessage", (messageReceive) => {
                setMessages(prev => [...prev, messageReceive]);
            });

            setConnection(connect);
        }
    }, [authStatus]);


    const handleSelectedUser = (user) => {
        if (user != null && userID != null) {
            setSelectedUser(user);
            setMessages([]);
            authService.getChatMessages(userID, user.userID)
                .then((res) => {
                    if (res.messageList.length > 0)
                        setMessages(res.messageList);
                });
        }
    }

    const sendMessage = () => {
        if (connection && selectedUser.userID && message) {
            connection.invoke("Send", userID, selectedUser.userID, message);
            setMessage('');
            const now = new Date();
            const date = now.toLocaleDateString();    
            const time = now.toLocaleTimeString();  
            const currenttime = date + ' ' + time;
            setMessages(prev => [...prev, { isSender: true, message: message, timestamp: currenttime }]);
        }
    };

    const onChangeSearchInput = (e) => {
        setSearchUser(e.target.value);
        if (users.length > 0) {
            const suser = users.filter((f) => {
                if (f.userID != userID)
                    return f.userName.toLowerCase().includes(e.target.value.toLowerCase());
            });  
            setSearchUserList(suser);
        }
    }


    return (

        <div className="chatUIWrapper">
            <section className="chatUIContent">
                <div className="chatUIScroll">
                    <div className="chatUIContainer">
                        <div className="chatUIRow">
                            <div className="chatUIColumn">
                                <div className="chatUICard">
                                    {authStatus ? (
                                        <>
                                            <div className="chatUserSidebar">
                                                <div className="chatSearchBar">
                                                    <div className="chatSearchIcon">
                                                        <i className="zmdi zmdi-search"></i>
                                                    </div>
                                                    <input
                                                        type="text"
                                                        className="chatSearchInput"
                                                        placeholder="Search..."
                                                        required
                                                        value={searchUser}
                                                        onChange={onChangeSearchInput}
                                                    />
                                                </div>
                                                <ul className="chatUserList">
                                                    {/* Example User Entry (You'll map these from your 'users' prop) */}

                                                    {/*<li*/}
                                                    {/*    className="chatUserEntry">*/}
                                                    {/*    */}{/*onClick={() => setSelectedUser({ UserID: 2, userName: "Alice", Image: "https://via.placeholder.com/45" })}*/}

                                                    {/*    <input type="hidden" value="2" />*/}
                                                    {/*    <input type="hidden" value="yourUserID" />*/}
                                                    {/*    <img src="https://via.placeholder.com/45" alt="avatar" className="chatUserAvatar" />*/}
                                                    {/*    <div className="chatUserInfo">*/}
                                                    {/*        <div className="chatUserName">Alice</div>*/}
                                                    {/*        <div className="chatUserStatus">*/}
                                                    {/*            <i className="zmdi zmdi-circle"></i> online*/}
                                                    {/*        </div>*/}
                                                    {/*    </div>*/}
                                                    {/*</li>*/}
                                                    {searchUserList.map((item) =>
                                                        item.userID !== userID ? (
                                                            <li
                                                                className={`chatUserEntry ${selectedUser && selectedUser.userID === item.userID ? 'selected' : ''}`}
                                                                key={item.userID}
                                                                onClick={() => handleSelectedUser(item)}
                                                            >
                                                                <input type="hidden" value={item.userID} />
                                                                <img src={item.image ? fileURL + item.image : defaultImage} alt="avatar" className="chatUserAvatar" />
                                                                <div className="chatUserInfo">
                                                                    <div className="chatUserName">{item.userName}</div>
                                                                    <div className="chatUserStatus">
                                                                        <i className="zmdi zmdi-circle"></i> online
                                                                    </div>
                                                                </div>
                                                            </li>
                                                        ) : null
                                                    )}
                                                </ul>
                                            </div>

                                            <div className="chatUIWindow">
                                                {selectedUser != null ? (
                                                    <>
                                                        <div
                                                            className="chatUIHeader"
                                                            style={{ visibility: 'visible' }}>
                                                            <div className="chatUIUser">
                                                                <img src={selectedUser.image ? fileURL + selectedUser.image : defaultImage} alt="avatar" /> {/* Replace with actual defaultuserimage.png path */}
                                                                <div className="chatUIUserInfo">
                                                                    <div className="chatUIUserName">{selectedUser.userName}</div> {/* Replace with selectedUser?.userName */}
                                                                    <div className="chatUIMessageCount">already 8 messages</div>
                                                                </div>
                                                            </div>
                                                            <a href="#" className="chatUIToggle">
                                                                <i className="zmdi zmdi-comments"></i>
                                                            </a>
                                                        </div>

                                                        <img src={ChatImage} className="chatUIBackground" alt="chat" /> {/* Replace with ChatImage */}

                                                        <ul
                                                            className="chatUIHistory"
                                                            style={{ display: 'block' }}
                                                        >
                                                            {/* Placeholder Chat Messages - EXACTLY matching the image */}

                                                            {messages.map((item) =>
                                                                item.isSender ? (
                                                                    <li className="sent">
                                                                        <div className="chatUIMessageBubble sent">
                                                                            <span className="chatUIMessageContent">{item.message}</span>
                                                                            <span className="chatUIMessageTimestamp">{item.timestamp}<span className="chatUISenderIndicator">You</span></span>
                                                                        </div>
                                                                    </li>
                                                                ) : (<li className="received">
                                                                    <div className="chatUIMessageBubble received">
                                                                        <span className="chatUIMessageContent">{item.message}</span>
                                                                        <span className="chatUIMessageTimestamp">{item.timestamp}<span className="chatUISenderIndicator">{item.receiverName}</span></span>
                                                                    </div>
                                                                </li>
                                                                )
                                                            )}


                                                            {/* You would dynamically add more messages here */}
                                                        </ul>

                                                        <div
                                                            className="chatUIMessageBox"
                                                            style={{ visibility: 'visible' }}
                                                        >
                                                            <div className="chatUIInputGroup">
                                                                <input
                                                                    type="text"
                                                                    className="chatUIInput"
                                                                    value={message}
                                                                    onChange={e => setMessage(e.target.value)}
                                                                    placeholder="Enter text here..."
                                                                    required
                                                                />
                                                                <div className="chatUISendButton" onClick={() => sendMessage()}>
                                                                    <span className="chatUISendIcon">
                                                                        <i className="zmdi zmdi-mail-send"></i>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div></>) : (
                                                    <img src={ChatImage} className="chatUIBackground" alt="chat" />
                                                )}
                                            </div>
                                        </>
                                    ) : (
                                        <div className="chatUIWelcome">
                                            <center>WELCOME TO CHITCHAT</center>
                                            <img src={frontImage} alt="welcome" className="chatUIWelcomeImage" /> {/* Replace with frontImage */}
                                        </div>
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        //<div className="chatUIWrapper">
        //    <section className="chatUIContent">
        //        <div className="chatUIScroll">
        //            <div className="chatUIContainer">
        //                <div className="chatUIRow">
        //                    <div className="chatUIColumn">
        //                        <div className="chatUICard">
        //                            {authStatus ? (
        //                                <>
        //                                    <div className="chatUserSidebar">
        //                                        <div className="chatSearchBar">
        //                                            <div className="chatSearchIcon">
        //                                                <i className="zmdi zmdi-search"></i>
        //                                            </div>
        //                                            <input
        //                                                type="text"
        //                                                className="chatSearchInput"
        //                                                placeholder="Search..."
        //                                                required
        //                                            />
        //                                        </div>
        //                                        <ul className="chatUserList">
        //                                            {users.map((item) =>
        //                                                item.UserID !== userID ? (
        //                                                    <li
        //                                                        className="chatUserEntry"
        //                                                        key={item.UserID}
        //                                                        onClick={() => setSelectedUser(item)}
        //                                                    >
        //                                                        <input type="hidden" value={item.UserID} />
        //                                                        <input type="hidden" value={userID} />
        //                                                        <img src={item.Image} alt="avatar" className="chatUserAvatar" />
        //                                                        <div className="chatUserInfo">
        //                                                            <div className="chatUserName">{item.userName}</div>
        //                                                            <div className="chatUserStatus">
        //                                                                <i className="zmdi zmdi-circle"></i> online
        //                                                            </div>
        //                                                        </div>
        //                                                    </li>
        //                                                ) : null
        //                                            )}
        //                                        </ul>
        //                                    </div>

        //                                    <div className="chatUIWindow">
        //                                        <div
        //                                            className="chatUIHeader"
        //                                            style={{ visibility: selectedUser ? 'visible' : 'hidden' }}
        //                                        >
        //                                            <div className="chatUIUser">
        //                                                <img src="~/Images/defaultuserimage.png" alt="avatar" />
        //                                                <div className="chatUIUserInfo">
        //                                                    <div className="chatUIUserName">{selectedUser?.userName}</div>
        //                                                    <div className="chatUIMessageCount">already 8 messages</div>
        //                                                </div>
        //                                            </div>
        //                                            <a href="#" className="chatUIToggle">
        //                                                <i className="zmdi zmdi-comments"></i>
        //                                            </a>
        //                                        </div>

        //                                        <img src={ChatImage} className="chatUIBackground" alt="chat" />
        //                                        <hr />

        //                                        <ul
        //                                            className="chatUIHistory"
        //                                            style={{ display: selectedUser ? 'block' : 'none' }}
        //                                        >
        //                                            {/* Add chat messages here */}
        //                                        </ul>

        //                                        <div
        //                                            className="chatUIMessageBox"
        //                                            style={{ visibility: selectedUser ? 'visible' : 'hidden' }}
        //                                        >
        //                                            <div className="chatUIInputGroup">
        //                                                <input type="hidden" />
        //                                                <input type="hidden" />
        //                                                <input
        //                                                    type="text"
        //                                                    className="chatUIInput"
        //                                                    placeholder="Enter text here..."
        //                                                    required
        //                                                />
        //                                                <div className="chatUISendButton">
        //                                                    <span className="chatUISendIcon">
        //                                                        <i className="zmdi zmdi-mail-send"></i>
        //                                                    </span>
        //                                                </div>
        //                                            </div>
        //                                        </div>
        //                                    </div>
        //                                </>
        //                            ) : (
        //                                <div className="chatUIWelcome">
        //                                    <center>WELCOME TO CHITCHAT</center>
        //                                    <img src={frontImage} alt="welcome" className="chatUIWelcomeImage" />
        //                                </div>
        //                            )}
        //                        </div>
        //                    </div>
        //                </div>
        //            </div>
        //        </div>
        //    </section>
        //</div>

    );
}

export default ChatView;