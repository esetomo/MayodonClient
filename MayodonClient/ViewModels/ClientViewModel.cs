using Mastonet.Entities;
using MayodonClient.Models;
using Reactive.Bindings;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayodonClient.ViewModels
{
    public class ClientViewModel
    {
        public ReadOnlyReactiveCollection<Status> PublicTimeline { get; private set; }

        public ClientViewModel(Client client)
        {
            PublicTimeline = client.PublicTimeline.ToReadOnlyReactiveCollection();
        }
    }
}
