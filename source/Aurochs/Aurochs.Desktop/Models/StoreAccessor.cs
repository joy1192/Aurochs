using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aurochs.Desktop.Models
{
    public class StoreAccessor
    {
        public static StoreAccessor Default { get; } = new StoreAccessor();

        public IEnumerable<IFluxStore> Stores
        {
            get
            {
                return _Stores ?? (_Stores = _Container.GetExportedValues<IFluxStore>());
            }
        }
        private IEnumerable<IFluxStore> _Stores;

        private CompositionContainer _Container;

        private StoreAccessor()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly()); 
            this._Container = new CompositionContainer(catalog);
        }

        public T Get<T>()
        {
            try
            {
                var store = Stores.OfType<T>().SingleOrDefault();
                if (store == null)
                    throw new InvalidOperationException($"{typeof(T).FullName} was not registerd.");

                return store;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"{typeof(T).FullName} was duplecated.", e);
            }
        }
    }
}
