using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCS.Files
{

    [DebuggerDisplay("Name = {Name}")]
    public class Column
    {
        public Column(Table table, string name)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _name = name;
            _table = table;
            _table._columns.Add(this);

            _data = new List<string>();
        }

        internal readonly List<string> _data;

        private readonly string _name;
        private readonly Table _table; public Table Table => _table;

        public string Name => _name;
    }
}
