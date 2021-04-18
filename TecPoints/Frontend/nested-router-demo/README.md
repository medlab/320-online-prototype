# 普通路由

## 安装两个模块
    1. npm i -D react-router
    2. npm i -D react-router-dom

## 修改app.js

        function Category({ match }) {
        return <h1>Sub Category {match.params.subId}</h1>;
        }

        function SubCategory({ match }) {
        return (
            <div>
            <h1>Category {match.params.id}</h1>
            <ul id="menu">
                <li>
                <Link to={`/NestedRoute/category/${match.params.id}/sub/1`}>Sub Category 1</Link>
                </li>
                <li>
                <Link to={`/NestedRoute/category/${match.params.id}/sub/2`}>Sub Category 2</Link>
                </li>
                <li>
                <Link to={`/NestedRoute/category/${match.params.id}/sub/3`}>Sub Category 3</Link>
                </li>
            </ul>
            <div id="page-container-2">
                <Route path="/NestedRoute/category/:id/sub/:subId" component={Category} />
            </div>
            </div>
        );
        }

        function NestedRoute() {
        return (
            <Router>
            <div>
                <ul id="menu">
                <li>
                    <Link to="/NestedRoute/category/1">Category 1</Link>
                </li>
                <li>
                    <Link to="/NestedRoute/category/2">Category 2</Link>
                </li>
                <li>
                    <Link to="/NestedRoute/category/3">Category 3</Link>
                </li>
                </ul>

                <div id="page-container">
                <Route path="/NestedRoute/category/:id" component={SubCategory} />
                </div>
            </div>
            </Router>
        );
        }