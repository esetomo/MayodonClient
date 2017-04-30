using MayodonClient.Models;
using MayodonClient.Views;
using Reactive.Bindings;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace MayodonClient.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly Config config;

        public ReadOnlyReactiveCollection<ClientViewModel> Clients { get; private set; }
        public TimelineViewModel Timeline {get; private set;}
        public AccountPanelViewModel AccountPanel { get; private set; }

        public MainWindowViewModel(Config config, AccountPanelViewModel accountPanel)
        {
            this.config = config;

            AccountPanel = accountPanel;

            Clients = config.Clients.ToObservable().Select(x => new ClientViewModel(x)).ToReadOnlyReactiveCollection();
            var timeline = config
                .Clients
                .ToObservable()
                .SelectMany(client => client.PublicTimeline)
                .Distinct(x => x.Uri);
            Timeline = new TimelineViewModel(timeline, 200, 100);
        }
    }
}
