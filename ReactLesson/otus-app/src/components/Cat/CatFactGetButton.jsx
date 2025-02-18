import './Cat.css'

const CatFactGetButton = (props) => {

    return (
    <div>
        <button className="cat-fact-get-button"
            onClick={() => {
                fetch('https://catfact.ninja/facts')
                    .then(response => response.json())
                    .then(data => {
                        if (props.onGetSuccess) {
                            props.onGetSuccess(data.data);
                        }
                    })
                    .catch(error => {
                        if (props.onGetError) {
                            props.onGetError(error.message);
                        }
                    });
            }}
        >

            Get Cat Facts !!!

        </button>
    </div>
  );

};

export default CatFactGetButton;

