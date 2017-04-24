using Aurochs.Core.Entities;
using Aurochs.Core.Extensions;
using Aurochs.Desktop.ActionMessages;
using Aurochs.Desktop.Events;
using Infrastructure.Flux.Core;
using Newtonsoft.Json;
using StatefulModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Models
{
    [Export(typeof(IFluxStore))]
    public class PublicTimelineStore : MultipleDisposable, IFluxStore
    {
        private Dictionary<string, DispatcherToken> Tokens { get; } = new Dictionary<string, DispatcherToken>();

        public List<Status> StatusCollection { get; private set; } = new List<Status>();

        private long UserId { get; }

        private const int TimelineMaxSize = 200;

        [ImportingConstructor]
        public PublicTimelineStore()
        {
            this.Tokens[nameof(OnPublicTimelineInitialize)] = Dispatcher.Default.Register<PublicTimelineInitializeMessage>(OnPublicTimelineInitialize);
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

        [Conditional("DEBUG")]
        private void SaveTestData(Status status)
        {
            try
            {
                var root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                var path = Path.Combine(root, "Aurochs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, "status_logs");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var filename = Path.Combine(path, $"{status.Account.AccountName}_{status.Id}.json");
                using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
                {
                    var json = JsonConvert.SerializeObject(status);
                    writer.Write(json);
                }
            }
            catch(Exception e)
            {
                Trace.TraceError($"{e}");
            }
        }

        private void OnUpdateStatus(UpdateStatusActionMessage msg)
        {
            if (msg.Source != StatusSource.Public)
                return;

            SaveTestData(msg.Status);

            this.StatusCollection.Insert(0, msg.Status);

            var adds = new Queue<Status>();
            adds.Enqueue(msg.Status);

            StoreContentChanged?.Invoke(this, new TimelineContentUpdatedEventArgs() { Adds = adds });
        }

        private void OnDeleteStatus(DeleteStatusActionMessage msg)
        {
            if (msg.Source != StatusSource.Public)
                return;

            this.StatusCollection.RemoveAll(x => x.Id == msg.StatusId);

            var removes = new Queue<long>();
            removes.Enqueue(msg.StatusId);

            StoreContentChanged?.Invoke(this, new TimelineContentUpdatedEventArgs() { Removes = removes });
        }

        private void OnPublicTimelineInitialize(PublicTimelineInitializeMessage msg)
        {
            this.StatusCollection = msg.TweetContent.Distinct((lhs, rhs) => lhs.Id == rhs.Id, x => (int)x.Id).Take(TimelineMaxSize).ToList();

            StoreContentChanged?.Invoke(this, TimelineContentUpdatedEventArgs.Default);
        }

        public event EventHandler<ApplicationLocalEventArgs> StoreContentChanged;
    }
}
