namespace Creelio.Framework.Web.Extensions
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public static class ListControlExtensions
    {
        public static void DataBind<T>(this ListControl ctrl, IEnumerable<T> list)
        {
            DataBind(ctrl, list, "Text", "Value");
        }

        public static void DataBind<T>(this ListControl ctrl, IEnumerable<T> list, string dataTextField, string dataValueField)
        {
            ctrl.DataSource = list;
            ctrl.DataTextField = dataTextField;
            ctrl.DataValueField = dataValueField;
            ctrl.DataBind();
        }
    }
}