using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignObject
{
    public class ZoomLevelCollection : IList<int>
    {
        #region Public Constructors

        public ZoomLevelCollection()
        {
            this.List = new SortedList<int, int>();
        }

        public ZoomLevelCollection(IEnumerable<int> collection)
          : this()
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            this.AddRange(collection);
        }

        #endregion

        #region Class Properties

        public static ZoomLevelCollection Default
        {
            get
            {
                return new ZoomLevelCollection(new[] { 25, 33, 50, 67, 75, 80, 90, 100, 110, 125, 150, 175, 200 });
            }
        }

        #endregion

        #region Public Properties

        public int Count
        {
            get { return this.List.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int this[int index]
        {
            get { return this.List.Values[index]; }
            set
            {
                this.List.RemoveAt(index);
                this.Add(value);
            }
        }

        #endregion

        #region Protected Properties

        protected SortedList<int, int> List { get; set; }

        #endregion

        #region Public Members
        public void Add(int item)
        {
            this.List.Add(item, item);
        }

        public void AddRange(IEnumerable<int> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (int value in collection)
            {
                this.Add(value);
            }
        }

        public void Clear()
        {
            this.List.Clear();
        }

        public bool Contains(int item)
        {
            return this.List.ContainsKey(item);
        }
        public void CopyTo(int[] array, int arrayIndex)
        {
            for (int i = 0; i < this.Count; i++)
            {
                array[arrayIndex + i] = this.List.Values[i];
            }
        }

        public int FindNearest(int zoomLevel)
        {
            int nearestValue = this.List.Values[0];
            int nearestDifference = Math.Abs(nearestValue - zoomLevel);
            for (int i = 1; i < this.Count; i++)
            {
                int value = this.List.Values[i];
                int difference = Math.Abs(value - zoomLevel);
                if (difference < nearestDifference)
                {
                    nearestValue = value;
                    nearestDifference = difference;
                }
            }
            return nearestValue;
        }

        public IEnumerator<int> GetEnumerator()
        {
            return this.List.Values.GetEnumerator();
        }

        public int IndexOf(int item)
        {
            return this.List.IndexOfKey(item);
        }
        public void Insert(int index, int item)
        {
            throw new NotImplementedException();
        }

        public int NextZoom(int zoomLevel)
        {
            int index;

            index = this.IndexOf(this.FindNearest(zoomLevel));
            if (index < this.Count - 1)
            {
                index++;
            }

            return this[index];
        }
        public int PreviousZoom(int zoomLevel)
        {
            int index;

            index = this.IndexOf(this.FindNearest(zoomLevel));
            if (index > 0)
            {
                index--;
            }

            return this[index];
        }

        public bool Remove(int item)
        {
            return this.List.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.List.RemoveAt(index);
        }

        public int[] ToArray()
        {
            int[] results;

            results = new int[this.Count];
            this.CopyTo(results, 0);

            return results;
        }

        #endregion

        #region IList<int> Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
