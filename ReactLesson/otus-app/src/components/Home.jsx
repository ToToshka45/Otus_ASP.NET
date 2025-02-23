import { useSelector, useDispatch } from 'react-redux';
import {
  selectComments,
} from "./Cat/catFactSlice";

const Home = (props) => {

  //const comments = useSelector(selectComments);

    return (
      <div>
        <div className="home">
          Домашняя страница
        </div>
        {/* <div className="row">
          {comments.map((item, i) => {
            return (
              <div className="row text-center" key={i}>
                <div className="col-xs-12 center-block text-center">
                  <p>
                    {item}
                  </p>
                </div>
              </div>
            )})
          }
        </div> */}
      </div>
    );
  
  };
  
  export default Home;