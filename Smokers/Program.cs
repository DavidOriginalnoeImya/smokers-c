using System;
using System.Collections.Generic;
using System.Threading;

namespace Smokers
{
    internal class Program
    {
        private static Random random = new Random();

        private static List<Semaphore> componentSemaphores = new List<Semaphore>
        {
            new Semaphore(0, 1), new Semaphore(0, 1), new Semaphore(0, 1)
        };

        private static Semaphore agentSemaphore = new Semaphore(0, 1);
        
        public static void Main(string[] args)
        {
            Thread smoker1Thread = new Thread(_ => Smoker(0));
            Thread smoker2Thread = new Thread(_ => Smoker(1));
            Thread smoker3Thread = new Thread(_ => Smoker(2));
            
            Thread agentThread = new Thread(Agent);
            
            smoker1Thread.Start();
            smoker2Thread.Start();
            smoker3Thread.Start();
            
            agentThread.Start();
        }

        public static void Agent()
        {
            while (true)
            {
                int componentIndex = random.Next(0, 3);
                
                Console.WriteLine("Агент выкладывает компонент " + (componentIndex + 1));
                
                int work = 1;
                
                for (int workCount = 0; workCount < 1000000; ++workCount)
                {
                    work += work * workCount * 2 + (int) Math.Pow(workCount, workCount) * (int) Math.Pow(workCount, workCount);
                }
                
                componentSemaphores[componentIndex].Release();

                agentSemaphore.WaitOne();
            }
        }
        
        public static void Smoker(int smokerIndex)
        {
            while (true)
            {
                componentSemaphores[smokerIndex].WaitOne();
                    
                Console.WriteLine("Курильщик " + (smokerIndex + 1) + " курит");
                
                int work = 1;
                
                for (int workCount = 0; workCount < 10000000; ++workCount)
                {
                    work += work * workCount * 2 + (int) Math.Pow(workCount, workCount) * (int) Math.Pow(workCount, workCount);
                }
                
                Console.WriteLine("Курильщик " + (smokerIndex + 1) + " закончил курить");

                agentSemaphore.Release();
            }
        }
    }
}