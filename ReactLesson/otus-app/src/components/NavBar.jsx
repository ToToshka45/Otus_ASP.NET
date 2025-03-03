import { NavLink } from "react-router-dom";
import { logout, isAuth } from "./authSlice"
import { useDispatch, useSelector } from 'react-redux';

function setActiveRouteClassActive(isActive) {
    return isActive ? "active-route" : "";
}

export const NavBar = () => {
    const dispatch = useDispatch();

    const isLoggedIn = useSelector(isAuth);

    var loginOrLogoutBtn;
    if (isLoggedIn) {
        loginOrLogoutBtn = <md-outlined-button onClick={ () => dispatch(logout()) }>Logout</md-outlined-button>
    } else {
        loginOrLogoutBtn = <NavLink to={"/login"} className={ ({ isActive }) => { return setActiveRouteClassActive(isActive) } }>Login</NavLink>
    }

    return (
        <nav className="header">
            <ul>
                <li>
                <NavLink to={"/"} className={ ({ isActive }) => { return setActiveRouteClassActive(isActive) } }>
                    Home
                </NavLink>
                </li>
                <li>
                <NavLink to={"/catFact"} className={ ({ isActive }) => { return setActiveRouteClassActive(isActive) } }>
                    Cat Fact
                </NavLink>
                </li>
                <li>
                <NavLink to={"/counter"} className={ ({ isActive }) => { return setActiveRouteClassActive(isActive) } }>
                    Counter
                </NavLink>
                </li>
                <li>
                <NavLink to={"/about"} className={ ({ isActive }) => { return setActiveRouteClassActive(isActive) } }>
                    About
                </NavLink>
                </li>
            </ul>
            <div className="logout-bar">
                {loginOrLogoutBtn}
            </div>
        </nav>
    );
};