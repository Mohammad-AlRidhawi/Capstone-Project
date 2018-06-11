/**
* \class MasterPage.Master.cs
* \brief Master page of the project, allows same look and feel through out the project
* \author Mohammad Al Ridhawi
* \date 08/04/2015
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AntiCorruptionSeachEngine
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        //Perform any necessary actions upon loading page.
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //adrotator(did you know) done by David Stoddard, allows for information to display on the page
        protected void AdRotator1_AdCreated(object sender, AdCreatedEventArgs e)
        {
            alternateText.Text = e.AlternateText;
            hyperLink.NavigateUrl = e.NavigateUrl;
        }
    }
}