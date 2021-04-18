# 使用react hook 以及不使用 react hook 

## 修改app.js
        import React, { useState, useEffect } from 'react';

        function App() {
        const [state, setState] = useState({ username: 'scq000' });

        useEffect(() => {
            console.log('componentDidMount: 组件加载后');
            return () => {
            console.log('componentWillUnmount: 组件卸载， 做一些清理工作');
            };
        }, []);

        useEffect(() => {
            console.log('componentDidUpdate： 更新usernmae');
        }, [state.username]);

        return (
            <>
            <p>Welcome to homepage. {state.username}</p>
            <input type="text" placeholder="input a username" onChange={(event) => setState({ username: event.target.value })} />
            </>
        );
        };
