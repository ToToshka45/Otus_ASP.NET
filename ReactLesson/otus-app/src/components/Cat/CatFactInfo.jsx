import './Cat.css'
import React, { useState } from "react";
import { useSelector, useDispatch } from 'react-redux';
import {
  addComment,
  deleteComment,
  selectCommentsCount,
  selectComments,
} from "./catFactSlice";

const CatFactInfo = (props) => {
  const dispatch = useDispatch();

  const commentsCount = useSelector(selectCommentsCount);
  const comments = useSelector(selectComments);

  const [newComment, setNewComment] = useState("New Comment");

  var infoBox;
  if (props.catFactsInfo) {
    infoBox = props.catFactsInfo.map((item, i) => {
        return (
          <div className="row g-4 mb-2" key={i}>
            <div className="col-md-1">
              <div className="form-check">
                  <md-checkbox></md-checkbox>
              </div>
            </div>

            <div className="col-md-11">
                <p className="form-control-plaintext">{item.fact}</p>
            </div>
          </div>
    )})
  }

  return (
    <div className="container cat-fact-info-box">
      <div>
          Cat Fact Info
      </div>

      <div className="container">
        <div className="row g-4 mb-2">
          <div className="col-md-1">
            <label className="form-label">Нравится</label>
          </div>

          <div className="col-md-11">
            <label className="form-label">Факт о коте</label>
          </div>
        </div>

        {infoBox}

      </div>

      <div className="row">

        <div className="row">
          Comments count: {commentsCount}
        </div>

        <div className="row">
          <div className="col-md-11">
              <md-outlined-text-field 
                label="Текст" 
                placeholder="Введите ваш комментарий..."
                type="textarea"
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
                cols='100'>
              </md-outlined-text-field>
          </div>

          <div className="col-md-1">
            <md-outlined-button className="mb-1" onClick={ () => dispatch(addComment(newComment)) }>Добавить</md-outlined-button>
            <md-outlined-button onClick={ () => dispatch(deleteComment()) }>Удалить</md-outlined-button>
          </div>
        </div>

        <div className="row">
          {comments.map((item, i) => (
              <div className="row text-center" key={i}>
                  <p>
                    {item}
                  </p>
              </div>
            ))
          }
        </div>
      </div>
    </div>
  );
  
};

export default CatFactInfo;
