import React from "react";
import { BrowserRouter as Router, Route, Link } from "react-router-dom";

const Topic = ({ match }) => <h1>Topic {match.params.id}</h1>;

function App() {
  return (
    <Router>
      <div>
        <ul id="menu">
          <li>
            <Link to="/RouterParams/topic/1">Topic 1</Link>
          </li>
          <li>
            <Link to="/RouterParams/topic/2">Topic 2</Link>
          </li>
          <li>
            <Link to="/RouterParams/topic/3">Topic 3</Link>
          </li>
        </ul>

        <div id="page-container">
          <Route path="/RouterParams/topic/:id" component={Topic} />
        </div>
      </div>
    </Router>
  );
}

export default App;
