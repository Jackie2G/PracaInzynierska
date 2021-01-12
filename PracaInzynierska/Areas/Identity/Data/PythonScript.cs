using IronPython.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PracaInzynierska.Areas.Identity.Data
{
    public class PythonScript
    { 

        public string RunFromFile(string weights, string dates, string expectedDate)
        {
            var psi = new ProcessStartInfo(); 
            psi.FileName = @"D:\home\python364x64\python.exe";

            string dummyPythonAppDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content");
            var script = Path.Combine(dummyPythonAppDirectory, "predictTotal.py");

            //var script = @"D:\home\python364x64\test.py";

            psi.Arguments = $"\"{script}\" --dates \"{dates}\" --volume \"{weights}\" --expecteddate \"{expectedDate}\"";
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }


            return (results);
        }
    }
}

