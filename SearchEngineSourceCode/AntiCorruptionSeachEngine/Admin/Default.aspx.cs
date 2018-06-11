using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AntiCorruptionSeachEngine.admin
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["Admin"] != null)
            {
                AdminObject aO = (AdminObject)Session["Admin"];
                welcomLabel.Text = "Welcome " + aO.GetUserName() + "!";
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }
    }
}