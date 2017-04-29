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
        public System.Windows.Visibility ReblogVisibility { get; private set; }
        public string ReblogBy { get; private set; }
        public System.Windows.Visibility MediaVisibility { get; private set; }
        public IEnumerable<MediaViewModel> MediaAttachments { get; private set; }

        public StatusViewModel(Status status)
        {
            CreatedAt =
                Observable.Interval(TimeSpan.FromSeconds(0.1))
                    .Select(x => FormatDate(status.CreatedAt))
                    .ToReactiveProperty();

            OpenCommand = new ReactiveCommand();
            OpenCommand.Subscribe(() => System.Diagnostics.Process.Start(status.Url));

            if (status.Reblog != null)
            {
                ReblogVisibility = System.Windows.Visibility.Visible;
                ReblogBy = status.Account.DisplayName;
                status = status.Reblog;
            }
            else
            {
                ReblogVisibility = System.Windows.Visibility.Collapsed;
            }

            AvatarUrl = status.Account.AvatarUrl;
            DisplayName = status.Account.DisplayName;
            AccountName = status.Account.AccountName;
            Content = Regex.Replace(status.Content, "<.*?>", "");

            int mediaCount = status.MediaAttachments.Count();
            if (mediaCount > 0)
            {
                MediaVisibility = System.Windows.Visibility.Visible;
            }
            else
            {
                MediaVisibility = System.Windows.Visibility.Collapsed;
            }

            MediaAttachments = status.MediaAttachments.Select((x, i) => new MediaViewModel(x, i, mediaCount));
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
