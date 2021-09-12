# 背景

c# 的 async await 极大提升了异步编程生产力，但是同时也引入了新的挑战，比如async await场景下的锁问题。

本文汇编了一个生产者、消费者队列示例，对async await 场景友好。

```c#
public class AsyncProducerConsumerCollection<T>
{
    private readonly Queue<T> m_collection = new Queue<T>();
    private readonly Queue<TaskCompletionSource<T>> m_waiting =
        new Queue<TaskCompletionSource<T>>();

    public void Add(T item)
    {
        TaskCompletionSource<T> tcs = null;
        lock (m_collection)
        {
            if (m_waiting.Count > 0) tcs = m_waiting.Dequeue();
            else m_collection.Enqueue(item);
        }
        if (tcs != null) tcs.TrySetResult(item);
    }

    public Task<T> Take()
    {
        lock (m_collection)
        {
            if (m_collection.Count > 0)
            {
                return Task.FromResult(m_collection.Dequeue());
            }
            else
            {
                var tcs = new TaskCompletionSource<T>();
                m_waiting.Enqueue(tcs);
                return tcs.Task;
            }
        }
    }
}
```

# 示例程序

场景介绍，本文定义了一个生产者和两个消费者
1. 生产者两秒生成一个产品(数字)
2. 两个消费者消费持续消费产品
3. 生产者生产200个产品后会退出

```c#
    public class AsyncProducerConsumerCollection<T>
    {
        private readonly Queue<T> m_collection = new Queue<T>();
        private readonly Queue<TaskCompletionSource<T>> m_waiting =
            new Queue<TaskCompletionSource<T>>();

        public void Add(T item)
        {
            TaskCompletionSource<T> tcs = null;
            lock (m_collection)
            {
                if (m_waiting.Count > 0) tcs = m_waiting.Dequeue();
                else m_collection.Enqueue(item);
            }
            if (tcs != null) tcs.TrySetResult(item);
        }

        public Task<T> Take()
        {
            lock (m_collection)
            {
                if (m_collection.Count > 0)
                {
                    return Task.FromResult(m_collection.Dequeue());
                }
                else
                {
                    var tcs = new TaskCompletionSource<T>();
                    m_waiting.Enqueue(tcs);
                    return tcs.Task;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AsyncProducerConsumerCollection<int> collection=new();
            var produce=Task.Run(async ()=>{
                int i=byte.MaxValue;
                var random=new Random();
                while(i-->0){
                    var randint=random.Next();
                    collection.Add(randint);
                }
                Console.WriteLine($"work done, cool down");
                await Task.Delay(TimeSpan.FromSeconds(10));
                Console.WriteLine($"finish cool down");
            });

            var consumer1=Task.Run(async ()=>{
                while(true)
                {
                    var data=await collection.Take();
                    Console.WriteLine($"consumer1:{data}");
                }
            });
            
            var consumer2=Task.Run(async ()=>{
                while(true)
                {
                    var data=await collection.Take();
                    Console.WriteLine($"consumer2:{data}");
                }
            });

            produce.Wait();
            Console.WriteLine("Bye Bye!");
        }
    }

```

# 写在后面

BufferBlock是一个更加现成的类，可以安装System.Threading.Tasks.Dataflow包 

# 参考
1. https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/consuming-the-task-based-asynchronous-pattern