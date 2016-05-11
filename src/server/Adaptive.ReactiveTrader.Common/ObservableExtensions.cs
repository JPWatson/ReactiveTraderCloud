using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace Adaptive.ReactiveTrader.Common
{
    public static class ObservableExtensions
    {
        public static IObservable<TSource> TakeUntilInclusive<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return Observable.Create<TSource>(
                observer => source.Subscribe(
                    item =>
                    {
                        observer.OnNext(item);
                        if (predicate(item))
                            observer.OnCompleted();
                    },
                    observer.OnError,
                    observer.OnCompleted
                    )
                );
        }
    }

    public static class ConcurrencyService
    {
        public static IScheduler CreateEventScheduler(string name, bool foreground = false)
        {
            return new EventLoopScheduler(a => new Thread(a) {Name = name, IsBackground = !foreground});
        }
    }
}