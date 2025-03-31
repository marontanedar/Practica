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
        public int Id { get; set; }  // Identificador del paciente
        public int LlegadaHospital { get; set; }  // Tiempo de llegada
        public int TiempoConsulta { get; set; }  // Duración de la consulta
        public string Estado { get; set; }  // Estado actual del paciente
        public int OrdenLlegada { get; set; }  // Orden de llegada
        public bool RequiereDiagnostico { get; set; }  //Requiere diagnóstico adicional?

        public Paciente(int id, int llegadaHospital, int tiempoConsulta, int ordenLlegada, bool requiereDiagnostico)
        {
            this.Id = id;
            this.LlegadaHospital = llegadaHospital;
            this.TiempoConsulta = tiempoConsulta;
            this.Estado = "EsperaConsulta"; // Estado inicial
            this.OrdenLlegada = ordenLlegada;
            this.RequiereDiagnostico = requiereDiagnostico;
        }
    }

    internal class Program
    {
        static SemaphoreSlim sem = new SemaphoreSlim(4);  // 4 médicos disponibles
        static SemaphoreSlim maquinasDiagnostico = new SemaphoreSlim(2);  // 2 máquinas de diagnóstico disponibles
        static readonly object locker = new object();
        static Random rnd = new Random();
        static List<int> medicosOcupados = new List<int>(); // Médicos en consulta
        static int tiempoGlobal = 0; // Simulación del tiempo transcurrido en segundos
        static int llegadaCounter = 0; // Contador de orden de llegada

        private static void Main(string[] args)
        {
            List<Paciente> pacientes = new List<Paciente>(); // Lista de pacientes

            for (int i = 1; i <= 4; i++)
            {
                int idPaciente = rnd.Next(1, 101);  // ID aleatorio entre 1 y 100
                int tiempoConsulta = rnd.Next(5, 16); // Tiempo de consulta entre 5 y 15 segundos
                bool requiereDiagnostico = rnd.Next(0, 2) == 1; // Valor aleatorio para requiere diagnostico
                llegadaCounter++;

                Paciente paciente = new Paciente(idPaciente, tiempoGlobal, tiempoConsulta, llegadaCounter, requiereDiagnostico);
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

            // Mostrar llegada del paciente
            Console.WriteLine($"Paciente {paciente.Id}. Llegado el {paciente.OrdenLlegada}. Estado: {paciente.Estado}. Duración Espera: 0 segundos.");

            // Esperar hasta que haya un médico disponible
            sem.Wait();

            lock (locker)
            {
                do
                {
                    medico = rnd.Next(1, 5);  // Asigna un médico disponible
                } while (medicosOcupados.Contains(medico));
                medicosOcupados.Add(medico);
                paciente.Estado = "Consulta";  // Cambia el estado a consulta
            }

            // Mostrar estado de consulta
            Console.WriteLine($"Paciente {paciente.Id}. Llegado el {paciente.OrdenLlegada}. Estado: {paciente.Estado}. Duración Consulta: {paciente.TiempoConsulta} segundos.");
            Thread.Sleep(paciente.TiempoConsulta * 1000);  // Simula la consulta

            // Finaliza consulta
            lock (locker)
            {
                paciente.Estado = "EsperaDiagnostico"; // Cambia el estado a espera de diagnóstico si lo necesita
                medicosOcupados.Remove(medico);
                Console.WriteLine($"Paciente {paciente.Id}. Llegado el {paciente.OrdenLlegada}. Estado: {paciente.Estado}. Duración Consulta: {paciente.TiempoConsulta} segundos.");
            }

            sem.Release();

            // Si el paciente necesita diagnóstico, asignar máquina
            if (paciente.RequiereDiagnostico)
            {
                maquinasDiagnostico.Wait(); // Espera hasta que haya una máquina disponible

                paciente.Estado = "Diagnostico";  // Cambia el estado a diagnóstico
                Console.WriteLine($"Paciente {paciente.Id}. Llegado el {paciente.OrdenLlegada}. Estado: {paciente.Estado}. Duración Diagnóstico: 15 segundos.");

                Thread.Sleep(15 * 1000);  // Simula el tiempo de diagnóstico

                paciente.Estado = "Finalizado";  // Cambia el estado a finalizado
                Console.WriteLine($"Paciente {paciente.Id}. Llegado el {paciente.OrdenLlegada}. Estado: {paciente.Estado}. Duración Diagnóstico: 15 segundos.");

                maquinasDiagnostico.Release(); // Libera la máquina de diagnóstico
            }
            else
            {
                paciente.Estado = "Finalizado";  // Si no requiere diagnóstico, finaliza directamente
                Console.WriteLine($"Paciente {paciente.Id}. Llegado el {paciente.OrdenLlegada}. Estado: {paciente.Estado}. Duración Consulta: {paciente.TiempoConsulta} segundos.");
            }
        }
    }
}