import React from "react";
import "./App.css";
import Routes from "./Routes";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

toast.configure({
  autoClose: 2000,
  draggable: false,
  position: toast.POSITION.BOTTOM_RIGHT
});

function App() {
  return <Routes />;
}

export default App;
