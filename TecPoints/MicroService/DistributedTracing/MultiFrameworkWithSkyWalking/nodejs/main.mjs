
import agent from 'skywalking-backend-js';

// //provide arguments if needed
agent.default.start();
//agent.start();

console.log({'agent':agent});

import express from 'express';
const app=express();
const port=9083

app.get('/', (req, res)=>{
	res.send('Hello World From Node Express!');
})

app.listen(port, ()=>{
	console.log(`Example app listening at http://localhost:${port}`)
})
