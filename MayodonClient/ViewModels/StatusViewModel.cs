using Mastonet.Entities;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MayodonClient.ViewModels
{
    public class StatusViewModel
    {
        public string AvatarUrl { get; private set; }
        public ReactiveProperty<string> CreatedAt { get; private set; }
        public string DisplayName { get; private set; }
        public string AccountName { get; private set; }
        public string Content { get; private set; }
        public ReactiveCommand OpenCommand { get; private set; }

        public StatusViewModel(Status status)
        {
            AvatarUrl = status.Account.AvatarUrl;
            DisplayName = status.Account.DisplayName;
            AccountName = status.Account.AccountName;
            Content = Regex.Replace(status.Content, "<.*?>", "");

            CreatedAt =
                Observable.Interval(TimeSpan.FromSeconds(0.1))
                    .Select(x => FormatDate(status.CreatedAt))
                    .ToReactiveProperty();

            OpenCommand = new ReactiveCommand();
            OpenCommand.Subscribe(() => System.Diagnostics.Process.Start(status.Url));
        }

        private string FormatDate(DateTime createdAt)
        {
            var diff = DateTimeOffset.Now - createdAt;

            if (diff >= TimeSpan.FromDays(1))
                return createdAt.ToString("M月d日");

            if (diff >= TimeSpan.FromHours(1))
                return string.Format("{0}時間前", Math.Floor(diff.TotalHours));

            if (diff >= TimeSpan.FromMinutes(1))
                return string.Format("{0}分前", Math.Floor(diff.TotalMinutes));

            double seconds = diff.TotalSeconds;

            if(seconds >= 10.0)
                return string.Format("{0}秒前", Math.Floor(seconds));

            if (seconds >= 0.0)
                return string.Format("{0:0.0}秒前", seconds);

            return string.Format("{0:0.0}秒後", -seconds);
        }
    }
}
