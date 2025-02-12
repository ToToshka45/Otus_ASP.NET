import logo from './logo.svg';
import './App.css';
import {ClassBasedButton} from './components/ClassBasedButton.jsx';
import {FunctionBasedButton} from './components/FunctionBasedButton.jsx';
import CatFact from './components/Cat/CatFact';
import Counter from './components/Counter';

function App() {

  let par = "!";

  const newWayClick = (d) => {
    console.log('log something ...');
    par += '!';
  }

  return <>
     <div className="App">
       <header className="App-header">
         <CatFact />
       </header>
     </div>

     {/* <br />
    <ClassBasedButton fancyText={par} chislo={() => 1234}/>
    <br />
    <ClassBasedButton fancyText={'text2'} chislo={1} param={10*42}/>
    <br />
    <FunctionBasedButton newText={'newText1'} onFancyClick={newWayClick} />
    <br />
    <FunctionBasedButton newText={'newText2'} after={<span>TEXT</span>} />
    <br />
    <FunctionBasedButton/>
    <br />
    <Counter /> */}
  </>;
  // return (
  //   <div className="App">
  //     <header className="App-header">
  //       <img src={logo} className="App-logo" alt="logo" />
  //       <p>
  //         Edit <code>src/App.js</code> and save to reload.
  //       </p>
  //       <a
  //         className="App-link"
  //         href="https://reactjs.org"
  //         target="_blank"
  //         rel="noopener noreferrer"
  //       >
  //         Learn React
  //       </a>
  //     </header>
  //   </div>
  // );
}

export default App;
