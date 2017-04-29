using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace UCS.Files
{
    /// <summary>
    /// Represents a collection of <see cref="CsvColumn"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public class CsvColumnCollection : ICollection<CsvColumn>
    {
        #region Constructors
        internal CsvColumnCollection()
        {
            _columns = new List<CsvColumn>();
            _name2index = new Dictionary<string, int>();
        }
        #endregion

        #region Fields & Properties
        private readonly List<CsvColumn> _columns;
        private readonly Dictionary<string, int> _name2index;

        bool ICollection<CsvColumn>.IsReadOnly => false;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="CsvColumnCollection"/>.
        /// </summary>
        public int Count => _columns.Count;

        /// <summary>
        /// Gets the <see cref="CsvColumn"/> at the specified index.
        /// </summary>
        /// <param name="index">Index at which to find the <see cref="CsvColumn"/>.</param>
        /// <returns><see cref="CsvColumn"/> at the specified index.</returns>
        public CsvColumn this[int index] => _columns[index];

        /// <summary>
        /// Gets the <see cref="CsvColumn"/> with the specified name.
        /// </summary>
        /// <param name="name">Name of the <see cref="CsvColumn"/>.</param>
        /// <returns><see cref="CsvColumn"/> with the specified name; returns <c>null</c> if not found.</returns>
        public CsvColumn this[string name]
        {
            get
            {
                var index = default(int);
                if (!_name2index.TryGetValue(name, out index))
                    return null;

                return _columns[index];
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds an <see cref="CsvColumn"/> to the end of the <see cref="CsvColumnCollection"/>.
        /// </summary>
        /// <param name="column"><see cref="CsvColumn"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="column"/> is null.</exception>
        public void Add(CsvColumn column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            _name2index.Add(column.Name, _columns.Count);
            _columns.Add(column);
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="CsvColumn"/> from the <see cref="CsvColumnCollection"/>.
        /// </summary>
        /// <param name="column"><see cref="CsvColumn"/> to remove.</param>
        /// <returns><c>tru</c> if the <see cref="CsvColumn"/> was removed successfully; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="column"/> is null.</exception>
        public bool Remove(CsvColumn column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            _name2index.Remove(column.Name);
            return _columns.Remove(column);
        }

        /// <summary>
        /// Determines whether a <see cref="CsvColumn"/> is in the <see cref="CsvColumnCollection"/>
        /// </summary>
        /// <param name="column"><see cref="CsvColumn"/> to locate.</param>
        /// <returns><c>true</c> if located; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="column"/> is null.</exception>
        public bool Contains(CsvColumn column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            return _columns.Contains(column);
        }

        /// <summary>
        /// Removes all the <see cref="CsvColumn"/> in the <see cref="CsvColumnCollection"/>.
        /// </summary>
        public void Clear()
        {
            _columns.Clear();
        }

        /// <summary>
        /// Copies the entire <see cref="CsvColumnCollection"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">index in <paramref name="array"/> to start copying to.</param>
        public void CopyTo(CsvColumn[] array, int arrayIndex)
        {
            _columns.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator{T}"/> that iterates through the <see cref="CsvColumnCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> that iterates through the <see cref="CsvColumnCollection"/>.</returns>
        public IEnumerator<CsvColumn> GetEnumerator()
        {
            return _columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
