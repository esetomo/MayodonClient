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
        private readonly int remove;

        public TimelineViewModel(IObservable<Status> input, int limit, int remove)
        {
            disposer = input.Subscribe(this);
            this.limit = limit;
            this.remove = remove;
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
            Add(new StatusViewModel(value));

            if(Count > limit)
                Enumerable.Range(1, remove).ToList().ForEach((i) => RemoveAt(0));
        }
    }
}
