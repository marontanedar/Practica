using System;
using System.Threading;

namespace tarea1
{
        internal class Program
    {
        //public static int pacienteId = 1;
        public static Random rnd = new Random();
        private static void Main(string[] args)
        {
            for (int i = 1; i <= 10; i++)
            {
                Thread paciente = new Thread(()=>Paciente(i));
                paciente.Start();
                
                Thread.Sleep(rnd.Next(1, 10000));
                
            }
            
            

            


            
        }

        static void Paciente(int id)
        {
            Console.WriteLine("Paciente {0} generado",id);
        }
    }
}