import React, { useState, useEffect } from 'react';
import axios from 'axios';
import mock from './MockData/data';


function App() {
  const [mockdata, setMockData] = useState([]);

  useEffect(() => {
    console.log(1);
    axios.get('/listUsers').then((data) => {
      console.log(data.data.data);
      setMockData(data.data.data);
    });
  }, []);

  return (
    <>
      <h3>Mock数据展示</h3>
      <ul>
        {mockdata.map((item, i) => (
          <li key={item.account}>
            <span>
              {item.account} ++ {item.email} : {item.contact.toString()}
            </span>
          </li>
        ))}
      </ul>
    </>
  );
};

export default App;
