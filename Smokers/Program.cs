using System;
using System.Collections.Generic;
using System.Threading;

namespace Smokers
{
    internal class Program
    {
        private static List<Mutex> smokerMutexes = new List<Mutex> {new Mutex(), new Mutex(), new Mutex()};

        private static int component = -1;
        
        private static Random random = new Random();
        
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
                if (component == -1)
                {
                    component = random.Next(0, 3);
                    
                    Console.WriteLine("Агент выкладывает компонент " + (component + 1));
                }
            }
        }
        
        public static void Smoker(int smokerIndex)
        {
            while (true)
            {
                smokerMutexes[smokerIndex].WaitOne();

                if (smokerIndex == component)
                {
                    component = -1;
                    
                    Console.WriteLine("Курильщик " + (smokerIndex + 1) + " курит");
                    
                    Thread.Sleep(1000);
                    
                    Console.WriteLine("Курильщик " + (smokerIndex + 1) + " закончил курить");
                }
                
                smokerMutexes[smokerIndex].ReleaseMutex();
            }
        }
    }
}