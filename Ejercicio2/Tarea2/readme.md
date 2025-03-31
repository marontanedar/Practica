## 🏥 Simulación de Hospital con Hilos en C#
Este programa simula la atención de pacientes en un hospital, donde los pacientes deben ser atendidos en el orden en que llegaron. Cada paciente pasa por una consulta médica y, si es necesario, por un proceso de diagnóstico. Se garantiza que el orden de llegada de los pacientes se respeta tanto en la consulta como en las pruebas diagnósticas.

![image](https://github.com/user-attachments/assets/ce1f6c2f-e9c9-4e49-934e-d968977fd634)

## 📌 Características
Los pacientes llegan al hospital en orden secuencial.

Solo pueden pasar a la consulta si hay un médico disponible.

Después de la consulta, si requieren pruebas diagnósticas, deben realizarlas en el mismo orden en el que llegaron.

Se usa sincronización para garantizar el orden correcto de atención.
## 📌 Requisitos
🔹 .NET Framework o .NET Core instalado 🔹 Compilador de C# (Visual Studio, VS Code o dotnet CLI)

## ▶️ ¿Porque escogiste esté código? 
Se emplea BlockingCollection<Paciente> para gestionar los pacientes en la cola de diagnóstico de manera segura entre hilos.

Add() permite que los pacientes agreguen su solicitud de diagnóstico en la cola.

Take() permite que los pacientes sean procesados en el orden correcto, asegurando que el siguiente paciente en la fila sea atendido antes que los demás.

Sincronización con SemaphoreSlim

Un semáforo controla el acceso a los médicos (sem = new SemaphoreSlim(4)).

Otro semáforo (maquinasDiagnostico = new SemaphoreSlim(2)) limita el acceso a las máquinas de diagnóstico.

Hilos para concurrencia

Cada paciente es manejado en un hilo separado.

La consulta y el diagnóstico se ejecutan en paralelo, pero el diagnóstico respeta estrictamente el orden de llegada.

Control del orden de diagnóstico

Se usa BlockingCollection<Paciente> para garantizar que los pacientes hagan las pruebas en el orden correcto sin bloqueos manuales.

Take() se usa para extraer pacientes en el orden en que llegaron.

¿Por qué esta solución?
Evita bloqueos innecesarios: Take() suspende el hilo hasta que haya un paciente disponible, evitando esperas activas y mejorando el rendimiento.
