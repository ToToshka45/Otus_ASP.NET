import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  isLoggedIn: localStorage.getItem('isLoggedIn') == 'true',
};

export const authSlice = createSlice({
  name: "auth",
  initialState,
  // The `reducers` field lets us define reducers and generate associated actions
  reducers: {
    login: (state) => {
      state.isLoggedIn = true;
      localStorage.setItem('isLoggedIn', true);
      console.log('Login');
    },
    logout: (state) => {
      state.isLoggedIn = false;
      localStorage.setItem('isLoggedIn', false);
      console.log('Logout');
    },
  },
});

export const { login, logout } = authSlice.actions;

export const isAuth = (state) => state.auth.isLoggedIn;

export default authSlice.reducer;