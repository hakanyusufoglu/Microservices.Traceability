
using NLog;
using System.Diagnostics;

Logger logger = LogManager.GetCurrentClassLogger();
Trace.CorrelationManager.ActivityId = Guid.NewGuid();


Work1(); 
void Work1()
{

    Console.WriteLine("Work1 started.");
    logger.Debug("Work1 started.");

    Work2();
}
void Work2()
{
    Console.WriteLine("Work2 started.");
    logger.Debug("Work2 started.");
    
    Work3();
}
void Work3()
{
    Console.WriteLine("Work3 started.");
    logger.Debug("Work3 started.");
}