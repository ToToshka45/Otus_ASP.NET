import './App.css';
import CatFact from './components/Cat/CatFact';
import { NavBar } from "./components/NavBar";
import { Route, Routes } from 'react-router-dom';
import Home from './components/Home';
import About from './components/About';
import Login from './components/Login';
import Register from './components/Register';
import NotFound404 from './components/NotFound404';

function App() {

  return (
    <>
     <div className="App">
      <NavBar />

      <Routes>
        <Route index element={<Home />} />
        <Route path="catFact" element={<CatFact />} />
        <Route path="about" element={<About />} />
        <Route path="login" element={<Login />} />
        <Route path="register" element={<Register />} />
        <Route path="notFound404" element={<NotFound404 />} />
      </Routes>
     </div>
    </>
  );

}

export default App;
