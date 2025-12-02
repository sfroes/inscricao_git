using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;

namespace SMC.GPI.Administrativo.Models
{
    public class LinkViewModel
    {
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool Hide { get; set; }
        public string[] SecurityToken { get; set; }
        public string[] CssClass { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public Dictionary<string, object> Atttributes { get; set; }

        public string AjaxConfirm { get; set; }
        public bool? AjaxIsPost { get; set; }
        public InsertionMode AjaxInsertMode { get; set; }
        public string AjaxLoadingElementId { get; set; }
        public string AjaxOnBegin { get; set; }
        public string AajaxOnComplete { get; set; }
        public string AjaxOnFailure { get; set; }
        public string AjaxOnSuccess { get; set; }
        public string AjaxUpdateTargetId { get; set; }
        public string AjaxFieldsContainerToRequest { get; set; }
        public string AjaxPrepareDataToRequest { get; set; }

        public bool AjaxConfigured { get; set; }

        public void Ajax(string confirm = null, bool? isPost = null, InsertionMode insertionMode = InsertionMode.Replace, string loadingElementId = null, string onBegin = null, string onComplete = null, string onFailure = null, string onSuccess = null, string updateTargetId = null, string fieldsContainerToRequest = null, string prepareDataToRequest = null)
        {
            AjaxConfigured = confirm != null || isPost != null || loadingElementId != null || onBegin != null || onComplete != null || onFailure != null || onSuccess != null || updateTargetId != null || fieldsContainerToRequest != null || prepareDataToRequest != null;

            AjaxConfirm = confirm;
            AjaxIsPost = isPost ?? true;
            AjaxInsertMode = insertionMode;
            AjaxLoadingElementId = loadingElementId;
            AjaxOnBegin = onBegin;
            AajaxOnComplete = onComplete;
            AjaxOnFailure = onFailure;
            AjaxOnSuccess = onSuccess;
            AjaxUpdateTargetId = updateTargetId;
            AjaxFieldsContainerToRequest = fieldsContainerToRequest;
            AjaxPrepareDataToRequest = prepareDataToRequest;
        }

        public string ConfirmTitle { get; set; }
        public string ConfirmMessage { get; set; }

        public bool ConfirmConfigured { get; set; }

        public void Confirm(string title = null, string message = null)
        {
            ConfirmConfigured = title != null && message != null;

            ConfirmTitle = title;
            ConfirmMessage = message;
        }

        public static string BUTTON_LABEL = "Botao_{0}";
        public static string BUTTON_TOOLTIP = "Botao_{0}_Tooltip";
    }
}