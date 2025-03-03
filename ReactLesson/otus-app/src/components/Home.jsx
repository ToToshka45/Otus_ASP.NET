import { useSelector } from 'react-redux';
import {
  selectComments,
} from "./Cat/catFactSlice";

const Home = () => {
  const comments = useSelector(selectComments);

    return (
      <div>
        <div className="home">
          <h1>Домашняя страница</h1>
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
  
  export default Home;