/**
* \class MainSearchPage.aspx.cs
* \brief This class is intended to search the web starting from a specific page for a word or words input by the user.
* \author Duncan Reitboeck
* \date 09/04/2015
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml;
using System.Globalization;
using System.Numerics;

namespace AntiCorruptionSeachEngine
{
    /* */
    public partial class MainSearchPage : System.Web.UI.Page
    {
        /**
        * Name:         protected void Page_load(object sender, EventArgs e)
        * Description:  Does all functionality necessary when page is first loaded.
        * Arguments:    sender: Object being sent. Not currently used.
        *              e:      Any events being sent. Not currently used.
        * Return:       Nothing being returned.
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SearchEntities db = new SearchEntities();

                /*Get all industries from website.*/
                List<string> industryNames = (from i in db.industries select i.name).ToList();

                /*Populate industry drop down with database entries.*/
                if (industryNames.Count != 0)
                {
                    industryDropDown.DataSource = industryNames;
                    industryDropDown.DataBind();
                }
            }
        }

        /**
       * Name:         protected void SearchButtonClick(object sender, EventArgs e)
       * Description:  Functionality when search button is clicked. Saves search into 				   history and redirects to results page with search word.
       * Arguments:    sender: Object being sent. Not currently used.
       *               e:      Any events being sent. Not currently used.
       * Return:       Nothing being returned.
       * Author:       Duncan Reitboeck
       * Date:         09/04/2015
       * */
        public void SearchButtonClick(object sender, EventArgs e)
        {
            SearchEntities db = new SearchEntities();
            string phrase = searchTextBox.Text;

            /*Save information from search into database for future use.*/
            history saveHistory = new history();
            saveHistory.phrase = phrase;
            saveHistory.frequency = 1;

            var found = (from h in db.histories where h.phrase == phrase select h).FirstOrDefault();

            if (found == null)
                db.histories.Add(saveHistory);

            else
                ++found.frequency;

            db.SaveChanges();

            Session["HeadOffice"] = headOfficeTextBox.Text;
            Session["BusinessIn"] = businessInTextBox.Text;
            Session["Industry"] = industryDropDown.Text;

            Response.Redirect("~/Results.aspx?phrase=" + phrase);
        }

        /*Auto complete search box.*/
        [System.Web.Services.WebMethod]
        public static string[] SearchAutoComplete(string prefixText, int count)
        {
            SearchEntities db = new SearchEntities();

            /*Change to quantity of search not most recent.*/
            return (from h in db.histories
                    where h.phrase.StartsWith(prefixText)
                    orderby h.frequency descending
                    select h.phrase).Take(count).ToArray();
        }
    }
} 