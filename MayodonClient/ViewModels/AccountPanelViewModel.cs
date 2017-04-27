using Mastonet;
using Mastonet.Entities;
using MayodonClient.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayodonClient.ViewModels
{
    public class AccountPanelViewModel
    {
        public ReactiveProperty<string> Domain { get; private set; }
        public ReactiveCommand AuthCommand { get; private set; }
        public ReactiveProperty<string> AuthCode { get; private set; }
        public ReactiveCommand OKCommand { get; private set; }
        public ObservableCollection<Client> Clients { get; private set; }

        public AccountPanelViewModel(Config config)
        {
            Clients = config.Clients;

            var appRegistration = new ReactiveProperty<AppRegistration>();
            var authClient = new ReactiveProperty<AuthenticationClient>();

            Domain = new ReactiveProperty<string>();

            var canExecuteAuthCommand = Domain.Select(x => !string.IsNullOrWhiteSpace(x));
            AuthCommand = new ReactiveCommand(canExecuteAuthCommand);
            AuthCommand.Subscribe(async () =>
            {
                authClient.Value = new AuthenticationClient(Domain.Value);
                appRegistration.Value = await authClient.Value.CreateApp("Mayodon Client", Scope.Read);
                System.Diagnostics.Process.Start(authClient.Value.OAuthUrl());
            });

            AuthCode = new ReactiveProperty<string>();
            OKCommand = Observable.CombineLatest(
                appRegistration.Select(x => x != null),
                AuthCode.Select(x => !string.IsNullOrWhiteSpace(x)),
                (x, y) => x & y).ToReactiveCommand();
            OKCommand.Subscribe(async () =>
            {
                var auth = await authClient.Value.ConnectWithCode(AuthCode.Value);
                var client = new MastodonClient(appRegistration.Value, auth);
                var account = await client.GetCurrentUser();

                config.Clients.Add(new Client()
                {
                    App = appRegistration.Value,
                    Auth = auth,
                    Account = account
                });

                config.Save();

                Domain.Value = "";
                AuthCode.Value = "";
            });                
        }
    }
}
