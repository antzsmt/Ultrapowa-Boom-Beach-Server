using System;
using System.Diagnostics;

namespace UCS.Files
{
    /// <summary>
    /// Represents a CSV row in a <see cref="CsvTable"/>.
    /// </summary>
    [DebuggerDisplay("Name = {Name}")]
    public class CsvRow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvRow"/> class with the specified <see cref="CsvTable"/>
        /// as parent.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="name"></param>
        public CsvRow(CsvTable table, string name)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _name = name;
            _table = table;
            _table._rows.Add(this);
        }
        #endregion

        #region Fields & Properties
        internal int _start;
        internal int _end;

        private readonly string _name;
        // Table which owns this row.
        private readonly CsvTable _table;

        /// <summary>
        /// Gets the <see cref="CsvTable"/> that owns this <see cref="CsvRow"/>.
        /// </summary>
        public CsvTable Table => _table;

        /// <summary>
        /// Gets the name of this <see cref="CsvRow"/> instance.
        /// </summary>
        public string Name => _name;
        #endregion
    }
}
