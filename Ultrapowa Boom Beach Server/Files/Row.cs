using System;
using System.Collections.Generic;
namespace UCS.Files
{
    public class Row
    {
        public Row(Table table, string name)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _name = name;
            _table = table;
            _table._rows.Add(this);
        }

        internal int _start;
        internal int _end;

        public Table Table => _table;
        public string Name => _name;

        private readonly string _name;
        private readonly Table _table;
    }
}
