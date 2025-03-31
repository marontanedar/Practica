## 🏥 Simulación de Hospital con Hilos en C#
Este proyecto es una simulación de un hospital en C# donde se gestionan pacientes que llegan de manera secuencial y pasan por consultas médicas y diagnósticos.

![image](https://github.com/user-attachments/assets/00bd2463-bbd3-45cc-af4f-2894d7acea8a)


## 📌 Características
la concurrencia y BlockingCollection para gestionar la cola de diagnóstico.Pacientes: Cada paciente tiene un ID, tiempo de llegada, duración de consulta y si requiere o no un diagnóstico adicional.

Consultas médicas: Hay 4 médicos disponibles simultáneamente.

Diagnósticos: Hay 2 máquinas de diagnóstico para los pacientes que lo necesiten.

Llegada secuencial: Cada paciente llega cada 2 segundos.

Orden de atención: Se mantiene el orden de llegada para las pruebas diagnósticas.

Sincronización: Uso de SemaphoreSlim para controlar
## 📌 Requisitos
🔹 .NET Framework o .NET Core instalado 🔹 Compilador de C# (Visual Studio, VS Code o dotnet CLI)

## ▶️ Explicación del código

Simulación de Pacientes y Médicos:

20 pacientes llegan al hospital cada 2 segundos con tiempos de consulta aleatorios.

Los pacientes son atendidos por 4 médicos disponibles de manera concurrente.

Uso de SemaphoreSlim para la Concurrencia:

Se usa SemaphoreSlim para manejar la disponibilidad de médicos y máquinas de diagnóstico. Los médicos y las máquinas de diagnóstico son recursos limitados y se gestionan de manera sincronizada.

Cola de Diagnóstico:

Los pacientes que requieren diagnóstico se agregan a una BlockingCollection, donde son procesados en el orden en que fueron atendidos.

Estados de los Pacientes:

Los pacientes tienen diferentes estados: EsperaConsulta, Consulta, EsperaDiagnostico, y Finalizado.

Uso de Hilos Concurrentes:

Cada paciente se maneja en un hilo independiente para simular la concurrencia de la atención médica.

Elección de la Solución:
Ventajas:

La solución es simple y fácil de entender, utilizando mecanismos básicos como SemaphoreSlim y BlockingCollection para gestionar la concurrencia.

Se puede escalar fácilmente para un mayor número de pacientes, médicos o máquinas de diagnóstico.

# Por qué elegí esta solución:
Se ajusta bien a los requisitos y es fácil de entender, sin agregar complejidad innecesaria.

# ¿Los pacientes que deben esperar entran luego a la consulta por orden de llegada? 
Comportamiento de los Pacientes
En el código, los pacientes llegan al hospital y son atendidos por médicos disponibles. Para garantizar que los pacientes entren a la consulta en el orden en que llegaron, se hace lo siguiente:

Orden de Llegada: Cada paciente tiene un campo OrdenLlegada que se incrementa con cada paciente que llega al hospital. Este campo asegura que, aunque los pacientes pueden ser atendidos en diferentes momentos dependiendo de la disponibilidad de médicos, el orden de llegada se mantiene.

Manejo de Médicos con SemaphoreSlim: La variable sem es un SemaphoreSlim(4), lo que significa que solo pueden ser atendidos 4 pacientes simultáneamente, ya que hay 4 médicos disponibles. Si un paciente llega pero no hay un médico libre, se bloqueará en el SemaphoreSlim.Wait() hasta que uno de los médicos termine con su consulta y libere un espacio para atenderlo.

Sincronización y Orden de Consulta: Los pacientes son atendidos en el orden en que llegaron debido a que se usan Thread.Sleep() y las estructuras de control adecuadas para garantizar que un paciente no pase a la consulta hasta que el médico esté disponible. Así, los pacientes en espera no saltan el turno y deben esperar su turno respetando su orden de llegada.

Pruebas Realizadas
Para asegurar que los pacientes ingresen a la consulta médica en el orden de llegada, he realizado las siguientes pruebas:

Prueba de Orden de Llegada:

He simulado que 20 pacientes lleguen al hospital secuencialmente, con un retraso de 2 segundos entre cada llegada. Al observar la salida del programa, se confirma que los pacientes aparecen en el log de acuerdo con su orden de llegada (es decir, primero el paciente 1, luego el 2, y así sucesivamente).

Prueba de Disponibilidad de Médicos:

He comprobado que, aunque los pacientes lleguen en un orden, aquellos que no tienen un médico disponible deben esperar hasta que se libere uno. Esto se verifica con la línea sem.Wait(), que bloquea a los pacientes hasta que un médico esté disponible. Los pacientes se procesan de manera síncrona en cuanto un médico queda libre.

Prueba de Consulta y Diagnóstico:

Los pacientes entran a la consulta en el orden en que llegaron, como lo indica la salida del programa, que muestra la secuencia de pacientes con sus respectivos tiempos de consulta. Los pacientes solo pasan a la consulta una vez que se haya liberado un médico, y en el caso de que necesiten diagnóstico, entran a una cola de diagnóstico de manera sincronizada, lo cual también respeta el orden de llegada.

Prueba de Comprobación de Estados:

El estado de cada paciente se imprime al inicio, durante y al final de cada proceso, lo que permite verificar que el orden de los pacientes se respeta en la consulta y diagnóstico. El estado de "EsperaConsulta" solo cambia cuando el paciente entra a consulta, y no salta a la consulta fuera de orden.

Conclusión
Los pacientes en espera para consulta entran a la consulta por orden de llegada, porque el sistema está diseñado para esperar que un médico esté disponible en el orden en que los pacientes llegan.

Las pruebas realizadas han demostrado que la sincronización entre médicos, pacientes y máquinas de diagnóstico asegura que el orden de llegada sea respetado.








