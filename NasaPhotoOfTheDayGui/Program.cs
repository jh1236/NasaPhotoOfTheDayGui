/*
 * Progam.cs
 * 
 * Jared Healy, All rights reserved (c) 2022
 * 
 * Entry Point to the Nasa APOD program.
 * 
 * 10/02/2022; Commented through all code
 *             Minimised the window to tray
 * 
 */



namespace Nasa
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var f = new NasaApodGui();
            f.Hide();
            f.Opacity = 0;
            f.ShowInTaskbar = false;
            Application.Run(f);
        }
    }
}