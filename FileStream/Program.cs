using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileStreamApp
{
    class Program
    {
        static FileStream file;
        static void Main(string[] args)
        {
            IAsyncResult ar = null;
            string fromFileName = "Z:\\123.bin";
            string toFileName = "Z:\\1234.bin";
            try
            {
                file = new FileStream(fromFileName, FileMode.Open);
                Console.WriteLine("file.Length = {0}", file.Length);
                byte[] buf = new byte[file.Length];
                // Старт чтения в асинхронном режиме
                ar = file.BeginRead(buf, 0,
                  (int)file.Length, null, null);
                //EndReadCallbak, ar);
                Console.WriteLine("ID основного потока: {0}",
                  Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Старт чтения в асинхронном режиме");
                int ReadSize = file.EndRead(ar);
                Console.WriteLine("Прочтено {0} байт из файла.",
                  ReadSize);
                // Старт записи в асинхронном
                file = new FileStream(toFileName, FileMode.OpenOrCreate);
                file.BeginWrite(buf, 0, buf.Length, null, null);
                Console.WriteLine("Файл успешно записан");
                // 
                // Рабочий код 
                // 
                //ar.AsyncWaitHandle.WaitOne(); // INFINITY
                //ar.AsyncWaitHandle.WaitOne(TimeSpan.MaxValue);
                //Console.WriteLine("Операция асинхронного чтения завершена");
                /*
                 * Примерный код системного потока асинхронного чтения
                while (!ar.IsCompleted) {
                   Оперция чтения
                }
                ((ManualResetEvent)ar.AsyncWaitHandle).Set();
                EndReadCallbak(obj);
                */
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: ");
                Console.WriteLine(e.Message);
            }
            Console.Read();
            Console.Read();
        }
        static void EndReadCallbak(object obj)
        {
            IAsyncResult ar = (IAsyncResult)obj;
            int ReadSize = file.EndRead(ar);
            Console.WriteLine("Прочтено {0} байт из файла.",
              ReadSize);
            Console.WriteLine("ID асинхронного потока: {0}",
              Thread.CurrentThread.ManagedThreadId);
            // Возможен код для сигнализации о завершении 
            //  операции чтения
        }
    }
}