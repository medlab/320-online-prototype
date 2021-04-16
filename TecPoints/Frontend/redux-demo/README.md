# 1.安装redux 
    npm i -D redux

# 2.修改index.js

        const Counter = ({ value, onIncrement, onDecrement }) => (
        <div>
            <h1>{value}</h1>
            <button type="button" onClick={onIncrement}>
            +
            </button>
            <button type="button" onClick={onDecrement}>
            -
            </button>
        </div>
        );

        const reducer = (state = 0, action) => {
        switch (action.type) {
            case 'INCREMENT':
            return state + 1;
            case 'DECREMENT':
            return state - 1;
            default:
            return state;
        }
        };

        const store = createStore(reducer);

        const render = () => {
        ReactDOM.render(
            <React.StrictMode>
            <Counter
                value={store.getState()}
                onIncrement={() => store.dispatch({ type: 'INCREMENT' })}
                onDecrement={() => store.dispatch({ type: 'DECREMENT' })}
                />
            </React.StrictMode>,
            document.getElementById('root')
        );
        }