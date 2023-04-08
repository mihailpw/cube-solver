namespace CS.Core.Utils;

public sealed class Disposable : IDisposable
{
    public static readonly Disposable Empty = new(() => { });

    private readonly Action _disposeAction;

    public Disposable(Action disposeAction) => _disposeAction = disposeAction;

    public void Dispose() => _disposeAction();
}