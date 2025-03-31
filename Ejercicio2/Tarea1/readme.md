## 🏥 Simulación de Hospital con Hilos en C#
Este proyecto implementa una simulación de pacientes en un hospital utilizando hilos en C#. Cada paciente tiene diferentes atributos y pasa por varias etapas en su atención.

image

## 📌 Características
Cada paciente tiene un ID único generado aleatoriamente.

Se registra el tiempo de llegada al hospital.

Se asigna un tiempo de consulta aleatorio entre 5 y 15 segundos.

Se maneja el estado del paciente en las siguientes etapas:

- EsperaConsulta: Ha llegado pero aún no entra en consulta.

- Consulta: Está en consulta con un médico.

- EsperaDiagnostico: Ha finalizado la consulta y requiere pruebas diagnósticas.

- Finalizado: Ha completado su atención.

Se simula que el hospital tiene 4 médicos disponibles.

Se agregó la funcionalidad de pruebas de diagnóstico:

- 2 máquinas disponibles para diagnóstico.

- Tiempo de diagnóstico fijo de 15 segundos.

Se usa SemaphoreSlim para controlar la concurrencia y evitar que más de 4 pacientes estén en consulta simultáneamente.

Se muestra información detallada en consola sobre el estado y los cambios de cada paciente.

## 📌 Requisitos
🔹 .NET Framework o .NET Core instalado 🔹 Compilador de C# (Visual Studio, VS Code o dotnet CLI)

## ▶️ ¿Los pacientes que deben esperar para hacerse las pruebas diagnostico entran luego a hacerse las pruebas por orden de llegada? 
No, en la implementación actual los pacientes que requieren pruebas de diagnóstico no están organizados explícitamente en una cola de espera basada en su orden de llegada. La asignación ocurre en el orden en que terminan la consulta y encuentran una máquina disponible.

Pruebas realizadas:

Verifiqué cuándo cada paciente cambia su estado de "Consulta" a "EsperaDiagnostico".

Observé si el tiempo de espera entre consulta y diagnóstico varía, dependiendo de la disponibilidad de las máquinas.

Agregué mensajes de depuración antes y después de la asignación de las máquinas de diagnóstico.

Esto ayudó a visualizar cuándo y en qué orden los pacientes entraban a las pruebas.

Forcé a que varios pacientes necesitaran diagnóstico.

Modifiqué el código para que todos los pacientes tuvieran requiereDiagnostico = true.

Resultados
Los pacientes solo pueden usar las máquinas si hay una disponible, lo que significa que el orden de llegada no siempre se respeta estrictamente.

En algunos casos, un paciente que terminó su consulta antes, pero esperó más tiempo por una máquina, ingresó después de otro que terminó la consulta más tarde pero encontró una máquina libre más rápido.

Para garantizar un orden de llegada estricto, habría que usar una cola de prioridad o un mecanismo de sincronización adicional.
