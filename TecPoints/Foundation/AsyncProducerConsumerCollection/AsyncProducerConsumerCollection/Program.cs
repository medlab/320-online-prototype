using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncProducerConsumerCollection
{
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
}
