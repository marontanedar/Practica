
## 🏥 Simulación de Hospital con Hilos en C#
Este programa simula un hospital donde los pacientes son atendidos por médicos de manera concurrente utilizando hilos (Threads). Se emplea un semáforo (SemaphoreSlim) para controlar el acceso a los médicos disponibles y garantizar que no haya más de 4 pacientes en consulta simultáneamente.

![image](https://github.com/user-attachments/assets/bc5a0a17-8444-44b4-a7d4-92f1e8428231)

## 📌 Características
Manejo de concurrencia: Uso de Thread para simular múltiples pacientes llegando al hospital.

Control de recursos: Uso de SemaphoreSlim para restringir la cantidad de pacientes en consulta a un máximo de 4.

Asignación de médicos sin repetición: Asegura que cada médico atienda a un solo paciente a la vez.

Sincronización con lock: Protege la asignación y liberación de médicos con un lock para evitar condiciones de carrera.

## 📌 Requisitos
🔹 .NET Framework o .NET Core instalado
🔹 Compilador de C# (Visual Studio, VS Code o dotnet CLI)

## ▶️ ¿Cuántos hilos se están ejecutando en este programa?   
En este programa se ejecutan 5 hilos en total:

El hilo principal, que se encarga de crear y lanzar los hilos de los pacientes.

4 hilos de pacientes, ya que se crean 4 hilos y cada uno simula la llegada de un paciente.

Cada hilo de paciente se ejecuta de forma concurrente y espera a que un médico esté disponible para la consulta.

## ▶️ ¿Cuál de los pacientes entra primero en consulta?
El primer paciente que entra en consulta es el primero que obtiene un médico disponible.

Dado que cada paciente llega cada 2 segundos, se forman en la sala de espera. Sin embargo, el orden de entrada en consulta depende del médico asignado aleatoriamente. Si el médico que un paciente necesita está ocupado, el paciente debe esperar hasta que quede libre.

En la mayoría de los casos, el primer paciente que llega es el primero en ser atendido, pero en escenarios de concurrencia, si un paciente obtiene un médico más rápido que otro, podría adelantarse.

## ▶️ ¿Cuál de los pacientes sale primero de consulta?
El paciente que sale primero de consulta es el primer paciente que fue atendido por un médico.

Dado que el tiempo de consulta es fijo (10 segundos), el primer paciente en entrar a consulta será el primero en salir, suponiendo que no hubo retrasos en la asignación de médicos.

Si todos los médicos comienzan a atender pacientes al mismo tiempo, entonces el orden de salida será el mismo que el orden de entrada en consulta.
