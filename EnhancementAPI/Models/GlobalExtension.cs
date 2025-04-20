using System.Collections.Generic;
using System.Data;

namespace EnhancementAPI.Models
{
    public static class GlobalExtension
    {
        /// <summary>
        /// Returns instance of this DataSet to List of Object.
        /// </summary>
        /// <param name="Dataset">The object of DataSet to convert List.</param>
        /// <returns></returns>
        public static List<object> ToList(this DataSet Dataset)
        {
            List<object> lstObject = new List<object>();

            try
            {
                for (int tableIndex = 0; tableIndex < Dataset.Tables.Count; tableIndex++)
                {
                    DataTable dtToJson = Dataset.Tables[tableIndex];
                    List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
                    Dictionary<string, object> childRow;
                    foreach (DataRow row in dtToJson.Rows)
                    {
                        childRow = new Dictionary<string, object>();
                        foreach (DataColumn col in dtToJson.Columns)
                        {
                            childRow.Add(col.ColumnName, row[col]);
                        }
                        parentRow.Add(childRow);
                    }

                    //string jsonData= jsSerializer.Serialize(parentRow);
                    //  JavaScriptSerializer jser = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue, RecursionLimit = 100 };
                    lstObject.Add(parentRow);

                }
                return lstObject;
            }
            catch
            {
                return null;
            }



        }

        public static DataSet GetTableName(this DataSet DataSet)
        {
            try
            {
                foreach (DataTable dt in DataSet.Tables)
                {
                    try
                    {
                        dt.TableName = dt.Rows[0]["TableName"].ToString();
                    }
                    catch { }
                }
            }
            catch { }
            return DataSet;
        }
    }
}