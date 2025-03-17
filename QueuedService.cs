using CodeMechanic.Async;
using CodeMechanic.Shargs;

namespace Sharpify.CLI;

public class QueuedService
{
    private List<Func<Task>> steps = new();
    protected readonly ArgsMap arguments;
    private bool debug = false;

    public QueuedService(ArgsMap arguments)
    {
        this.arguments = arguments;
        debug = arguments.HasFlag("--debug");
    }

    public virtual async Task Run()
    {
        var Q = new SerialQueue();
        var tasks = steps.Select(step => Q.Enqueue(step));

        await Task.WhenAll(tasks);

        if (debug)
            Console.WriteLine("DONE. All tasks in Q have completed.");
        steps.Clear();
    }
}