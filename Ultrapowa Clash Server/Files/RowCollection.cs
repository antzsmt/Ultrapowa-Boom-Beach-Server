namespace UCS.Files
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Count = {Count}")]
    public class RowCollection : ICollection<Row>
    {
        internal RowCollection()
        {
            _rows = new List<Row>();
            _name2index = new Dictionary<string, int>();
        }

        private readonly List<Row> _rows;
        private readonly Dictionary<string, int> _name2index;
        bool ICollection<Row>.IsReadOnly => false;
        public int Count => _rows.Count;
        public Row this[int index] => _rows[index];
        public Row this[string name]
        {
            get
            {
                var index = default(int);
                if (!_name2index.TryGetValue(name, out index))
                    return null;

                return _rows[index];
            }
        }
        public void Add(Row row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _name2index.Add(row.Name, _rows.Count);
            _rows.Add(row);
        }
        public bool Remove(Row row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _name2index.Remove(row.Name);
            return _rows.Remove(row);
        }
        public bool Contains(Row row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return _rows.Contains(row);
        }
        public void Clear()
        {
            _rows.Clear();
        }
        public void CopyTo(Row[] array, int arrayIndex)
        {
            _rows.CopyTo(array, arrayIndex);
        }
        public IEnumerator<Row> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
