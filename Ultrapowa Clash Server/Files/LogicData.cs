using System;

namespace UCS.Files
{
    /// <summary>
    /// Represents a data that governs logic.
    /// </summary>
    public abstract class LogicData
    {
        #region Fields & Properties
        // Global ID of the LogicData instance, set by loading of data relative
        // to CsvTable.
        internal int _id;

        /// <summary>
        /// Gets the global ID of this <see cref="LogicData"/>.
        /// </summary>
        public int Id => _id;

        /// <summary>
        /// Gets the kind ID of this <see cref="LogicData"/> type.
        /// </summary>
        protected internal abstract int KindId { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Loads this instance of the <see cref="LogicData"/> from the specified <see cref="CsvRow"/>.
        /// </summary>
        /// <param name="row"><see cref="CsvRow"/> form which to load data.</param>
        public void Load(CsvRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            var type = GetType();
            var properties = type.GetProperties();
            var table = row.Table;

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.DeclaringType == typeof(LogicData))
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
        #endregion
    }
}
