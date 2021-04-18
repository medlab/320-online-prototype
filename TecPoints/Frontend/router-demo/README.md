# 普通路由

## 安装两个模块
    1. npm i -D react-router
    2. npm i -D react-router-dom

## 修改app.js

    const Home = () => <h1>Home</h1>;
    const Hello = () => <h1>Hello</h1>;
    const About = () => <h1>About Us</h1>;

    export default function App() {
        return (
            <Router>
            <div>
                <ul id="menu">
                <li>
                    <Link to="/RouterSample/home">Home</Link>
                </li>
                <li>
                    <Link to="/RouterSample/hello">Hello</Link>
                </li>
                <li>
                    <Link to="/RouterSample/about">About</Link>
                </li>
                </ul>
        
                <div id="page-container">
                <Route path="/RouterSample/home" component={Home} />
                <Route path="/RouterSample/hello" component={Hello} />
                <Route path="/RouterSample/about" component={About} />
                </div>
            </div>
            </Router>
        );
    }