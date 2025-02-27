import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  comments: []
};

export const catFactSlice = createSlice({
  name: "catFactInfo",
  initialState,
  // The `reducers` field lets us define reducers and generate associated actions
  reducers: {
    addComment: (state, action) => {
      state.comments.push(action.payload);
    },
    deleteComment: (state) => {
      state.comments.pop();
    },
  },
});

export const { addComment, deleteComment } = catFactSlice.actions;

export const selectCommentsCount = (state) => state.catFactInfo.comments.length;
export const selectComments = (state) => state.catFactInfo.comments;

export default catFactSlice.reducer;
