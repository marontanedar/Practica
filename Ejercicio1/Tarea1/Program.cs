using System;
using System.Collections.Generic;
using System.Threading;

namespace tarea1
{
    /// <summary>
    /// Clase principal que simula la atención de pacientes en un hospital con médicos limitados
    /// </summary>
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

        /// <summary>
        /// Método que simula la atención de un paciente en el hospital.
        /// </summary>
        /// <param name="id">Id del paciente</param>
        static void Paciente(object id)
        {
            int pacienteId = (int)id;
            int medico;

            // Asignar médico disponible
            lock (locker)
            {
                do
                {
                    medico = rnd.Next(1, 5); //médico aleatorio entre 1 y 4
                } while (medicosOcupados.Contains(medico)); //Verifica que el médico no este ocupado
                medicosOcupados.Add(medico); // Marcar al médico como ocupado
            }

            Console.WriteLine("Paciente {0} en sala de espera", pacienteId);
            sem.Wait(); // Esperando a un médico disponible

            Console.WriteLine("  Paciente {0} en consulta con medico Nº {1}", pacienteId, medico);
            Thread.Sleep(10000); //Duración de cada consulta con cada uno de los pacientes (10 segundos)
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
