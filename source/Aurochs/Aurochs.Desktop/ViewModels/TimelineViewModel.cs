using Aurochs.Desktop.Events;
using Aurochs.Desktop.ViewModels.Contents;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.ViewModels
{
    public class TimelineViewModel : BindableBase
    {
        public ObservableCollection<StatusViewModel> StatusCollection
        {
            get { return _StatusCollection; }
            set { SetProperty(ref _StatusCollection, value); }
        }
        private ObservableCollection<StatusViewModel> _StatusCollection = new ObservableCollection<StatusViewModel>();

        public TimelineViewModel()
        {
            //var store = StoreAccessor.Default.Get<TimelineStore>();
            //var observer = Observable.FromEventPattern<ApplicationLocalEventArgs>
            //(
            //    handler => store.StoreContentChanged += handler,
            //    handler => store.StoreContentChanged -= handler
            //).
            //Select(x => x.Sender).
            //OfType<TimelineStore>().
            //Subscribe(s =>
            //{
            //    DispatcherHelper.CurrentDispatcher.Invoke(() =>
            //    {
            //        try
            //        {
            //            while (this.StatusCollection.Count < s.Tweets.Count)
            //            {
            //                this.StatusCollection.Add(new StatusViewModel());
            //            }
            //            while (this.StatusCollection.Count > s.Tweets.Count)
            //            {
            //                this.StatusCollection.RemoveAt(0);
            //            }
            //            for (int i = 0; i < this.StatusCollection.Count; i++)
            //            {
            //                this.StatusCollection[i].Update(s.Tweets[i]);
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            Trace.TraceError(e.ToString());
            //        }
            //    });
            //});
        }

        public void Initalize()
        {
            #region 仮処理
            var userId = long.Parse(Environment.GetEnvironmentVariable("TWEETAREAS_USERID", EnvironmentVariableTarget.User));

            var mock = new Mock<IRepository<AuthedUser>>();
            mock.Setup(m => m.Query(userId)).Returns(new AuthedUser()
            {
                AccessToken = Environment.GetEnvironmentVariable("TWEETAREAS_ACCESS_TOKEN", EnvironmentVariableTarget.User),
                AccessTokenSecret = Environment.GetEnvironmentVariable("TWEETAREAS_ACCESS_TOKEN_SECRET", EnvironmentVariableTarget.User),
                UserId = userId
            });

            var ac = new FirehoseActionCreator(userId)
            {
                Repository = mock.Object
            };

            ac.Initialize();
            #endregion
        }
    }
}
