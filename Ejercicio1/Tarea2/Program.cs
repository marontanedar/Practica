using System;
using System.Collections.Generic;
using System.Threading;

namespace Hospital
{
    /// <summary>
    /// Información relacionada al paciente
    /// </summary>
    public class Paciente
    {
        public int Id { get; set; }  // Identificador paciente
        public int LlegadaHospital { get; set; }  // Tiempo de llegada
        public int TiempoConsulta { get; set; }  // Duración de la consulta
        public string Estado { get; set; }  // Estado del paciente

        public Paciente(int id, int llegadaHospital, int tiempoConsulta)
        {
            Id = id;
            LlegadaHospital = llegadaHospital;
            TiempoConsulta = tiempoConsulta;
            Estado = "Espera"; // Estado inicial
        }
    }

    internal class Program
    {
        static SemaphoreSlim sem = new SemaphoreSlim(4);  // 4 médicos disponibles
        static readonly object locker = new object();  
        static Random rnd = new Random();
        static List<int> medicosOcupados = new List<int>(); // Médicos en consulta
        static int tiempoGlobal = 0; // Simulación del tiempo transcurrido en segundos
        static int llegadaCounter = 0; // Contador de orden de llegada


        private static void Main(string[] args)
        {
            List<Paciente> pacientes = new List<Paciente>(); //Lista 

            for (int i = 1; i <= 4; i++)
            {
                int idPaciente = rnd.Next(1, 101);  // ID aleatorio entre 1 y 100
                int tiempoConsulta = rnd.Next(5, 16); // Tiempo de consulta entre 5 y 15 segundos
                llegadaCounter++;

                Paciente paciente = new Paciente(idPaciente, tiempoGlobal, tiempoConsulta);
                pacientes.Add(paciente);

                Thread pacienteThread = new Thread(PacienteAtencion);
                pacienteThread.Start(paciente);

                tiempoGlobal += 2;  // Simula la llegada de pacientes cada 2 segundos
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Método que maneja la consulta de un paciente.
        /// </summary>
        /// <param name="obj">Objeto Paciente</param>
        static void PacienteAtencion(object obj)
        {
            Paciente paciente = (Paciente)obj;
            int medico;

            Console.WriteLine($"Paciente {paciente.Id} ha llegado al hospital en el segundo {paciente.LlegadaHospital} llegó {llegadaCounter}. Estado: {paciente.Estado}");

            // Esperar hasta que haya un médico disponible
            sem.Wait();

            lock (locker)
            {
                do
                {
                    medico = rnd.Next(1, 5);
                } while (medicosOcupados.Contains(medico));
                medicosOcupados.Add(medico);
                paciente.Estado = "Consulta";
            }

            Console.WriteLine($"  Paciente {paciente.Id} en consulta con médico Nº {medico} llegó {llegadaCounter}. Tiempo estimado: {paciente.TiempoConsulta} segundos.");
            Thread.Sleep(paciente.TiempoConsulta * 1000); // Simula la consulta

            lock (locker)
            {
                paciente.Estado = "Finalizado";
                medicosOcupados.Remove(medico);
                Console.WriteLine($"    Paciente {paciente.Id} ha finalizado su consulta con el médico Nº {medico}. Estado: {paciente.Estado}");
            }

            sem.Release();
        }
    }
}