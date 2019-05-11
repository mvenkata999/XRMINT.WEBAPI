using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarXRMWebAPI
{
    public static class Extensions
    {
        public static void CopyPropertyValues(object source, object destination)
        {
            var destProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                foreach (var destProperty in destProperties)
                {
                    if (destProperty.Name == sourceProperty.Name &&
                destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    {
                        destProperty.SetValue(destination, sourceProperty.GetValue(
                            source, new object[] { }), new object[] { });

                        break;
                    }
                }
            }
        }

        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
        {

            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    try
                    {
                        p.SetValue(dest, sourceProp.GetValue(source, null).ToString(), null);
                    }
                    catch (Exception)
                    {


                    }

                }

            }

        }
        public static string TryGetElementValue(this XElement parentEl, string elementName, string defaultValue = null)
        {
            var foundEl = parentEl.Element(elementName);

            if (foundEl != null)
            {
                return foundEl.Value;
            }

            return defaultValue;
        }

        public static string SerializeXML<T>(this T value)
        {
            if (value == null) return string.Empty;

            //XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            //namespaces.Add("soap", "soap");
            //namespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            //namespaces.Add("tran", "http://www.starstandards.org/webservices/2009/transport");
            var xmlserializer = new XmlSerializer(typeof(T));

            using (StringWriter stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    //xmlserializer.Serialize(writer, value, namespaces);
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
        }

        public static string SerializeXMLWithNamespaces<T>(this T value, XmlSerializerNamespaces namespaces)
        {
            if (value == null) return string.Empty;

            //XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            //namespaces.Add("soap", "soap");
            //namespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            //namespaces.Add("tran", "http://www.starstandards.org/webservices/2009/transport");
            var xmlserializer = new XmlSerializer(typeof(T));

            using (StringWriter stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlserializer.Serialize(writer, value, namespaces);
                    //xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
        }

        public static string SerializeXMLNoNamespaces<T>(this T value)
        {
            if (value == null) return string.Empty;

            //XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            //namespaces.Add("soap", "soap");
            //namespaces.Add("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            //namespaces.Add("tran", "http://www.starstandards.org/webservices/2009/transport");
            var xmlserializer = new XmlSerializer(typeof(T));

            using (StringWriter stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    //xmlserializer.Serialize(writer, value, namespaces);
                    var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    xmlserializer.Serialize(writer, value, emptyNamepsaces);
                    return stringWriter.ToString();
                }
            }
        }

        public static string SerializeXMLNoDeclaration<T>(this T value)
        {
            if (value == null) return string.Empty;

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");

            XmlSerializer cs = new XmlSerializer(typeof(T));

            cs.Serialize(writer, value, names);

            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }
        public static string SerializeObject(object obj)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                serializer.Serialize(ms, obj);
                ms.Position = 0;
                xmlDoc.Load(ms);
                return xmlDoc.InnerXml;
            }
        }


        public static string GenericSerialize<T>(T dataToSerialize)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
            catch
            {
                throw;
            }
        }

        public static T GenericDeserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Convert XmlElement to an Object of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlElement"></param>
        /// <returns></returns>
        public static object XmlElementToObject<T>(XmlElement _Doc)
        {
            var ser = new XmlSerializer(typeof(T));
            var wrapper = (T)ser.Deserialize(new XmlNodeReader(_Doc));
            return wrapper;
        }

        /// <summary>
        /// Convert XElement to an Object of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_XElement"></param>
        /// <returns></returns>
        public static object XElementToObject<T>(XElement _XElement)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(_XElement.CreateReader());
        }

        /// <summary>
        /// Convert an XDocument to an XmlDocument
        /// </summary>
        public static XmlDocument ToXmlDocument(this XDocument _xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = _xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }


        /// <summary>
        /// Remove the empty string elements
        /// </summary>
        public static XDocument RemoveEmptyElements(this XDocument _xDocument)
        {
            _xDocument.Descendants().Where(e => string.IsNullOrEmpty(e.Value)).Remove();
            // VB =  DealAddDoc.Descendants().Where(Function(e) String.IsNullOrEmpty(e.Value)).Remove()

            return _xDocument;
        }

        /// <summary>
        /// Convert an XmlDocument to an XDocument.
        /// </summary>
        public static XDocument ToXDocument(this XmlDocument _xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(_xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        /// <summary>
        /// Convert an XElement to an XmlElement
        /// </summary>
        public static XmlElement ToXmlElement(this XElement _element)
        {
            var doc = new XmlDocument();
            doc.Load(_element.CreateReader());
            return doc.DocumentElement;
        }

        /// <summary>
        /// Find or add a node, and find and change or add and set an attribute to a value.
        /// Force a Header element, if there isn't one.  (This is specific to Lead Responder.)
        /// </summary>
        public static void SetXAttribute(this XContainer _doc, string _nodeName, string _attributeName, string _value)
        {
            var header = _doc.Descendants("Header").FirstOrDefault();
            if (header == null)
            {
                _doc.Add(new XElement("Header"));
            }
            var node = _doc.Descendants(_nodeName).FirstOrDefault();
            if (node == null)
            {
                header.Add(node = new XElement(_nodeName));
            }
            var attribute = node.Attribute(_attributeName);
            if (attribute != null)
            {
                attribute.Value = _value;
            }
            else
            {
                node.Add(new XAttribute(_attributeName, _value));
            }
        }

        /// <summary>
        /// Given an XElement name and default value, return the element's value or, if null, the default.
        /// </summary>
        public static string GetElementText(this XElement _element, string _name, string _default = "")
        {
            var e = _element.Element(_name);
            return e == null ? _default : e.Value;
        }

        /// <summary>
        /// Given an XElement, an Attribute name, and a default value, return the attribute's value or, if null, the default.
        /// </summary>
        public static string GetAttributeText(this XElement _element, string _name, string _default = "")
        {
            var a = _element.Attribute(_name);
            return a == null ? _default : a.Value.ToString();
        }

        /// <summary>
        /// Given a DataRow and a string (ColumnName), convert it to the requested Type, returning the default
        /// for the Type if the value is null or empty.  T can be Nullable.
        /// </summary>
        public static T GetValue<T>(this DataRow _row, string _column)
        {
            T rc = default(T);
            if (!_row.IsNull(_column))
            {
                var tc = TypeDescriptor.GetConverter(typeof(T));
                rc = (T)tc.ConvertFromString(_row[_column].ToString());
            }
            return rc;
        }

        /// <summary>
        /// Create a new SqlParameter, including type length, and Value, add it to the collection, and return the collection 
        /// so that another parameter can be chained on.
        /// </summary>
        public static SqlParameterCollection AddEx(this SqlParameterCollection _coll, string _name, SqlDbType _type, int _length, object _value)
        {
            _coll.Add(_name, _type, _length).Value = _value;
            return _coll;
        }

        /// <summary>
        /// Create a new SqlParameter, including Value, add it to the collection, and return the collection so that another 
        /// parameter can be chained on.
        /// </summary>
        public static SqlParameterCollection AddEx(this SqlParameterCollection _coll, string _name, SqlDbType _type, object _value)
        {
            _coll.Add(_name, _type).Value = _value;
            return _coll;
        }

        public static DataSet CreateDataSet<T>(List<T> list)
        {
            //list is nothing or has nothing, return nothing (or add exception handling)
            if (list == null || list.Count == 0) { return null; }

            //get the type of the first obj in the list
            var obj = list[0].GetType();

            //now grab all properties
            var properties = obj.GetProperties();

            //make sure the obj has properties, return nothing (or add exception handling)
            if (properties.Length == 0) { return null; }

            //it does so create the dataset and table
            var dataSet = new DataSet();
            var dataTable = new DataTable();

            //now build the columns from the properties
            var columns = new DataColumn[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                columns[i] = new DataColumn(properties[i].Name, properties[i].PropertyType);
            }

            //add columns to table
            dataTable.Columns.AddRange(columns);

            //now add the list values to the table
            foreach (var item in list)
            {
                //create a new row from table
                var dataRow = dataTable.NewRow();

                //now we have to iterate thru each property of the item and retrieve it's value for the corresponding row's cell
                var itemProperties = item.GetType().GetProperties();

                for (int i = 0; i < itemProperties.Length; i++)
                {
                    dataRow[i] = itemProperties[i].GetValue(item, null);
                }

                //now add the populated row to the table
                dataTable.Rows.Add(dataRow);
            }

            //add table to dataset
            dataSet.Tables.Add(dataTable);

            //return dataset
            return dataSet;
        }

        public static DataTable ConvertToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static bool TryAddCookie(this WebRequest webRequest, Cookie cookie)
        {
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                return false;
            }

            if (httpRequest.CookieContainer == null)
            {
                httpRequest.CookieContainer = new CookieContainer();
            }

            httpRequest.CookieContainer.Add(new Uri("https://www.guestconceptslink.com"), cookie);

            return true;
        }

        public static string DataTableToXml(this DataTable table, int metaIndex = 0)
        {
            XDocument xdoc = new XDocument(
                //new XElement(table.TableName,
                //    from column in table.Columns.Cast<DataColumn>()
                //    where column != table.Columns[metaIndex]
                //    select new XElement(column.ColumnName,
                //        from row in table.AsEnumerable()

                //        select new XElement(row.Field<string>(metaIndex), Convert.ToString(row[column]))
                //        )
                //    )
                );

            return xdoc.ToString();
        }

        public static string ConvertDataTableToXML(DataTable dtBuildSQL, string TableName, string XmlDataFormat)
        {

            DataSet dsBuildSQL = new DataSet("CarResearch" + TableName);

            StringBuilder sbSQL;
            StringWriter swSQL;

            string XMLformat;

            sbSQL = new StringBuilder();
            swSQL = new StringWriter(sbSQL);

            dsBuildSQL.Merge(dtBuildSQL, true, MissingSchemaAction.AddWithKey);
            dsBuildSQL.Tables[0].TableName = TableName;

            foreach (DataColumn col in dsBuildSQL.Tables[0].Columns)
            {
                if (XmlDataFormat == "Attribute")
                {
                    col.ColumnMapping = MappingType.Attribute;
                }
                else
                {
                    col.ColumnMapping = MappingType.Element;
                }
            }

            // to include a Schema
            // dsBuildSQL.WriteXml(swSQL, XmlWriteMode.WriteSchema);
            dsBuildSQL.WriteXml(swSQL);

            XMLformat = sbSQL.ToString();

            return XMLformat;

        }

        public static string ConvertDataTableToXMLString(DataTable dtData, string tableName)
        {
            DataSet dsData = new DataSet();
            StringBuilder sbSQL;
            StringWriter swSQL;
            string XMLformat;
            try
            {
                sbSQL = new StringBuilder();
                swSQL = new StringWriter(sbSQL);
                dsData.Merge(dtData, true, MissingSchemaAction.AddWithKey);
                dsData.Tables[0].TableName = tableName;
                foreach (DataColumn col in dsData.Tables[0].Columns)
                {
                    col.ColumnMapping = MappingType.Attribute;
                }
                dsData.WriteXml(swSQL, XmlWriteMode.WriteSchema);
                XMLformat = sbSQL.ToString();
                return XMLformat;
            }
            catch (Exception sysException)
            {
                throw sysException;
            }
        }

        public static string GetJSONString(DataTable Dt)
        {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;

            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

            StringBuilder Sb = new StringBuilder();
            Sb.Append("{\"" + Dt.TableName + "\" : [");

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string TempStr = HeadStr;
                Sb.Append("{");

                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
                }
                Sb.Append(TempStr + "},");
            }

            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]}");

            return Sb.ToString();
        }

        public static string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }


        public static string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder jsonString = new StringBuilder();


            if (ds.Tables[0].Rows.Count > 0)
            {
                jsonString.Append("[");
                for (int rows = 0; rows < ds.Tables[0].Rows.Count; rows++)
                {
                    jsonString.Append("{");
                    for (int cols = 0; cols < ds.Tables[0].Columns.Count; cols++)
                    {
                        jsonString.Append(@"""" + ds.Tables[0].Columns[cols].ColumnName + @""":");

                        /* 
                        //IF NOT LAST PROPERTY

                        if (cols < ds.Tables[0].Columns.Count - 1)
                        {
                            GenerateJsonProperty(ds, rows, cols, jsonString);
                        }

                        //IF LAST PROPERTY

                        else if (cols == ds.Tables[0].Columns.Count - 1)
                        {
                            GenerateJsonProperty(ds, rows, cols, jsonString, true);
                        }
                        */

                        var b = (cols < ds.Tables[0].Columns.Count - 1)
                            ? GenerateJsonProperty(ds, rows, cols, jsonString)
                            : (cols != ds.Tables[0].Columns.Count - 1)
                              || GenerateJsonProperty(ds, rows, cols, jsonString, true);
                    }
                    jsonString.Append(rows == ds.Tables[0].Rows.Count - 1 ? "}" : "},");
                }
                jsonString.Append("]");
                return jsonString.ToString();
            }
            return null;
        }



        private static bool GenerateJsonProperty(DataSet ds, int rows, int cols, StringBuilder jsonString, bool isLast = false)
        {

            // IF LAST PROPERTY THEN REMOVE 'COMMA'  IF NOT LAST PROPERTY THEN ADD 'COMMA'
            string addComma = isLast ? "" : ",";

            if (ds.Tables[0].Rows[rows][cols] == DBNull.Value)
            {
                jsonString.Append(" null " + addComma);
            }
            else if (ds.Tables[0].Columns[cols].DataType == typeof(DateTime))
            {
                jsonString.Append(@"""" + (((DateTime)ds.Tables[0].Rows[rows][cols]).ToString("yyyy-MM-dd HH':'mm':'ss")) + @"""" + addComma);
            }
            else if (ds.Tables[0].Columns[cols].DataType == typeof(string))
            {
                jsonString.Append(@"""" + (ds.Tables[0].Rows[rows][cols]) + @"""" + addComma);
            }
            else if (ds.Tables[0].Columns[cols].DataType == typeof(bool))
            {
                jsonString.Append(Convert.ToBoolean(ds.Tables[0].Rows[rows][cols]) ? "true" : "fasle");
            }
            else
            {
                jsonString.Append(ds.Tables[0].Rows[rows][cols] + addComma);
            }

            return true;
        }

        public static string SerializeToJSON<T>(this T value)
        {
            if (value == null) return string.Empty;

            var Jsonserializer = new JsonSerializer();

            using (StringWriter stringWriter = new StringWriter())
            {
                using (JsonWriter writer = new JsonTextWriter(stringWriter))
                {
                    Jsonserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
        }



    }
}
