using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UCS.Files
{
    // A Column contains an array of values.

    /// <summary>
    /// Represents a CSV column in a <see cref="CsvTable"/>.
    /// </summary>
    [DebuggerDisplay("Name = {Name}")]
    public class CsvColumn
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumn"/> class with the specified <see cref="CsvTable"/>
        /// as parent.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="name"></param>
        public CsvColumn(CsvTable table, string name)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _name = name;
            _table = table;
            _table._columns.Add(this);

            _data = new List<string>();
        }
        #endregion

        #region Fields & Properties
        internal readonly List<string> _data;

        private readonly string _name;
        // Table which owns this column.
        private readonly CsvTable _table;

        /// <summary>
        /// Gets the <see cref="CsvTable"/> that owns this <see cref="CsvColumn"/>.
        /// </summary>
        public CsvTable Table => _table;

        /// <summary>
        /// Gets the name of the <see cref="CsvColumn"/>.
        /// </summary>
        public string Name => _name;
        #endregion
    }
}
