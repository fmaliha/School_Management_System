﻿/********************************************************************************
Copyright (C) Panguchi Soft. www.panguchi.com

This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. 
If a copy of the MPL was not distributed  with this file, You can obtain one at 
http://mozilla.org/MPL/2.0/.
***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;



namespace InovexiaEBS
{
    public class BasePageClass : System.Web.UI.Page
    {
        /// <summary>
        /// Use this parameter on the Page_Init event of member pages.
        /// This parameter ensures that the user is not redirected to the login page 
        /// even when the user is not logged in.
        /// </summary>
        public bool NoLogOn { get; set; }

        /// <summary>
        /// Since we save the menu on the database, this parameter is only used 
        /// when there is no associated record of this page's url or path in the menu table.
        /// Use this to override or fake the page's url or path. This forces navigation menus 
        /// on the left hand side to be displayed in regards with the specified path.
        /// </summary>
        public string OverridePath { get; set; }











        protected override void OnLoad(EventArgs e)
        {

            //string pinnumber = Context.Session["pin_number"].ToString();
            if (Context.Session["pin_number"] != null)
            {

                if (string.IsNullOrWhiteSpace(OverridePath))
                {
                    OverridePath = this.Page.Request.Url.AbsolutePath;
                }

                string PinNo = Context.Session["pin_number"].ToString();

                string PageName = Conversion.GetRelativePath(this.OverridePath);

                int chk = MenuHelper.PageAuthenticationCheck(PinNo, PageName);

                if (chk == 1)
                {

                    Literal menuLiteral = ((Literal)PageUtility.FindControlIterative(this.Master, "ContentMenuLiteral"));

                    if (menuLiteral != null)
                    {
                        string pinnumber = Context.Session["pin_number"].ToString();

                        string menu = MenuHelper.GetContentPageMenu(this.Page, this.OverridePath, pinnumber);
                        menuLiteral.Text = "";
                    }

                    base.OnLoad(e);


                }

                else
                {


                    Response.Redirect("~/Dashboard.aspx", true);


                }
                base.OnLoad(e);
            }

            else

            {
                Response.Redirect("~/Login.aspx");
            
            }
        }


        //rezaul------------------------



















        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        }

        protected override void InitializeCulture()
        {
            SetCulture();
            base.InitializeCulture();
        }

        protected override void OnInit(EventArgs e)
        {
            ////if (!IsPostBack)
            ////{
            ////    if (Request.IsAuthenticated)
            ////    {
            ////        if (Context.Session == null)
            ////        {
            ////            SetSession();
            ////        }
            ////        else
            ////        {
            ////            if (Context.Session["UserId"] == null)
            ////            {
            ////                SetSession();
            ////            }
            ////        }
            ////    }
            ////    else
            ////    {
            ////        if (!this.NoLogOn)
            ////        {
            ////            RequestLogOnPage();
            ////        }
            ////    }
            //}

            base.OnInit(e);
        }

        private static void SetCulture()
        {
            if (HttpContext.Current.Session["Culture"] == null)
            {
                return;
            }

            string cultureName = HttpContext.Current.Session["Culture"].ToString();
            CultureInfo culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }


        private void SetSession()
        {
            //MixERP.Net.BusinessLayer.Security.User.SetSession(this.Page, User.Identity.Name);
        }

        public static void RequestLogOnPage()
        {
            FormsAuthentication.SignOut();

            foreach (var cookie in HttpContext.Current.Request.Cookies.AllKeys)
            {
                HttpContext.Current.Request.Cookies.Remove(cookie);
            }

            string currentPage = HttpContext.Current.Request.Url.AbsolutePath;
            string loginUrl = (HttpContext.Current.Handler as Page).ResolveUrl(FormsAuthentication.LoginUrl);

            if (currentPage != loginUrl)
            {
                FormsAuthentication.RedirectToLoginPage(currentPage);
            }
        }
    }
}
