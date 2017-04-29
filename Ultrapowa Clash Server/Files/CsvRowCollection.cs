using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace UCS.Files
{
    /// <summary>
    /// Represents a collection of <see cref="CsvRow"/>.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    public class CsvRowCollection : ICollection<CsvRow>
    {
        #region Constructors
        internal CsvRowCollection()
        {
            _rows = new List<CsvRow>();
            _name2index = new Dictionary<string, int>();
        }
        #endregion

        #region Fields & Properties
        private readonly List<CsvRow> _rows;
        private readonly Dictionary<string, int> _name2index;

        bool ICollection<CsvRow>.IsReadOnly => false;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="CsvRowCollection"/>.
        /// </summary>
        public int Count => _rows.Count;

        /// <summary>
        /// Gets the <see cref="CsvRow"/> at the specified index.
        /// </summary>
        /// <param name="index">Index at which to find the <see cref="CsvRow"/>.</param>
        /// <returns><see cref="CsvRow"/> at the specified index.</returns>
        public CsvRow this[int index] => _rows[index];

        /// <summary>
        /// Gets the <see cref="CsvRow"/> with the specified name.
        /// </summary>
        /// <param name="name">Name of the <see cref="CsvRow"/>.</param>
        /// <returns><see cref="CsvRow"/> with the specified name; returns <c>null</c> if not found.</returns>
        public CsvRow this[string name]
        {
            get
            {
                var index = default(int);
                if (!_name2index.TryGetValue(name, out index))
                    return null;

                return _rows[index];
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Adds an <see cref="CsvRow"/> to the end of the <see cref="CsvRowCollection"/>.
        /// </summary>
        /// <param name="row"><see cref="CsvRow"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public void Add(CsvRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _name2index.Add(row.Name, _rows.Count);
            _rows.Add(row);
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="CsvRow"/> from the <see cref="CsvRowCollection"/>.
        /// </summary>
        /// <param name="row"><see cref="CsvRow"/> to remove.</param>
        /// <returns><c>tru</c> if the <see cref="CsvRow"/> was removed successfully; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public bool Remove(CsvRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            _name2index.Remove(row.Name);
            return _rows.Remove(row);
        }

        /// <summary>
        /// Determines whether a <see cref="CsvRow"/> is in the <see cref="CsvRowCollection"/>
        /// </summary>
        /// <param name="row"><see cref="CsvRow"/> to locate.</param>
        /// <returns><c>true</c> if located; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="row"/> is null.</exception>
        public bool Contains(CsvRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return _rows.Contains(row);
        }

        /// <summary>
        /// Removes all the <see cref="CsvRow"/> in the <see cref="CsvRowCollection"/>.
        /// </summary>
        public void Clear()
        {
            _rows.Clear();
        }

        /// <summary>
        /// Copies the entire <see cref="CsvRowCollection"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">Destination array.</param>
        /// <param name="arrayIndex">index in <paramref name="array"/> to start copying to.</param>
        public void CopyTo(CsvRow[] array, int arrayIndex)
        {
            _rows.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerator{T}"/> that iterates through the <see cref="CsvRowCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> that iterates through the <see cref="CsvRowCollection"/>.</returns>
        public IEnumerator<CsvRow> GetEnumerator()
        {
            return _rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
