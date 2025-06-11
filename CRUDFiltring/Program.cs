using System;
using System.Windows.Forms;
using FiltringApp; // Asegura que este using est� presente

namespace FiltringApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LogIn()); // Aseg�rate de que el nombre coincida con la clase en LogIn.cs
        }
    }
}
