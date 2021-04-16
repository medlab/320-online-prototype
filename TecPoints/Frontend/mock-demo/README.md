# 安装模块
   1. npm i -D axios
   2. npm i -D mockjs

### 新建MockData/data.js
    import Mock from 'mockjs';

    const mockdata = {
    'data|10': [
        {
        account: '@name',
        email: '@date',
        contact: false,
        },
    ],
    };

    Mock.mock('/listUsers', mockdata);

### 修改app.js
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