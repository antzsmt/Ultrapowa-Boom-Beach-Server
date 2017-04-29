namespace UCS.Files
{
    using Files;
    using System;

    public class Data
    {
        internal DataTable DataTable;
        internal Row Row;

        internal readonly int Id;
        internal int Type => this.DataTable.Index;

        internal Data(Row Row, DataTable DataTable)
        {
            this.Row = Row;
            this.DataTable = DataTable;
            this.Id = DataTable.Datas.Count + 1000000 * DataTable.Index;
        }

        public void Load(Row row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            var type = GetType();
            var properties = type.GetProperties();
            var table = row.Table;

            foreach (var property in properties)
            {
                if (property.DeclaringType == typeof(Data))
                    continue;

                var column = table.Columns[property.Name];
                var propertyType = property.PropertyType;

                if (propertyType.IsArray)
                {
                    // Calculate how many upgrade levels the logic data has.
                    var lvls = row._end - row._start;
                    // Base type of the array.
                    var arrayType = propertyType.GetElementType();
                    // Array instance we're going to set the property value to.
                    var array = Array.CreateInstance(arrayType, lvls);

                    var prevStrValue = (string)null;
                    for (int j = 0; j < lvls; j++)
                    {
                        var strValue = column._data[row._start + j];

                        // If the data value is empty, we check if we
                        // have a non-empty previous data value to use.
                        if (strValue == string.Empty)
                        {
                            if (prevStrValue != null)
                            {
                                // No need to change the type of the value since its
                                // already a string and the property type is string.
                                if (propertyType == typeof(string))
                                {
                                    array.SetValue(prevStrValue, j);
                                }
                                else
                                {
                                    var value = Convert.ChangeType(prevStrValue, arrayType);
                                    array.SetValue(value, j);
                                }
                            }
                            continue;
                        }
                        // Else if the data is not empty, we use it directly.
                        else
                        {
                            // Mark the current value as the previous value or parent value.
                            prevStrValue = strValue;

                            // Update the current value to the one in the column data.
                            var value = Convert.ChangeType(strValue, arrayType);
                            array.SetValue(value, j);
                        }
                    }

                    property.SetValue(this, array);
                }
                else
                {
                    // Take the first value of the data in column from the row.
                    var strValue = column._data[row._start];
                    if (strValue != string.Empty)
                    {
                        // No need to change the type of the value since its
                        // already a string and the property type is string.
                        if (propertyType == typeof(string))
                        {
                            property.SetValue(this, strValue);
                        }
                        else
                        {
                            var value = Convert.ChangeType(strValue, propertyType);
                            property.SetValue(this, value);
                        }
                    }
                }
            }
        }
        internal int GetID()
        {
            return GlobalID.GetID(this.Id);
        }

        internal int GetGlobalID()
        {
            return this.Id;
        }
    }
}
