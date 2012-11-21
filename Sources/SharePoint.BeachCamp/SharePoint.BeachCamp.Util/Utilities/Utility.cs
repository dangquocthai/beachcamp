using System;
using System.IO;
using System.Linq;
using System.Web;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;

using System.Text.RegularExpressions;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Reflection;
using SharePoint.BeachCamp.Util.Helpers;
using SharePoint.BeachCamp.Util.Extensions;

namespace SharePoint.BeachCamp.Util.Utilities
{
    public class Utility
    {
        public static void TransferToErrorPage(string message, string linkText, string linkURL)
        {
            if (!string.IsNullOrEmpty(linkURL))
                SPUtility.TransferToErrorPage(message + "\n\n {0} {1}", linkText, linkURL);
            else
                SPUtility.TransferToErrorPage(message);
        }

        public static bool IsAbsoluteUri(string strURL)
        {
            if (string.IsNullOrEmpty(strURL)) return false;
            Uri uriGet;
            Uri.TryCreate(strURL, UriKind.RelativeOrAbsolute, out uriGet);
            return uriGet.IsAbsoluteUri;
        }

        public static string GetRelativeUrl(string fullUrl)
        {
            try
            {
                Uri uri = new Uri(fullUrl);//fullUrl is absoluteUrl  
                string relativeUrl = uri.AbsolutePath;//The Uri property AbsolutePath gives the relativeUrl  

                return relativeUrl;
            }
            catch (Exception ex)
            {
                Utility.LogError("Cannot get relative url from " + fullUrl, "Hypertek.IOffice");
            }
            return null;
        } 

        #region Log
        public static void LogInfo(string errorMessage, BeachCampFeatures category)
        {
            try
            {
                DiagnosticsService myULS = DiagnosticsService.Local;
                if (myULS != null)
                {
                    SPDiagnosticsCategory cat = myULS[category];
                    string format = errorMessage;
                    //myULS.WriteTrace(1, cat, TraceSeverity.Medium, format, myULS.TypeName);
                    myULS.Information(cat, errorMessage);
                }
            }
            catch{}

        }

        public static void LogInfo(string errorMessage, string category)
        {
            //Log(errorMessage, TraceProvider.TraceSeverity.InformationEvent, category);
            BeachCampFeatures e = BeachCampFeatures.BeachCamp;
            try 
	        {	        
		        e = (BeachCampFeatures)Enum.Parse(typeof(BeachCampFeatures), category);
	        }
	        catch{};
            LogInfo(errorMessage, e);
            

            //SPTraceLogger logger = new SPTraceLogger();
            //logger.Write(0, SPTraceLogger.TraceSeverity.InformationEvent, "Hypertek.IOffice TraceProvider", "Hypertek.IOffice", category, errorMessage);

        }
        public static void LogError(string errorMessage, BeachCampFeatures category)
        {
            
            try
            {
                DiagnosticsService myULS = DiagnosticsService.Local;
                if (myULS != null)
                {
                    //SPDiagnosticsCategory cat = myULS[CategoryId.DocuSignService];
                    SPDiagnosticsCategory cat = myULS[category];

                    string format = errorMessage;
                    myULS.WriteTrace(1, cat, TraceSeverity.Unexpected, format, myULS.TypeName);
                }
            }
            catch (Exception)
            {
                
            }

            //Log(errorMessage, TraceProvider.TraceSeverity.CriticalEvent, category);
            //SPTraceLogger logger = new SPTraceLogger();
            //logger.Write(0, SPTraceLogger.TraceSeverity.Exception,"Hypertek.IOffice TraceProvider", "Hypertek.IOffice", category, errorMessage);
        }

        public static void LogError(string errorMessage, string category)
        {
            BeachCampFeatures e = BeachCampFeatures.BeachCamp;
            try
            {
                e = (BeachCampFeatures)Enum.Parse(typeof(BeachCampFeatures), category);
            }
            catch { };

            LogError(errorMessage, e);
            
            //Log(errorMessage, TraceProvider.TraceSeverity.CriticalEvent, category);
            //SPTraceLogger logger = new SPTraceLogger();
            //logger.Write(0, SPTraceLogger.TraceSeverity.Exception,"Hypertek.IOffice TraceProvider", "Hypertek.IOffice", category, errorMessage);
        }

        public static void Log(string message, TraceProvider.TraceSeverity severity, string category)
        {
            //string exeName = Assembly.GetExecutingAssembly().FullName;
            string exeName = "Beachcamp TraceProvider";

            TraceProvider.WriteTrace(0, severity, Guid.NewGuid(), exeName, "Beachcamp", category, message);
        }

        public static void Debug(string message)
        {
            #if DEBUG
            LogError(message, "Corriror .app Debug");
            #else
            
            #endif
        }
        public static void Debug(Exception ex){
            Debug(ex.Message +"\r\n"+ ex.StackTrace);
        }
        #endregion

        public static string BuildKey<T>(BeachCampFeatures featureName)
        {
            return featureName.ToString() + typeof(T).ToString();
        }


        public static SPList GetListFromURL(string strURL)
        {
            return GetListFromURL(strURL, null);
        }

        public static SPListItem GetEmailTemplate(string url, string templateName)
        {
            return GetEmailTemplate(url, templateName, null);
        }

        public static SPListItem GetEmailTemplate(string url, string templateName, SPWeb web)
        {
            SPList list = GetListFromURL(url, web);
            SPQuery query = new SPQuery();

            System.Text.StringBuilder strQuery = new System.Text.StringBuilder();

            strQuery.Append("         <Where>");
            strQuery.Append("               <Eq>");
            strQuery.Append("                   <FieldRef Name='Title' />");
            strQuery.AppendFormat("                   <Value Type='Text'>{0}</Value>", templateName);
            strQuery.Append("               </Eq>");
            strQuery.Append("         </Where>");

            query.Query = strQuery.ToString();

            SPListItemCollection items = list.GetItems(query);

            return items.Cast<SPListItem>().FirstOrDefault();
        }

        public static SPListItem GetEmailTemplate(string url, SPWeb web, string templateName)
        {
            SPList list = GetListFromURL(url, web);
            SPQuery query = new SPQuery();

            System.Text.StringBuilder strQuery = new System.Text.StringBuilder();

            strQuery.Append("         <Where>");
            strQuery.Append("               <Eq>");
            strQuery.Append("                   <FieldRef Name='Title' />");
            strQuery.AppendFormat("                   <Value Type='Text'>{0}</Value>", templateName);
            strQuery.Append("               </Eq>");
            strQuery.Append("         </Where>");

            query.Query = strQuery.ToString();

            SPListItemCollection items = list.GetItems(query);

            return items.Cast<SPListItem>().FirstOrDefault();
        }
        
        public static SPList CopyList(SPList source, SPWeb webDestination, string destinationTitle, bool deleteIfExist)
        {
            SPList destinationList = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(webDestination.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(webDestination.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        destinationList = web.Lists.Cast<SPList>().Where(p => string.Compare(p.Title, destinationTitle) == 0).FirstOrDefault();

                        if (destinationList != null)
                        {
                            if (deleteIfExist)
                                destinationList.Delete();
                            else
                                return;
                            //throw new Exception("Destination list already exist");
                        }

                        try
                        {
                            Guid newListID = web.Lists.Add(destinationTitle, string.Empty, SPListTemplateType.GenericList );
                            destinationList = web.Lists[newListID];

                            destinationList.ContentTypesEnabled = true;
                            destinationList.Update();
                            
                            if (destinationList.ContentTypes.Cast<SPContentType>().FirstOrDefault(ct => ct.Name == "Item") != null
                                && source.ContentTypes.Count > 1)
                            {
                                destinationList.ContentTypes["Item"].Delete();
                                destinationList.Update();
                            }

                            source.CopyAllFieldsToList(destinationList);

                            source.CopyAllContentTypesToList(destinationList);

                            source.CopyAllViewsToList(destinationList);

                        }
                        catch (Exception ex)
                        {

                        }
                        web.AllowUnsafeUpdates = false;
                    }
                }
            }
            );

            return destinationList;
        }

        //public static string ExtractWordContent(SPFile file)
        //{
        //    string ext = Path.GetExtension(file.Name).ToLower();
        //    if (ext == ".doc")
        //    {
        //        String tempFile = Path.GetTempFileName();
        //        using (FileStream fs = File.Create(tempFile))
        //        {
        //            using(var datastream = file.OpenBinaryStream()){
        //                byte[] data =new byte[datastream.Length];
        //                datastream.Read(data, 0, (int)datastream.Length);
        //                fs.Write(data,0, data.Length);
        //            }
        //        }
        //        using (OfficeFileReader objOFR = new OfficeFileReader())
        //        {
        //            string output = "";
        //            objOFR.GetText(tempFile, ref output);
        //            return output;
        //        }
        //    }
        //    if (ext == ".docx")
        //    {
        //        using (DocxFileReader dtt = new DocxFileReader(file.OpenBinaryStream()))
        //        {
        //            return dtt.ExtractText();
        //        }
        //    }

        //    return string.Empty;
        //}

        public static SPListItem GetItemByDocumentUrl(string url)
        {
            SPListItem item = null;
            using (SPSite site = new SPSite(url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPFile file = web.GetFile(url);
                    var match = Regex.Match(url,@"ID=(\d+)");

                    if (file.Exists && match != null && match.Success)
                    {
                        var list = web.Lists[file.ParentFolder.ParentListId];
                        return list.GetItemById(int.Parse(match.Groups[1].Value));
                    }

                    if (file.Exists && file.Item != null)
                        item = file.Item;
                }
            }
            return item;
        }

        public static void LogError(Exception ex)
        {
            LogError(ex.Message + ex.StackTrace, BeachCampFeatures.BeachCamp);
        }

        

        public static SPList GetListFromURL(string strURL, SPWeb externalWeb)
        {
            if (string.IsNullOrEmpty(strURL))
                return null;

            SPSite site = null;
            SPWeb web = null;
            SPList list = null;
            bool disposeSite = false;
            try
            {
                if (Utility.IsAbsoluteUri(strURL))
                    try
                    {
                        site = new SPSite(strURL);
                        web = site.OpenWeb();
                        disposeSite = true;
                    }
                    catch
                    {
                        Utility.LogInfo("Unable to open web from Url : " + strURL + "It isn't SharePoint site or current user don't have permission to open it", "Hypertek.IOffice");
                    }
                else
                {
                    if (externalWeb == null)
                    {
                        site = SPContext.Current.Site;
                        web = site.OpenWeb(HttpUtility.UrlDecode(strURL), false);
                    }
                    
                }

                try
                {
                    if (externalWeb != null)
                    {
                        
                        string url = externalWeb.ServerRelativeUrl.TrimEnd('/') + "/"+strURL.TrimStart('/');
                        list = externalWeb.GetList(url);
                        
                    }
                    else
                        list = web.GetList(web.ServerRelativeUrl.TrimEnd('/') + strURL);
                }
                catch (Exception ex)
                {
                    Utility.LogInfo("Unable to load list from Url : " + strURL, "Hypertek.IOffice");
                }
            }
            catch
            {
                Utility.LogInfo("Couldn't open " + strURL + " as a SharePoint list", "Hypertek.IOffice");
            }
            finally
            {
                if (web != null) web.Dispose();
                if (disposeSite && site != null) site.Dispose();
            }
            return list;
        }

        public static ListTitleViewSelectorMenu FindViewSelectorMenu(Control control)
        {
            ListTitleViewSelectorMenu selectorMenu = null;
            if (control is ListTitleViewSelectorMenu)
            {
                selectorMenu = control as ListTitleViewSelectorMenu;
            }
            else
            {
                foreach (Control child in control.Controls)
                {
                    selectorMenu = FindViewSelectorMenu(child);
                    if (selectorMenu != null)
                    {
                        break;
                    }
                }
            }
            return selectorMenu;
        }

        public static void ShowViewSelectorMenu(Control control)
        {
            ListTitleViewSelectorMenu selectorMenu = FindViewSelectorMenu(control);

            if (selectorMenu != null)
            {
                typeof(ListTitleViewSelectorMenu)
               .GetField("m_wpSingleInit", BindingFlags.Instance | BindingFlags.NonPublic)
               .SetValue(selectorMenu, true);
                typeof(ListTitleViewSelectorMenu)
                   .GetField("m_wpSingle", BindingFlags.Instance | BindingFlags.NonPublic)
                   .SetValue(selectorMenu, true);
            }
        }
    }
}
