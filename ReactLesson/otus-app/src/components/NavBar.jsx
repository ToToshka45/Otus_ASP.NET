import { NavLink } from "react-router-dom";
import { logout } from "./authSlice"
import { useDispatch } from 'react-redux';

function setActiveRouteClassActive(isActive) {
    return isActive ? "active-route" : "";
}

export const NavBar = () => {
    const dispatch = useDispatch();

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
                <li>
                <NavLink to={"/login"} className={ ({ isActive }) => { return setActiveRouteClassActive(isActive) } }>
                    Login
                </NavLink>
                </li>
            </ul>
            <div>
                <md-outlined-button onClick={ () => dispatch(logout()) }>Logout</md-outlined-button>
            </div>
        </nav>
    );
};