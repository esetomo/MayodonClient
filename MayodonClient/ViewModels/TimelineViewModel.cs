using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayodonClient.ViewModels
{
    public class TimelineViewModel : ObservableCollection<StatusViewModel>, IObserver<Status>, IDisposable
    {
        private readonly IDisposable disposer;
        private readonly int limit;

        public TimelineViewModel(IObservable<Status> input, int limit)
        {
            disposer = input.Subscribe(this);
            this.limit = limit;
        }

        public void Dispose()
        {
            disposer.Dispose();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(Status value)
        {
            Insert(0, new StatusViewModel(value));

            while (Count > limit)
                RemoveAt(Count - 1);
        }
    }
}
