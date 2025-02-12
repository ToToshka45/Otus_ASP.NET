import './Cat.css'

const CatFactInfo = (props) => {

  var infoBox;
  if (props.catFactsInfo) {
    infoBox = props.catFactsInfo.map((item, i) => {
        return (
            <li key = {i} className="cat-fact-info-box-item">
                <span>Fact: {item.fact} </span>
                <span>(</span><span className="cat-fact-info-box-item-length">Length: {item.length}</span><span>)</span>
            </li>
    )})
  }

  return (
    <div className="cat-fact-info-box">
        <div>
            Cat Fact Info: <br/> <br/> {props.catFactsInfo.total}
        </div>
        <ul>
            {infoBox}
        </ul>
    </div>
  );
  
};

export default CatFactInfo;
