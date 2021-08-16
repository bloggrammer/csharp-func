Public void RestartApplication()
{
   Process.Start(Process.GetCurrentProcess().MainModule.FileName);
   System.Windows.Application.Current.Shutdown();
}
