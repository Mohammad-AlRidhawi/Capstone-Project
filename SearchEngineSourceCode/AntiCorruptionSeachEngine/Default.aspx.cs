/**
* \class Default.aspx.cs
* \brief A class that represents the default page that redirects the user to MainSearchPage.aspx
* \author Johnathan Falbo
* \date 16/04/2015
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AntiCorruptionSeachEngine
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("MainSearchPage.aspx");
        }
    }
}