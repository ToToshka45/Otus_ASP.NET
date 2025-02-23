import { NavLink } from "react-router-dom";

function setActiveRouteClassActive(isActive) {
    return isActive ? "active-route" : "";
}

export const NavBar = () => {
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
                <NavLink to={"/about"} className={ ({ isActive }) => { return setActiveRouteClassActive(isActive) } }>
                    About
                </NavLink>
                </li>
            </ul>
        </nav>
    );
};