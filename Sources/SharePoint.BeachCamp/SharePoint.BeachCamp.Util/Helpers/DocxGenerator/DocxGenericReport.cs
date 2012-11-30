using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Reflection;
using SharePoint.BeachCamp.Util.Helper.DocXGenerator;

namespace SharePoint.BeachCamp.Util.Helper.DocXGenerator
{
    /// <summary>
    /// Usage
    /// DocxGenericReport<MySampleReport> reporter = new DocxGenericReport<MySampleReport>(filepath, data);
    ///        reporter.GenerateDocument();
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DocxGenericReport<T> : SharePoint.BeachCamp.Util.Helper.DocXGenerator.DocumentGenerator where T : IReportBase
    {
        public T ReportData{ get; set; }

        
        private static DocumentGenerationInfo GetDocumentGenerationInfo( object dataContext, string fileName, bool useDataBoundControls)
        {
            DocumentGenerationInfo generationInfo = new DocumentGenerationInfo();
            generationInfo.Metadata = new DocumentMetadata() { DocumentType = "DocxGenericReport", DocumentVersion = "1.0" };
            generationInfo.DataContext = dataContext;
            generationInfo.TemplateData = File.ReadAllBytes(fileName);
            generationInfo.IsDataBoundControls = useDataBoundControls;

            return generationInfo;
            
        }

        
       
        public DocxGenericReport(string templateFile, T reportData) 
            : base ( GetDocumentGenerationInfo( reportData, templateFile, false))
        {
            ReportData = reportData;
        }
        #region Override methods
        
        protected override void ContainerPlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {

        }
        protected override void RecursivePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {

        }
        protected override void IgnorePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {

        }
        protected override void RefreshCharts(DocumentFormat.OpenXml.Packaging.MainDocumentPart mainDocumentPart)
        {

        }
        protected override void NonRecursivePlaceholderFound(string placeholderTag, OpenXmlElementDataContext openXmlElementDataContext)
        {
            if (openXmlElementDataContext == null || openXmlElementDataContext.Element == null || openXmlElementDataContext.DataContext == null)
            {
                return;
            }

            string tagPlaceHolderValue = string.Empty;
            string tagGuidPart = string.Empty;
            GetTagValue(openXmlElementDataContext.Element as SdtElement, out tagPlaceHolderValue, out tagGuidPart);

            string tagValue = string.Empty;
            string content = string.Empty;

            tagValue = GetPlaceHolderData(tagPlaceHolderValue, (T)openXmlElementDataContext.DataContext);
            content = tagValue;
            //switch (tagPlaceHolderValue)
            //{
            //    //case "FULLNAME":
            //    //    tagValue = ((openXmlElementDataContext.DataContext) as string);
            //    //    content = ((openXmlElementDataContext.DataContext) as string);
            //    //    break;

            //}

            // Set the tag for the content control
            if (!string.IsNullOrEmpty(tagValue))
            {
                this.SetTagValue(openXmlElementDataContext.Element as SdtElement, GetFullTagValue(tagPlaceHolderValue, tagValue));
            }

            // Set text without data binding
            this.SetContentOfContentControl(openXmlElementDataContext.Element as SdtElement, content);
        }

        private string GetPlaceHolderData(string tagPlaceHolderValue, T data)
        {
            Type type = typeof(T);
            string results = string.Empty;

            foreach (PropertyInfo item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var placeHolder = item.GetCustomAttributes(typeof(PlaceHolderAttribute), true).Cast<PlaceHolderAttribute>().FirstOrDefault();
               if(placeHolder!=null && placeHolder.Type == PlaceHolderType.NonRecursive && placeHolder.Name == tagPlaceHolderValue){
               var rawdata = item.GetValue(data, null );
               if (rawdata != null)
               {
                   results = rawdata.ToString();
               }

               }
            }
            return results;
        }
        #endregion
        protected override Dictionary<string, PlaceHolderType> GetPlaceHolderTagToTypeCollection()
        {
            Dictionary<string, PlaceHolderType> placeHolderTagToTypeCollection = new Dictionary<string, PlaceHolderType>();
            //placeHolderTagToTypeCollection.Add("FULLNAME", PlaceHolderType.NonRecursive);
            Type type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            foreach (var pi in properties)
            {
                var placeholder =  pi.GetCustomAttributes(typeof(PlaceHolderAttribute), true).Cast<PlaceHolderAttribute>().FirstOrDefault();
                //foreach (PlaceHolderAttribute item in placeholders)
                {
                    placeHolderTagToTypeCollection.Add(placeholder.Name, placeholder.Type);
                }

            }
            //// Handle ignore placeholders
            //placeHolderTagToTypeCollection.Add(PlaceholderIgnoreA, PlaceHolderType.Ignore);
            //placeHolderTagToTypeCollection.Add(PlaceholderIgnoreB, PlaceHolderType.Ignore);

            //// Handle container placeholders            
            //placeHolderTagToTypeCollection.Add(PlaceholderContainerA, PlaceHolderType.Container);

            //// Handle recursive placeholders            
            //placeHolderTagToTypeCollection.Add(PlaceholderRecursiveA, PlaceHolderType.Recursive);
            //placeHolderTagToTypeCollection.Add(PlaceholderRecursiveB, PlaceHolderType.Recursive);

            //// Handle non recursive placeholders
            //placeHolderTagToTypeCollection.Add(PlaceholderNonRecursiveA, PlaceHolderType.NonRecursive);
            //placeHolderTagToTypeCollection.Add(PlaceholderNonRecursiveB, PlaceHolderType.NonRecursive);
            //placeHolderTagToTypeCollection.Add(PlaceholderNonRecursiveC, PlaceHolderType.NonRecursive);
            //placeHolderTagToTypeCollection.Add(PlaceholderNonRecursiveD, PlaceHolderType.NonRecursive);

            return placeHolderTagToTypeCollection;
        }

        public byte[] GetReportData()
        {
            return GenerateDocument();
        }
        public void Save(string filename)
        {
            File.WriteAllBytes(filename, GetReportData());
        }

    }
}
