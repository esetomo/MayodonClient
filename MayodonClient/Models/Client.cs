using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Mastonet;
using Mastonet.Entities;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace MayodonClient.Models
{
    public class Client
    {
        public AppRegistration App { get; set; }
        public Auth Auth { get; set; }
        public Account Account { get; set; }

        private Timeline publicTimeline;
        public Timeline PublicTimeline
        {
            get
            {
                if(publicTimeline == null)
                {
                    publicTimeline = new Timeline(Mastodon.GetPublicTimeline(), Mastodon.GetPublicStreaming());
                }
                return publicTimeline;
            }
        }

        private MastodonClient mastodon;
        private MastodonClient Mastodon
        {
            get
            {
                if(mastodon == null)
                    mastodon = new MastodonClient(App, Auth);
                return mastodon;
            }
        }
    }
}
