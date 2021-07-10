using System.Windows;
using System;
using System.Threading.Tasks;

public class WPFGlobalExceptionHandler
{
    public void GlobalExceptionHandler()
    {
        AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
        Application.Current.Dispatcher.UnhandledExceptionFilter += OnFilterDispatcherException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    /// <summary>
    /// This methods gets invoked for every unhandled exception
    /// that is raise on the application Dispatcher, the AppDomain
    /// or by the GC cleaning up a faulted Task.
    /// </summary>
    /// <param name="e">The unhandled exception</param>
    public void OnUnhandledException(Exception ex)
    {

        _logger.Log($"OnUnhandledException on application dispatcher: {ex.Message}");
    }

    /// <summary>
    /// Override this method to decide if the <see cref="OnUnhandledException(Exception)"/>
    /// method should be called for the passes Dispatcher exception.
    /// </summary>
    /// <param name="exception">The unhandled exception on the applications dispatcher.</param>
    /// <returns>True if the <see cref="OnUnhandledException(Exception)"/> method should
    /// be called. False if not</returns>
    protected virtual void CatchDispatcherException(Exception exception)
    {
        _logger.Log($"CatchDispatcherException on application dispatcher: {exception.Message}");
    }




    /// <summary>
    /// This method is invoked whenever there is an unhandled
    /// exception on a delegate that was posted to be executed
    /// on the UI-thread (Dispatcher) of a WPF application.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        _logger.Log($"Unhandled exception on application dispatcher: {e.Exception.Message}");
        OnUnhandledException(e.Exception);
        e.Handled = true;
    }

    /// <summary>
    /// This event is invoked whenever there is an unhandled
    /// exception in the default AppDomain. It is invoked for
    /// exceptions on any thread that was created on the AppDomain. 
    /// </summary>
    private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        _logger.Log($"Unhandled exception on current AppDomain  {e.IsTerminating}");
    }


    /// <summary>
    /// This method is called when a faulted task, which has the
    /// exception object set, gets collected by the GC. This is useful
    /// to track Exceptions in asnync methods where the caller forgets
    /// to await the returning task
    /// </summary>
    private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        _logger.Log($"Unobserved task exception: {e.Exception}");
        OnUnhandledException(e.Exception);
        e.SetObserved();
    }

    /// <summary>
    /// The method gets called for any unhandled exception on the
    /// Dispatcher. When e.RequestCatch is set to true, the exception
    /// is catched by the Dispatcher and the DispatcherUnhandledException
    /// event will be invoked.
    /// </summary>
    private void OnFilterDispatcherException(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
    {
        _logger.Log(e.Exception.Message);
    }

    private readonly ILogger _logger;
}