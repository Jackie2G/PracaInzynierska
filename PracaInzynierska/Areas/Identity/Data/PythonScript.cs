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
            psi.FileName = @"C:\Python\python.exe";

            var script = "C:\\Users\\Jacek\\source\\repos\\PracaInzynierska\\PracaInzynierska\\Areas\\Identity\\Data\\pythonScript.py";

            psi.Arguments = $"\"{script}\" \"{weights}\" \"{dates}\" \"{expectedDate}\"";
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

