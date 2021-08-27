import React from "react";
import { SimpleLife } from "./SimpleLifeComponent";

function App() {
  return <SimpleLife width={window.innerWidth} height={window.innerHeight} rows={35} columns={35}></SimpleLife>;
}

export default App;
