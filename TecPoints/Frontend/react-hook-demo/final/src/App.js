// 在使用过程中，useEffect方法需要传入两个参数，第一个参数是回调函数：这个回调函数会在每次组件渲染后执行，包括初始化渲染以及每次更新时。
// 另一个参数，则是状态依赖项(数组形式)，一旦检测到依赖项数据变动，组件会更新，并且回调函数都会被再次执行一遍，从而实现componentDidUpdate的功能。
// 如果你传入一个空依赖，就能实现原来componentDidMount的效果，即只会执行一次。回调函数中如果返回的是闭包，这个返回的闭包函数将会在组件重新渲染前执行，
// 所以你可以在这个位置做一些清理操作，从而实现componentWillUnmount的功能。
// 还有要注意的是componentWillMount和componentWillUpdate两个生命周期方法在新版本的React中已经不推荐使用了

// use class
// class Person extends React.Component {
//   constructor(props) {
//     super(props);
//     this.state = {
//       username: 'scq000',
//     };
//   }

//   componentDidMount() {
//     console.log('componentDidMount: 组件加载后');
//   }

//   componentDidUpdate(prevProps, prevState) {
//     if (prevState.username !== this.state.username) {
//       console.log('componentDidUpdate： 更新usernmae');
//     }
//   }

//   componentWillUnmount() {
//     console.log('componentWillUnmount: 组件卸载， 做一些清理工作');
//   }

//   render() {
//     return (
//       <>
//         <p>Welcome to homepage. {this.state.username}</p>
//         <input type="text" placeholder="input a username" onChange={(event) => this.setState({ username: event.target.value })} />
//       </>
//     );
//   }
// }

// export default Person;



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

export default App;
