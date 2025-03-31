## 🏥 Simulación de Hospital 
Este proyecto simula la llegada de pacientes a un hospital, su asignación a médicos disponibles y el tiempo que pasan en consulta. Cada paciente tiene un identificador único, un tiempo de llegada, un tiempo de consulta y un estado que cambia durante el proceso.

## 📌 Características
Asignación de pacientes: Cada paciente recibe un ID aleatorio y es atendido según el orden de llegada.

Médicos disponibles: Hay 4 médicos que pueden atender pacientes simultáneamente.

Estados del paciente:

Espera: Aún no ha entrado en consulta.

Consulta: Está siendo atendido por un médico.

Finalizado: Ha concluido la consulta.

Simulación del tiempo: Se utiliza Thread.Sleep para representar los tiempos de espera y consulta.

Control de concurrencia: Uso de SemaphoreSlim para asegurar que solo 4 pacientes sean atendidos al mismo tiempo.

## 📌 Requisitos
🔹 .NET Framework o .NET Core instalado 
🔹 Compilador de C# (Visual Studio, VS Code o dotnet CLI)

## ▶️¿Has decidido visualizar información adicional a la planteada en el ejercicio?

✅ Tiempo de espera antes de entrar a consulta

Esto permite ver cuánto tiempo ha estado un paciente en "Espera" antes de ser atendido.

Ayuda a evaluar si hay pacientes esperando más tiempo del necesario.

✅ Orden de llegada del paciente

Se muestra explícitamente para diferenciar entre el ID del paciente (único) y el orden en el que llegó al hospital.

Facilita entender cuál paciente entrará antes o después en consulta.

✅ Estado del paciente en cada fase del proceso

Se imprime cada cambio de estado ("Espera" → "Consulta" → "Finalizado") para dar un seguimiento claro.

Esto ayuda a visualizar qué pacientes están siendo atendidos y quiénes han terminado.

✅ Tiempo total de consulta

Se muestra junto con el cambio de estado a "Consulta" y "Finalizado" para que sea más evidente cuánto tiempo cada paciente permanece con el médico.


