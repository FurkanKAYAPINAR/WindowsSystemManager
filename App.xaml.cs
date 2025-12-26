// ============================================================================
// Windows System Manager
// Author: FurkanKAYAPINAR
// Version: 1.5.0
// Description: Windows Task Scheduler and Services Management Application
// Copyright © 2024 FurkanKAYAPINAR - All Rights Reserved
// ============================================================================

using System;

namespace WindowsSystemManager
{
    public partial class App : System.Windows.Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show($"UI Error:\n{e.Exception.Message}\n\n{e.Exception.StackTrace}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            Console.WriteLine($"UI Error: {e.Exception}");
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            System.Windows.MessageBox.Show($"Fatal Error:\n{ex?.Message}\n\n{ex?.StackTrace}", "Fatal Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            Console.WriteLine($"Fatal Error: {ex}");
        }
    }
}
