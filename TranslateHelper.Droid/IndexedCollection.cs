using PortableCore.BL.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TranslateHelper.Droid
{
    //ToDo:Работаем с конкретным типом TranslateResult - можно либо отказаться от генерика, либо переименовать класс во что-то более универсальное
    public class IndexedCollection<T>:IEnumerable<T> where T :IHasLabel, IComparable<T>
    {
        private List<T> collection;

        public IndexedCollection()
        {
            collection = new List<T>();
        }
        public void Add(T value)
        {
            collection.Add(value);
        }

		public void AddList(List<T> value)
		{
			collection = value;
		}

        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Dictionary<string, List<T>> GetSortedData()
        {
            var results = new Dictionary<string, List<T>>();
            List<T> sectionRows = null;
            string currentIndex = null;

            foreach (var e in collection)
            {
                if (e.Label != currentIndex)
                { // start the next index
                    sectionRows = new List<T>();
                    currentIndex = e.Label;
                    if (!results.ContainsKey(currentIndex))
                        results.Add(currentIndex, sectionRows);
                    else results.TryGetValue(currentIndex, out sectionRows);
                }
                sectionRows.Add(e);
            }

            foreach (var v in results.Values)
                v.Sort();

            return results;
        }

    }
}