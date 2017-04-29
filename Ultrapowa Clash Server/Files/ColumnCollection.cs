namespace UCS.Files
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay("Count = {Count}")]
    public class ColumnCollection : ICollection<Column>
    {
        internal ColumnCollection()
        {
            _columns = new List<Column>();
            _name2index = new Dictionary<string, int>();
        }
        private readonly List<Column> _columns;
        private readonly Dictionary<string, int> _name2index;

        bool ICollection<Column>.IsReadOnly => false;
        public int Count => _columns.Count;
        public Column this[int index] => _columns[index];
        public Column this[string name]
        {
            get
            {
                var index = default(int);
                if (!_name2index.TryGetValue(name, out index))
                    return null;

                return _columns[index];
            }
        }
        public void Add(Column column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            _name2index.Add(column.Name, _columns.Count);
            _columns.Add(column);
        }
        public bool Remove(Column column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            _name2index.Remove(column.Name);
            return _columns.Remove(column);
        }
        public bool Contains(Column column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            return _columns.Contains(column);
        }
        public void Clear()
        {
            _columns.Clear();
        }

        public void CopyTo(Column[] array, int arrayIndex)
        {
            _columns.CopyTo(array, arrayIndex);
        }
        public IEnumerator<Column> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
