import React from "react";
import { login } from "./authSlice"
import { useDispatch } from 'react-redux';

const Login = () => {
  const dispatch = useDispatch();

  return (
    <div className="login">
      <h3>Необходимо авторизоваться !!!</h3>
      <div>
        <md-outlined-button onClick={ () => dispatch(login()) }>Login</md-outlined-button>
      </div>
    </div>
  );
}
  
export default Login;