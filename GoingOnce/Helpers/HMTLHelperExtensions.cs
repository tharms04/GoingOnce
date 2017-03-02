using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace GoingOnce.Helpers
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action.ToLower() == currentAction.ToLower() ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        public static MvcHtmlString TextBoxDisabledFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool disabled, object htmlAttributes = null)
        {
            return TextBoxDisabledFor(htmlHelper, expression, disabled, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString TextBoxDisabledFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool disabled, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (disabled)
                htmlAttributes["disabled"] = "disabled";
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        public static MvcHtmlString TextBoxReadOnlyFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool disabled, object htmlAttributes = null)
        {
            return TextBoxReadOnlyFor(htmlHelper, expression, disabled, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString TextBoxReadOnlyFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool disabled, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (disabled)
                htmlAttributes["readonly"] = "readonly";
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description ?? "";

            return MvcHtmlString.Create(description);
        }

        public static HtmlString GetDisplayName(this Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayName != null)
            {
                return new HtmlString(displayName.Name);
            }

            return new HtmlString(item.ToString());
        }
    }
}
