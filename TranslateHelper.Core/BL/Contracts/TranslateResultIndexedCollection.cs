using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Collections;

namespace TranslateHelper.Core.BL.Contracts
{
    //ToDo:�������� ������������� ������� ������ TranslateResultCollection �� ����
    //ToDo:�������� � ���������� ����� TranslateResult - ����� ���� ���������� �� ��������, ���� ������������� ����� �� ���-�� ����� �������������
    class TranslateResultIndexedCollection<T>:IEnumerable<T> where T :IHasLabel, IComparable<T>
    {
        private List<T> collection;

        public TranslateResultIndexedCollection()
        {
            collection = new List<T>();
        }
        public void Add(T value)
        {
            collection.Add(value);
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
                }
                sectionRows.Add(e);
            }

            foreach (var v in results.Values)
                v.Sort();

            return results;
        }

    }
}