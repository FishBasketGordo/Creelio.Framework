namespace Creelio.Framework.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Creelio.Framework.Extensions;
    using Creelio.Framework.Interfaces;

    public static class ViewUtility
    {
        public static string RenderView(string path)
        {
            IServiceMetadata metadata;
            IPagingDataProvider pagingData;
            return RenderView(path, true, out metadata, out pagingData);
        }

        public static string RenderView(string path, out IServiceMetadata metadata, out IPagingDataProvider pagingData)
        {
            return RenderView(path, true, out metadata, out pagingData);
        }

        public static string RenderView(string path, bool populateFromQueryString)
        {
            IServiceMetadata metadata;
            IPagingDataProvider pagingData;
            return RenderView(path, true, out metadata, out pagingData);
        }

        public static string RenderView(string path, bool populateFromQueryString, out IServiceMetadata metadata, out IPagingDataProvider pagingData)
        {
            Page page = new Page();
            UserControl ctrl = (UserControl)page.LoadControl(path);

            return RenderControlInternal(page, ctrl, populateFromQueryString, out metadata, out pagingData);
        }

        public static string RenderControl(Control ctrl)
        {
            Page page = new Page();
            IServiceMetadata metadata;
            IPagingDataProvider pagingData;
            return RenderControlInternal(page, ctrl, false, out metadata, out pagingData);
        }

        public static void DataBindLayoutTemplate(ListView listView)
        {
            listView.ThrowIfNull(() => new ArgumentNullException("listView"));

            // Create a databound layout template.
            var template = new Control();
            listView.LayoutTemplate.InstantiateIn(template);
            template.DataBind();

            // Remove the existing, non-databound layout template.
            listView.Controls.RemoveAt(0);

            // Add the databound layout template.
            listView.Controls.Add(template);
        }

        private static string RenderControlInternal(
            Page page, 
            Control ctrl, 
            bool populateFromQueryString,
            out IServiceMetadata metadata, 
            out IPagingDataProvider pagingData)
        {
            if (populateFromQueryString)
            {
                Populate(ctrl);
            }

            using (StringWriter sw = new StringWriter())
            {
                page.Controls.Add(ctrl);
                HttpContext.Current.Server.Execute(page, sw, false);

                metadata = GetServiceMetadata(ctrl);
                pagingData = GetPagingData(ctrl);

                return sw.ToString();
            }
        }

        private static IServiceMetadata GetServiceMetadata(Control ctrl)
        {
            IServiceMetadata metadata = ctrl is IServiceMetadataProvider
                                      ? (ctrl as IServiceMetadataProvider).Metadata
                                      : null;
            return metadata;
        }

        private static IPagingDataProvider GetPagingData(Control ctrl)
        {
            var pagingDataProxy = new PagingDataProxy();

            if (ctrl is IPagingDataProvider)
            {
                pagingDataProxy.DataPage = (ctrl as IPagingDataProvider).DataPage;
                pagingDataProxy.TotalDataPages = (ctrl as IPagingDataProvider).TotalDataPages;
                pagingDataProxy.TotalRecords = (ctrl as IPagingDataProvider).TotalRecords;
                pagingDataProxy.RecordsPerPage = (ctrl as IPagingDataProvider).RecordsPerPage;
            }

            return pagingDataProxy;
        }

        private static void Populate(Control ctrl)
        {
            NameValueCollection qs = HttpContext.Current.Request.QueryString;

            if (ctrl is IQueryMappableView)
            {
                (ctrl as IQueryMappableView).Map(qs);
            }
            else
            {
                PopulateByReflection(ctrl, qs);
            }
        }

        private static void PopulateByReflection(Control ctrl, NameValueCollection qs)
        {
            List<MemberInfo> members = GetMembers(ctrl);

            foreach (string key in qs)
            {
                MemberInfo member = (from cm in members
                                     where string.Compare(cm.Name, key, true) == 0
                                     select cm).FirstOrDefault();

                if (member != null)
                {
                    if (member is PropertyInfo)
                    {
                        PropertyInfo property = member as PropertyInfo;
                        property.SetValue(ctrl, ConvertValue(qs[key], property.PropertyType), null);
                    }
                    else if (member is FieldInfo)
                    {
                        FieldInfo field = member as FieldInfo;
                        field.SetValue(ctrl, ConvertValue(qs[key], field.FieldType));
                    }
                }
            }
        }

        private static List<MemberInfo> GetMembers(Control ctrl)
        {
            List<MemberInfo> members = new List<MemberInfo>();
            Type ctrlType = ctrl.GetType();

            while (ctrlType != typeof(UserControl))
            {
                members.AddRange(ctrlType.GetProperties());
                members.AddRange(ctrlType.GetFields());
                ctrlType = ctrlType.BaseType;
            }

            return members;
        }

        private static object ConvertValue(string valueString, Type convertTo)
        {
            if (convertTo == typeof(string))
            {
                return valueString;
            }
            else
            {
                return valueString;
            }
        }
    }
}