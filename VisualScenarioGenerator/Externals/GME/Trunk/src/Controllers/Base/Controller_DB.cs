/*
 * Class            : Controller
 * File             : Controller_DB.cs
 * Author           : Bhavna Mangal
 * File Description : One piece of partial class Controller.
 * Description      :
 */

#region Imported Namespaces

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using AME.Model;

#endregion  //Namespaces

namespace AME.Controllers
{
    public partial class Controller
    {
        //Intersection
        //Union
        //Difference
        private DataTable _Intersect(
            DataTable parentTable, string pColName,
            DataTable childTable, string cColName)
        {
            DataTable parent = parentTable.Copy();
            DataTable child = childTable.Copy();
            //Creating Empty Table
            DataTable table = new DataTable("Intersect");

            //Creating Dataset to use DataRelation
            using (DataSet ds = new DataSet("IntersectedData"))
            {

                //Adding Tables to Dataset
                ds.Tables.AddRange(new DataTable[] { parent, child});

                //Creating columns for DataRelation
                DataColumn colParent = parent.Columns[pColName];
                DataColumn colChild = child.Columns[cColName];

                //Creating DataRelation
                DataRelation relIntersect = new DataRelation("RelIntersect", colParent, colChild);

                //Adding DataRelation to DataSet.
                ds.Relations.Add(relIntersect); //TODO: solve problem here

                //Cloning the Structure of Parent table to Return table.
                table = parent.Clone();

                //if Parent row is in Child table, Add it to Return table.
                table.BeginLoadData();
                foreach (DataRow parentRow in parent.Rows)
                {
                    DataRow[] childRows = parentRow.GetChildRows(relIntersect);

                    if (childRows.Length > 0)
                    {
                        table.LoadDataRow(parentRow.ItemArray, true);
                    }
                }//foreach parent row
                table.EndLoadData();
            }//using Dataset

            return table;            
        }//_Intersect

        private DataTable _Intersect(DataTable table1, DataTable table2, string column)
        {
            if (!this._SchemaMatching(table1, table2))
            {
                string sError = string.Format("Tables' schemas do not match.");
                throw new System.Exception(sError);
            }

            return this._Intersect(table1, column, table2, column);
        }//_Intersect

        private Dictionary<string, Type> _GetColumns(DataTable table)
        {
            Dictionary<string, Type> colsInfo = new Dictionary<string, Type>();
            DataColumnCollection columns = table.Columns;

            foreach (DataColumn column in columns)
            {
                colsInfo.Add(column.ColumnName, column.DataType);
            }//foreach column

            return colsInfo;
        }//_GetColumns

#if NotNeeded
        private StringWriter _GetSchema(DataTable table)
        {
            return this._GetSchema(table, false);
        }//_GetSchema

        private StringWriter _GetSchema(DataTable table, bool hierarchy)
        {
            StringWriter writer = new StringWriter();

            if (table != null)
            {
                try
                {
                    table.WriteXmlSchema(writer, hierarchy);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return writer;
        }//_GetSchema
#endif

        private bool _SchemaMatching(DataTable table1, DataTable table2)
        {
            Dictionary<string, Type> columns1 = this._GetColumns(table1);
            Dictionary<string, Type> columns2 = this._GetColumns(table2);

            if (columns1.Count != columns2.Count)
            {
                return false;
            }

#if NotNeeded
            StringWriter schema1 = this._GetSchema(table1);
            StringWriter schema2 = this._GetSchema(table2);

            StringBuilder sb1 = schema1.GetStringBuilder();
            StringBuilder sb2 = schema2.GetStringBuilder();

            return sb1.Equals(sb2);
#endif

            Dictionary<string, Type>.Enumerator enumerator1 = columns1.GetEnumerator();
            Dictionary<string, Type>.Enumerator enumerator2 = columns2.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                //TODO: compare if both column collections are same
                KeyValuePair<string, Type> pair1 = enumerator1.Current;
                KeyValuePair<string, Type> pair2 = enumerator1.Current;

                string column1_Name = pair1.Key;
                string column2_Name = pair2.Key;

                Type column1_Type = pair1.Value;
                Type column2_Type = pair2.Value;

                if (!column1_Name.Equals(column2_Name)
                    || !column1_Type.Equals(column2_Type))
                {
                    return false;
                }

            }//while

            return true;
        }//_SchemaMatching

    }//Controller class
}//namespace AME.Controllers