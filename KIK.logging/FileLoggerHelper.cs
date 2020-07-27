using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace KIK.logging
{
    internal class FileLoggerHelper
    {
        private string fileName;
        public FileLoggerHelper(string filename)
        {
            this.fileName = filename;
        }

        
       

        // Blokada dostepu do logu
        static ReaderWriterLock locker = new ReaderWriterLock();
        internal void InsertLog(LogEntry logEntry)
        {
            var directory = Path.GetDirectoryName(fileName);
            if(!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            try
            {
                // po czasie
                locker.AcquireWriterLock(int.MaxValue);
                var help = Environment.NewLine.ToArray();
                var iestring = ($"{logEntry.CreatedTime} {logEntry.EventId} {logEntry.LogLevel} {logEntry.Message}").ToArray();

                string[] IEStr = new string[iestring.Length+help.Length];
                
                // tworzenie jednej spojnej tablicy
                for(int i =0; i<iestring.Length+help.Length; i++)
                {
                    if (i < iestring.Length) IEStr[i] = iestring[i].ToString();

                    if (i > iestring.Length && i < iestring.Length+help.Length) IEStr[i] = help[i].ToString();

                }
                File.AppendAllLines(fileName, IEStr); 

                
            }
            finally
            {
                // dostosowuje liczbe blokad 
                locker.ReleaseWriterLock();
            }

        }


    }
}
