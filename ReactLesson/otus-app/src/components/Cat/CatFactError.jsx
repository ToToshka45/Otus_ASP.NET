import './Cat.css'

const CatFactError = (props) => {

  return (
    <div className="cat-fact-error">
        При получении данных произошла ошибка: {props.errorText}
    </div>
  );

};

export default CatFactError;
