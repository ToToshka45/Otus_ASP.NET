import { configureStore } from "@reduxjs/toolkit";
import counterReducer from "./components/counter/counterSlice";
import catFactReducer from "./components/Cat/catFactSlice";
import authReducer from "./components/authSlice";

export const store = configureStore({
  reducer: {
    counter: counterReducer,
    catFactInfo: catFactReducer,
    auth: authReducer,
  },
});
