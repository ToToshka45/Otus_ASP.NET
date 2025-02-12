import './Cat.css'

import CatFactGetButton from './CatFactGetButton';
import CatFactInfo from './CatFactInfo';
import CatFactError from './CatFactError';
import React, { useState } from 'react';

const CatFact = (props) => {

    // Используем хук useState для управления состоянием компонента
    const [catFacts, setCatFacts] = useState();
    const [catFactsError, setCatFactsError] = useState();

    const catFactsSuccessHandler = (data) => {
        console.log('Success...');
        setCatFacts(data);
        setCatFactsError();
    }

    const catFactsErrorHandler = (data) => {
        console.log('Error...');
        setCatFacts();
        setCatFactsError(data);
    }

    var container;
    if (catFactsError) {
        container = <CatFactError errorText={catFactsError}/>;;
    } else if (catFacts) {
        container = <CatFactInfo catFactsInfo={catFacts}/>;
    }

    return (
        <div className="cat-fact">

            Cat Facts

            <br/>

            {container}

            <br/>

            <CatFactGetButton onGetSuccess={catFactsSuccessHandler} onGetError={catFactsErrorHandler} />

        </div>
    );

  };
  
export default CatFact;
