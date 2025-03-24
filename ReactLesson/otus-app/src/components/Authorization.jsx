import { isAuth } from "./authSlice"
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';

const withAuth = (Component) => {
  const AuthenticatedComponent = (props) => {
    const isLoggedIn = useSelector(isAuth);
    const navigate = useNavigate();

    useEffect(() => {
      if (!isLoggedIn) {
        navigate("/login");
      }
    }, [isLoggedIn]);

    return <Component {...props} />;
  };

  return <AuthenticatedComponent />;
};

export { withAuth };