using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace FixedMountScanner
{
    public class FixedMountScannerCollection : IEnumerable<FixedMountScannerBase>, IList<FixedMountScannerBase>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public event EventHandler<ConnectionChangedArgs> CollectionConnectionChanged;
        protected List<FixedMountScannerBase> _items;
        public bool IsAllFixedScannersConnected
        {
            get
            {
                if (_items.Count <= 0)
                    return false;

                return _items.All(x => x.IsConnected);
            }
        }
        public bool IsAnyFixedScannersConnected
        {
            get
            {
                if (_items.Count <= 0)
                    return false;

                return _items.Any(x => x.IsConnected);
            }
        }
        public int Count => _items.Count;
        public bool IsReadOnly => false;
        public FixedMountScannerBase this[int index] { get => _items[index]; set => _items[index] = value; }
        public FixedMountScannerBase this[string name]
        {
            get { return this.FirstOrDefault(x => x.Name == name); }
            set
            {
                var scanner = _items.FirstOrDefault(x => x.Name == name);
                if (scanner != null)
                {
                    var index = _items.IndexOf(scanner);
                    _items[index] = value;
                }
            }
        }
        public FixedMountScannerCollection() 
        {
            _items = new List<FixedMountScannerBase>();
        }
        private void Dataman_ConnectionChanged(object sender, ConnectionChangedArgs e)
        {
            log.InfoFormat("ConnectionChanged {0}", ((Dataman)sender).Name);
            CollectionConnectionChanged?.Invoke(sender, e);
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                item.Dispose();
                item.ConnectionChanged -= Dataman_ConnectionChanged;
            }
            _items.Clear();
        }
        public IEnumerator<FixedMountScannerBase> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public int IndexOf(FixedMountScannerBase item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, FixedMountScannerBase item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if (_items.Count > index)
            {
                Remove(_items[index]);
            }
        }

        public void Add(FixedMountScannerBase item)
        {
            if (_items.Any(x => x.Name.Equals(item.Name)) == false)
            {
                try
                {
					log.InfoFormat("Added {0}", item.Name);
					_items.Add(item);
					item.ConnectionChanged += Dataman_ConnectionChanged;
                    item.Connect();
                }
                catch (Exception) { }
            }
        }

        public bool Contains(FixedMountScannerBase item)
        {
            return _items.Contains(item);
        }
        public bool Contains(int i)
        {
            return _items.Count > i;
        }
        public bool Contains(string name)
        {
            return _items.Any(q => q.Name == name);
        }

        public void CopyTo(FixedMountScannerBase[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(FixedMountScannerBase item)
        {
            if (item != null)
            {
                log.InfoFormat("Removed {0}", item.Name);
                item.Dispose();
                item.ConnectionChanged -= Dataman_ConnectionChanged;
                _items.Remove(item);
                return true;
            }
            return false;
        }
        public void ForEach(Action<FixedMountScannerBase> action)
        {
            _items.ForEach(action);
        }
        public FixedMountScannerBase Find(Predicate<FixedMountScannerBase> action)
        {
            return _items.Find(action);
        }
    }
}
