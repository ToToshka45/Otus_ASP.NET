import React from "react";
import { login } from "./authSlice"
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  function performLogin() {
    dispatch(login());

    navigate("/");
  };
  
  return (
    <div className="login">
      <h3>Необходимо авторизоваться !!!</h3>
      <div>
        <md-outlined-button onClick={ () => performLogin() }>Login</md-outlined-button>
      </div>
    </div>
  );
}
  
export default Login;