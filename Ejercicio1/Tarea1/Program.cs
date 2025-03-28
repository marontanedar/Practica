using System;
using System.Collections.Generic;
using System.Threading;

namespace tarea1
{
    internal class Program
    {
        static SemaphoreSlim sem = new SemaphoreSlim(4);
        static readonly object locker = new object();
        static Random rnd = new Random();
        static List<int> medicosOcupados = new List<int>(); //lista de medicos asignados

        private static void Main(string[] args)
        {
            for (int i = 1; i <= 4; i++)
            {
                Thread paciente = new Thread(Paciente);
                paciente.Start(i);
                Thread.Sleep(2000);
            }
        }

        static void Paciente(object id)
        {
            int pacienteId = (int)id;
            int medico;

            // Asignar médico disponible
            lock (locker)
            {
                do
                {
                    medico = rnd.Next(1, 5);
                } while (medicosOcupados.Contains(medico));
                medicosOcupados.Add(medico);
            }

            Console.WriteLine("Paciente {0} en sala de espera", pacienteId);
            sem.Wait();

            Console.WriteLine("  Paciente {0} en consulta con medico Nº {1}", pacienteId, medico);
            Thread.Sleep(10000);
            Console.WriteLine("    Paciente {0} finaliza con medico Nº {1}", pacienteId, medico);

            // Liberar médico
            lock (locker)
            {
                medicosOcupados.Remove(medico);
            }

            sem.Release();
        }
    }
}
