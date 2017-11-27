using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Reflection;

namespace RecordFileUtil
{
    public class ExcelHelper
    {
        public static string ExportAsTempFile(DataTable dataTable, bool appendColumnNames)
        {
            if(dataTable == null)
            {
                return null;
            }

            return ExportAsTempFile(new DataTable[] { dataTable }, appendColumnNames);
        }

        public static string ExportAsTempFile(DataSet dataSet, bool appendColumnNames)
        {
            string fileName = Path.GetTempFileName();

            if(ExportFile(dataSet, fileName, appendColumnNames))
            {
                return fileName;
            }

            return null;
        }

        public static string ExportAsTempFile(DataTable[] dataTables, bool appendColumnNames)
        {
            string fileName = Path.GetTempFileName();

            if(ExportFile(dataTables, fileName, appendColumnNames))
            {
                return fileName;
            }

            return null;
        }

        public static bool ExportFile(DataTable dataTable, string fileName, bool appendColumnNames)
        {
            if(dataTable == null)
            {
                return false;
            }

            return ExportFile(new DataTable[] { dataTable }, fileName, appendColumnNames);
        }

        public static bool ExportFile(DataSet dataSet, string fileName, bool appendColumnNames)
        {
            if(dataSet == null)
            {
                return false;
            }

            DataTable[] dataTables = new DataTable[dataSet.Tables.Count];
            dataSet.Tables.CopyTo(dataTables, 0);
            return ExportFile(dataTables, fileName, appendColumnNames);
        }

        public static bool ExportFile(DataTable[] dataTables, string fileName, bool appendColumnNames)
        {
            if(dataTables == null || dataTables.Length == 0 || string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            XmlDocument xmlDoc = GetXmlDataTables(dataTables, appendColumnNames);
            XmlDocument xlsDoc = TransformXml(xmlDoc);

            try
            {
                xlsDoc.Save(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static XmlDocument GetXmlDataTables(DataTable[] dataTables, bool appendColumnNames)
        {
            if(dataTables == null)
            {
                return null;
            }

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootNode = xmlDoc.CreateElement("DTS");
            XmlElement tableNode;
            XmlElement rowNode;
            XmlElement colNode;
            DataTable dt;

            for(int i = 0; i < dataTables.Length; i++)
            {
                dt = dataTables[i];

                if(dt == null)
                {
                    break;
                }

                if(dt.TableName.Trim() == string.Empty)
                {
                    dt.TableName = "DataTable" + i.ToString();
                }

                tableNode = xmlDoc.CreateElement("DT");
                tableNode.SetAttribute("N", dt.TableName);

                if(appendColumnNames)
                {
                    rowNode = xmlDoc.CreateElement("DR");

                    foreach(DataColumn dc in dt.Columns)
                    {
                        colNode = xmlDoc.CreateElement("DC");
                        colNode.SetAttribute("N", dc.ColumnName);
                        colNode.SetAttribute("T", "String");
                        colNode.AppendChild(xmlDoc.CreateTextNode(dc.ColumnName));
                        rowNode.AppendChild(colNode);
                    }

                    tableNode.AppendChild(rowNode);
                }


                foreach(DataRow dr in dt.Rows)
                {
                    rowNode = xmlDoc.CreateElement("DR");

                    foreach(DataColumn dc in dt.Columns)
                    {
                        colNode = xmlDoc.CreateElement("DC");
                        colNode.SetAttribute("N", dc.ColumnName);
                        colNode.SetAttribute("T", GetDataType(dc.DataType));
                        colNode.AppendChild(xmlDoc.CreateTextNode(GetTextValue(dc.DataType, dr[dc.ColumnName])));
                        rowNode.AppendChild(colNode);
                    }

                    tableNode.AppendChild(rowNode);
                }

                rootNode.AppendChild(tableNode);
            }

            xmlDoc.AppendChild(rootNode);
            return xmlDoc;
        }

        private static string GetTextValue(Type type, object value)
        {
            string text;

            if(type == typeof(DateTime))
            {
                text = ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else
            {
                text = value.ToString();
            }

            return text;
        }

        private static string GetDataType(Type type)
        {
            string dataType;

            if(type == typeof(string))
            {
                dataType = "String";
            }
            else if(type == typeof(DateTime))
            {
                dataType = "DateTime";
            }
            else if(type == typeof(bool))
            {
                dataType = "Boolean";
            }
            else
            {
                dataType = "Number";
            }

            return dataType;
        }

        private static XmlDocument TransformXml(XmlDocument xmlDoc)
        {
            XmlDocument xlsDoc = new XmlDocument();
            XslCompiledTransform xslt = new XslCompiledTransform();
            Assembly assembly = Assembly.GetExecutingAssembly();

            using(Stream s = assembly.GetManifestResourceStream("RecordFileUtil.XmlSpreadsheet.xsl"))
            {
                if(s != null)
                {
                    xslt.Load(XmlReader.Create(s));
                    MemoryStream output = new MemoryStream();
                    XmlTextWriter xmlWriter = new XmlTextWriter(output, Encoding.UTF8);
                    xslt.Transform(xmlDoc, xmlWriter);
                    output.Position = 0;
                    xlsDoc.Load(output);
                    xlsDoc.PrependChild(xlsDoc.CreateXmlDeclaration("1.0", null, null));
                    output = null;
                }
            }

            return xlsDoc;
        }

        public static DataTable LoadXMLFile(String filename)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filename);
            
            XmlNamespaceManager m = new XmlNamespaceManager(xmldoc.NameTable);
            m.AddNamespace("nhb", "urn:schemas-microsoft-com:office:spreadsheet");
            XmlNodeList list = xmldoc.SelectNodes("/nhb:Workbook/nhb:Worksheet/nhb:Table/nhb:Row",m);
            DataRow dr;
            foreach (XmlNode node in list)
            {
                dr = dt.NewRow();
                dr[0]=node.ChildNodes[0].ChildNodes[0].InnerText;
                dr[1] = node.ChildNodes[1].ChildNodes[0].InnerText;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
