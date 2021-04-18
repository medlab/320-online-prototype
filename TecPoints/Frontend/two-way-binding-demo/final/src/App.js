import React, { useState } from "react";

const App = () => {
  const [count, setCount] = useState(0);
  const [text, setText] = useState("");

  const handleClick = () => {
    console.log("click");
    const newcount = count + 1;
    setCount(newcount);
  };

  const handleChange = (e) => {
    setText(e.target.value);
    console.log(e);
  };

  return (
    <>
      <button onClick={handleClick} type="button">
        button
      </button>
      <h1>count:{count}</h1>
      <input type="text" value={text} onChange={handleChange} />
      <h1>input text: {text} </h1>
    </>
  );
};

export default App;
