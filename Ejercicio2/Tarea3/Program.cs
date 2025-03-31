using System;
using System.Collections.Generic;
using System.Threading;

namespace Hospital
{
    /// <summary>
    /// Representa a un paciente en el hospital.
    /// </summary>
    public class Paciente
    {
        public int Id { get; set; }  // Identificador único del paciente
        public int LlegadaHospital { get; set; }  // Tiempo de llegada al hospital
        public int TiempoConsulta { get; set; }  // Duración de la consulta con el médico
        public string Estado { get; set; }  // Estado actual del paciente
        public int OrdenLlegada { get; set; }  // Orden de llegada del paciente
        public bool RequiereDiagnostico { get; set; }  // Indica si el paciente necesita un diagnóstico adicional

        public Paciente(int id, int llegadaHospital, int tiempoConsulta, int ordenLlegada, bool requiereDiagnostico)
        {
            Id = id;
            LlegadaHospital = llegadaHospital;
            TiempoConsulta = tiempoConsulta;
            Estado = "EsperaConsulta"; // Estado inicial del paciente
            OrdenLlegada = ordenLlegada;
            RequiereDiagnostico = requiereDiagnostico;
        }
    }

    internal class Program
    {
        static SemaphoreSlim sem = new SemaphoreSlim(4); // 4 médicos disponibles
        static SemaphoreSlim maquinasDiagnostico = new SemaphoreSlim(2); // 2 máquinas de diagnóstico disponibles
        static readonly object locker = new object(); // Bloqueo para sincronización de hilos
        static Random rnd = new Random(); // Generador de números aleatorios
        static Queue<Paciente> colaPruebas = new Queue<Paciente>(); // Cola que asegura el orden de pruebas según el orden de llegada
        static List<int> medicosOcupados = new List<int>(); // Lista de médicos ocupados
        static int tiempoGlobal = 0; // Tiempo de simulación en segundos
        static int llegadaCounter = 0; // Contador de orden de llegada de pacientes

        private static void Main(string[] args)
        {
            List<Paciente> pacientes = new List<Paciente>(); // Lista de pacientes

            for (int i = 1; i <= 4; i++)
            {
                int idPaciente = i;
                int tiempoConsulta = rnd.Next(5, 16); // Tiempo de consulta aleatorio entre 5 y 15 segundos
                bool requiereDiagnostico = rnd.Next(0, 2) == 1; // Determina si el paciente requiere diagnóstico
                llegadaCounter++;

                Paciente paciente = new Paciente(idPaciente, tiempoGlobal, tiempoConsulta, llegadaCounter, requiereDiagnostico);
                pacientes.Add(paciente);

                Thread pacienteThread = new Thread(PacienteAtencion);
                pacienteThread.Start(paciente);
                Thread.Sleep(2000); // Simula la llegada de pacientes cada 2 segundos
                tiempoGlobal += 2;
            }
        }

        /// <summary>
        /// Método que gestiona la atención del paciente en consulta y diagnóstico.
        /// </summary>
        /// <param name="obj">Objeto Paciente</param>
        static void PacienteAtencion(object obj)
        {
            Paciente paciente = (Paciente)obj;
            int medico;

            // Muestra la llegada del paciente
            Console.WriteLine($"Paciente {paciente.Id}. Llegado el {paciente.OrdenLlegada}. Estado: {paciente.Estado}. Duración Espera: 0 segundos.");

            // Espera hasta que haya un médico disponible
            sem.Wait();

            lock (locker)
            {
                // Selecciona aleatoriamente un médico que esté disponible
                do
                {
                    medico = rnd.Next(1, 5); // Asigna un médico disponible aleatoriamente (de 1 a 4)
                } while (medicosOcupados.Contains(medico));
                medicosOcupados.Add(medico); // Marca al médico como ocupado
                paciente.Estado = "Consulta"; // Actualiza el estado del paciente a "Consulta"
            }

            // Simula el tiempo de consulta
            Console.WriteLine($"Paciente {paciente.Id}. Estado: {paciente.Estado}. Duración Consulta: {paciente.TiempoConsulta} segundos.");
            Thread.Sleep(paciente.TiempoConsulta * 1000);

            lock (locker)
            {
                // Actualiza el estado del paciente tras la consulta
                paciente.Estado = "EsperaDiagnostico"; // Cambia el estado a "EsperaDiagnostico"
                medicosOcupados.Remove(medico); // Libera al médico
                colaPruebas.Enqueue(paciente); // Encola al paciente para realizar pruebas
                Console.WriteLine($"Paciente {paciente.Id}. Estado: {paciente.Estado}. Agregado a la cola de pruebas.");
            }

            sem.Release(); // Libera el semáforo del médico

            ProcesarPruebas(); // Intenta procesar las pruebas para los pacientes en la cola
        }

        /// <summary>
        /// Método que gestiona las pruebas para los pacientes en la cola.
        /// </summary>
        static void ProcesarPruebas()
        {
            lock (locker)
            {
                // Verifica si hay pacientes en la cola de pruebas
                if (colaPruebas.Count > 0 && ReferenceEquals(colaPruebas.Peek(), null) == false)
                {
                    Paciente pacienteActual = colaPruebas.Peek(); // Obtén el paciente al frente de la cola

                    // Verifica si el paciente está en espera de diagnóstico y hay una máquina disponible
                    if (pacienteActual.Estado == "EsperaDiagnostico" && maquinasDiagnostico.CurrentCount > 0)
                    {
                        colaPruebas.Dequeue(); // Extrae al paciente de la cola
                        maquinasDiagnostico.Wait(); // Ocupa una máquina de diagnóstico

                        pacienteActual.Estado = "Diagnostico"; // Cambia el estado a "Diagnostico"
                        Console.WriteLine($"Paciente {pacienteActual.Id}. Estado: {pacienteActual.Estado}. Realizando diagnóstico.");

                        Thread.Sleep(15 * 1000); // Simula el tiempo fijo para diagnóstico

                        pacienteActual.Estado = "Finalizado"; // Cambia el estado a "Finalizado"
                        Console.WriteLine($"Paciente {pacienteActual.Id}. Estado: {pacienteActual.Estado}. Diagnóstico completado.");

                        maquinasDiagnostico.Release(); // Libera la máquina de diagnóstico
                    }
                }
            }
        }
    }
}
