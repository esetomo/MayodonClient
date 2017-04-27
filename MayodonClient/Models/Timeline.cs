using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayodonClient.Models
{
    public class Timeline : IObservable<Status>
    {
        private readonly List<IObserver<Status>> observers = new List<IObserver<Status>>();

        public Timeline(Task<IEnumerable<Status>> past, TimelineStreaming streaming)
        {
            Start(past, streaming);
        }

        class Disposer : IDisposable
        {
            private readonly List<IObserver<Status>> observers;
            private readonly IObserver<Status> observer;

            public Disposer(List<IObserver<Status>> observers, IObserver<Status> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                observers.Remove(observer);
            }
        }

        public IDisposable Subscribe(IObserver<Status> observer)
        {
            observers.Add(observer);
            return new Disposer(observers, observer);
        }

        private void OnNext(Status status)
        {
            foreach (var observer in observers)
                observer.OnNext(status);
        }

        private async void Start(Task<IEnumerable<Status>> past, TimelineStreaming streaming)
        {
            foreach(var status in await past)
            {
                OnNext(status);
            }

            streaming.OnUpdate += (_, e) => OnNext(e.Status);
            await streaming.Start();
        }
    }
}
