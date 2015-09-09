using System;
using System.Collections.Generic;
using System.Text;

namespace AME.Adapters
{
    /// <summary>
    /// Class for Exporting Data to a char array in a CSV format
    /// </summary>
    abstract public class ToCsvExportDataAdapter : IExportDataAdapter<String>
    {
        private ToCsvExportDataSettings settings;
        protected StringBuilder outputStringBuilder;

        protected ToCsvExportDataAdapter(ToCsvExportDataSettings settings)
        {
            this.settings = settings;
        }

        #region IExportDataAdapter<String> Members

        abstract public String Process();
        
        #endregion

        /// <summary>
        /// Adds a set of values to the current line followed by the default line terminator.
        /// </summary>
        /// <param name="values"></param>
        protected void AddRow(List<String> values)
        {
            foreach (String s in values)
            {
                AddCellValue(s);
            }
            NewRow();
        }

        /// <summary>
        /// Adds a single value to the current line of the csv buffer.
        /// The value will be followed by a ','.
        /// </summary>
        /// <param name="value"></param>
        protected void AddCellValue(String value)
        {
            if(value.Contains(","))
            {
                outputStringBuilder.Append("\"");
                outputStringBuilder.Append(value);
                outputStringBuilder.Append("\"");
            }
            else
            {
               outputStringBuilder.Append(value);
            }
            outputStringBuilder.Append(",");
        }
        /// <summary>
        /// Inserts the default line terminator to the end of the output buffer.
        /// </summary>
        protected void NewRow()
        {
            outputStringBuilder.AppendLine();
        }


    }

    public class ToCsvExportDataSettings : IExportDataSettings
    {

    }

}
