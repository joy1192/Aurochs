using Aurochs.Core.Entities;
using Aurochs.Core.Extensions;
using Aurochs.Desktop.ActionMessages;
using Aurochs.Desktop.Events;
using Infrastructure.Flux.Core;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Models
{
    [Export(typeof(IFluxStore))]
    public class UserTimelineStore : MultipleDisposable, IFluxStore
    {
        private Dictionary<string, DispatcherToken> Tokens { get; } = new Dictionary<string, DispatcherToken>();

        public List<Status> StatusCollection { get; private set; } = new List<Status>();

        private const int TimelineMaxSize = 200;
        
        [ImportingConstructor]
        public UserTimelineStore()
        {
            this.Tokens[nameof(OnUserTimelineInitialize)] = Dispatcher.Default.Register<UserTimelineInitializeMessage>(OnUserTimelineInitialize);
            this.Tokens[nameof(OnUpdateStatus)] = Dispatcher.Default.Register<UpdateStatusActionMessage>(OnUpdateStatus);
            this.Tokens[nameof(OnDeleteStatus)] = Dispatcher.Default.Register<DeleteStatusActionMessage>(OnDeleteStatus);

            this.Add(() =>
            {
                foreach (var token in Tokens.Values)
                {
                    Dispatcher.Default.Release(token);
                }
            });
        }

        private void OnUpdateStatus(UpdateStatusActionMessage msg)
        {
            if (msg.Source != StatusSource.User)
                return;
            
            this.StatusCollection.Insert(0, msg.Status);

            var adds = new Queue<Status>();
            adds.Enqueue(msg.Status);

            StoreContentChanged?.Invoke(this, new TimelineContentUpdatedEventArgs() { Adds = adds });
        }

        private void OnDeleteStatus(DeleteStatusActionMessage msg)
        {
            if (msg.Source != StatusSource.User)
                return;

            this.StatusCollection.RemoveAll(x => x.Id == msg.StatusId);

            var removes = new Queue<long>();
            removes.Enqueue(msg.StatusId);

            StoreContentChanged?.Invoke(this, new TimelineContentUpdatedEventArgs() { Removes = removes });
        }

        private void OnUserTimelineInitialize(TimelineInitializeMessage msg)
        {
            this.StatusCollection = msg.TweetContent.Distinct().OrderByDescending(x => x.CreatedAt).Take(TimelineMaxSize).ToList();

            StoreContentChanged?.Invoke(this, TimelineContentUpdatedEventArgs.Default);
        }

        public event EventHandler<ApplicationLocalEventArgs> StoreContentChanged;
    }
}
