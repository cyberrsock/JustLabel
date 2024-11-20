// using System;

// namespace JustLabel.UnreachableCodeTests
// {
//     public class UnreachableCodeTests
//     {
//         // Метод с недостижимым кодом после return
//         public void UnreachableAfterReturn()
//         {
//             Console.WriteLine("This code is reachable.");
//             return;
//             Console.WriteLine("This code is unreachable."); // Эта строка недостижима
//         }

//         // Метод с недостижимым кодом после throw
//         public void UnreachableAfterThrow()
//         {
//             Console.WriteLine("This code is reachable.");
//             throw new Exception("An error occurred");
//             Console.WriteLine("This code is unreachable."); // Эта строка недостижима
//         }

//         // Метод с вечным циклом
//         public void InfiniteLoopWithUnreachableCode()
//         {
//             while (true)
//             {
//                 Console.WriteLine("This code is in an infinite loop");
//                 break; // Поставлено для выхода, но можно убрать, чтобы сделать цикл вечным
//             }
//             Console.WriteLine("This code is unreachable."); // Эта строка недостижима
//         }

//         // Метод, где условие никогда не выполняется
//         public void AlwaysFalseCondition()
//         {
//             if (false)
//             {
//                 Console.WriteLine("This code is unreachable."); // Эта строка недостижима
//             }
//             Console.WriteLine("This code is reachable.");
//         }

//         // Метод с вложенной недостижимой логикой
//         public void NestedUnreachableCode()
//         {
//             if (true)
//             {
//                 return; // Если это будет выполнено, все, что после недостижимо
//                 Console.WriteLine("This code is unreachable."); // Эта строка недостижима
//             }
//             Console.WriteLine("This code is reachable."); // Эта строка достижима
//         }
//     }
// }
