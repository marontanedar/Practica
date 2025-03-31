# README

> [!NOTE]
> Esto es información adicional que puede ayudar al usuario

> [!TIP]
> Un consejo útil o sugerencia

> [!IMPORTANT]
> Información crucial

> [!WARNING]
> Necesita la atención del usuario

> [!CAUTION]
> Consecuencias negativas

## 🏥 Simulación de Hospital con Hilos en C#
Este programa simula un hospital donde los pacientes son atendidos por médicos de manera concurrente utilizando hilos (Threads). Se emplea un semáforo (SemaphoreSlim) para controlar el acceso a los médicos disponibles y garantizar que no haya más de 4 pacientes en consulta simultáneamente.


## 📌 Características
Manejo de concurrencia: Uso de Thread para simular múltiples pacientes llegando al hospital.

Control de recursos: Uso de SemaphoreSlim para restringir la cantidad de pacientes en consulta a un máximo de 4.

Asignación de médicos sin repetición: Asegura que cada médico atienda a un solo paciente a la vez.

Sincronización con lock: Protege la asignación y liberación de médicos con un lock para evitar condiciones de carrera.


## 🔧 Cómo funciona el código
Inicio del programa

Se crean 4 hilos que representan pacientes llegando al hospital cada 2 segundos.

Asignación de médicos

Se genera un número aleatorio entre 1 y 4 para asignar un médico.

Si el médico ya está ocupado, se busca otro disponible.

Atención médica

El paciente espera en la sala hasta que haya un médico libre (sem.Wait()).

Una vez asignado, el paciente es atendido durante 10 segundos (Thread.Sleep(10000)).

Liberación del médico

Tras la consulta, el médico es liberado y otro paciente puede ser atendido.

## 📜 Código principal

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
        static List<int> medicosOcupados = new List<int>(); // Lista de médicos asignados

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

            Console.WriteLine($"Paciente {pacienteId} en sala de espera");
            sem.Wait();

            Console.WriteLine($"  Paciente {pacienteId} en consulta con medico Nº {medico}");
            Thread.Sleep(10000);
            Console.WriteLine($"    Paciente {pacienteId} finaliza con medico Nº {medico}");

            // Liberar médico
            lock (locker)
            {
                medicosOcupados.Remove(medico);
            }

            sem.Release();
        }
    }
}

## 📌 Requisitos
🔹 .NET Framework o .NET Core instalado
🔹 Compilador de C# (Visual Studio, VS Code o dotnet CLI)

## ▶️ Cómo ejecutar
Guarda el código en un archivo Program.cs.

Compila el programa:

sh
Copiar
Editar
csc Program.cs
Ejecuta el programa:

sh
Copiar
Editar
./Program.exe

## 📌 Mejoras futuras
✅ Manejar más pacientes y médicos dinámicamente.
✅ Implementar cierre del hospital cuando todos los pacientes sean atendidos.
✅ Usar Queue para gestionar pacientes en espera.


