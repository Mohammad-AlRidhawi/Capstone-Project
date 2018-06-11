/**
* \class Login.aspx.cs
* \brief A class that represents the Login page.
* \author Johnathan Falbo
* \date 16/04/2015
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace AntiCorruptionSeachEngine.admin
{
    public partial class Login : System.Web.UI.Page
    {
        /**
* Name:         protected void Page_Load(object sender, EventArgs e)  
* Description:  Called on page load. rejects admin if they are logged and redirects to admin default page.
* Arguments:    sender: Object being sent. Not currently used.
*               e:      Any events being sent. Not currently used.
* Return:       Nothing being returned.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["Admin"] != null)
            {
                Response.Redirect("Default.aspx");
            }
        }
        /**
* Name:         protected void Page_Load(object sender, EventArgs e)  
* Description:  Called to authenticate the admin credentials and redirects to admin default page if theyre correct.
* Arguments:    sender: Object being sent. Not currently used.
*               e:      Any events being sent. Not currently used.
* Return:       Nothing being returned.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        protected void Authenticate_Login(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            SearchEntities db = new SearchEntities();

            admin_users adminUsr = new admin_users();
            adminUsr.username = usernameTextBox.Text;
            adminUsr.password = passwordTextBox.Text;

            List<admin_users> adminList = db.admin_users.ToList<admin_users>();
            admin_users tempAdmin;
            admin_users tempAdmin2 = null;

            for(int i = 0;i < adminList.Count;i++)
            {
                tempAdmin = adminList.ElementAt<admin_users>(i);
                if(Encryption.Decrypt(tempAdmin.username) == usernameTextBox.Text)
                {
                    tempAdmin2 = tempAdmin;
                    break;
                }
            }

            if(tempAdmin2 != null)
            {
                if (Encryption.Decrypt(tempAdmin2.password) == Encryption.GetSHA256Hash(passwordTextBox.Text))
                {
                    messageLabel1.Text = "authenticated";
                    AdminObject aO = new AdminObject();
                    aO.SetUserName(Encryption.Decrypt(tempAdmin2.username));
                    Session["Admin"] = aO;
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    messageLabel1.Text = "authentication failed";
                }    
            }

            //string enusr = Encryption.Encrypt(usernameTextBox.Text);
            //string enusr = Encryption.GetSHA256Hash(usernameTextBox.Text);

            //messageLabel1.Text = Server.HtmlEncode(enusr);
        }
    }
}