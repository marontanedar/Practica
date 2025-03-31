
## 🏥 Pacientes en un Hospital

Este programa en C# simula la atención de pacientes en un hospital con un sistema de concurrencia usando hilos y semáforos. Cada paciente tiene un identificador único, un tiempo de llegada y un tiempo de consulta asignado aleatoriamente.
![image](https://github.com/user-attachments/assets/d7b2e0fa-5be4-48e0-b8fd-4e26c8ac0a5d)



## 📌 Características

Uso de hilos para simular la concurrencia de pacientes en el hospital.

Semáforo para limitar la cantidad de médicos disponibles (4 médicos simultáneos).

Asignación de tiempos aleatorios para la llegada y la duración de la consulta.

Control del estado del paciente: espera, consulta y finalizado.

Uso de listas y bloqueos para manejar los médicos ocupados y la gestión de pacientes.


## 📌 Requisitos
🔹 .NET Framework o .NET Core instalado
🔹 Compilador de C# (Visual Studio, VS Code o dotnet CLI)

## ▶️ ¿Cuál de los pacientes sale primero de consulta? 
En mi ejemplo es el paciente 98 pero puede variar porque el tiempo de consulta es aleatorio entre 5 y 15 segundos.
