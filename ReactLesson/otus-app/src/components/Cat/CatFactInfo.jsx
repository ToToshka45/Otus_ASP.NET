import './Cat.css'

const CatFactInfo = (props) => {

  var infoBox;
  // if (props.catFactsInfo) {
  //   infoBox = props.catFactsInfo.map((item, i) => {
  //       return (
  //           <li key = {i} className="cat-fact-info-box-item">
  //               <span>Fact: {item.fact} </span>
  //               <span>(</span><span className="cat-fact-info-box-item-length">Length: {item.length}</span><span>)</span>
  //           </li>
  //   )})
  // }

  if (props.catFactsInfo) {
    infoBox = props.catFactsInfo.map((item, i) => {
        return (
          <div class="row g-4 mb-2">
            <div class="col-md-1">
              <div class="form-check">
                  <md-checkbox></md-checkbox>
              </div>
            </div>

            <div class="col-md-7">
                <p class="form-control-plaintext">{item.fact}</p>
            </div>

            <div class="col-md-4">
                <md-outlined-text-field label="Текст" placeholder="Введите ваш комментарий..." type='textarea'>
                </md-outlined-text-field>
            </div>
          </div>
    )})
  }

  return (
    <div className="container cat-fact-info-box">
        <div>
            Cat Fact Info: <br/> <br/> {props.catFactsInfo.total}
        </div>
        {/* <ul>
            {infoBox}
        </ul> */}

      <div class="container">
        <div class="row g-4 mb-2">
          <div class="col-md-1">
            <label class="form-label">Нравится</label>
          </div>

          <div class="col-md-7">
            <label class="form-label">Факт о коте</label>
          </div>

          <div class="col-md-4">
            <label for="comment" class="form-label">Комментарий</label>
          </div>
        </div>

        {infoBox}

      </div>
    </div>
  );
  
};

export default CatFactInfo;
