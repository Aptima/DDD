/*****************************************************
Project		: MOST
Author		: Art Rogers
Copyright	: 2008 Aptima, Inc.
******************************************************/

using System;
using System.Data;

namespace AME.Views.View_Components {

    public class ReportCardModel : DataSet {

        public const string TABLE_REPORT_CARD = "REPORT_CARD";
        public const string PK_ID = "ID";
        public const string MEASURE_NAME = "MEASURE_NAME";
        public const string THRESHOLD_VALUE = "THRESHOLD_VALUE";
        public const string POSSIBLE_COUNT = "POSSIBLE_COUNT";
        public const string COMPLETED_COUNT = "COMPLETED_COUNT";

        //*************************************************
        // Construction/Destruction
        //*************************************************
        public ReportCardModel() {
            BuildTables();
        }

        //*************************************************
        // Private Helpers
        //*************************************************
        private void BuildTables() {
            DataTable table = new DataTable(TABLE_REPORT_CARD);
            DataColumnCollection columns = table.Columns;
            // Primary key
            DataColumn pkColumn = columns.Add(PK_ID, typeof(System.Int32));
            pkColumn.AllowDBNull = false;
            pkColumn.AutoIncrement = true;
            // Add the rest
            columns.Add(MEASURE_NAME, typeof(System.String));
            columns.Add(THRESHOLD_VALUE, typeof(System.Int16));
            columns.Add(POSSIBLE_COUNT, typeof(System.Int16));
            columns.Add(COMPLETED_COUNT, typeof(System.Int16));

            Tables.Add(table);
        }
    }
}
