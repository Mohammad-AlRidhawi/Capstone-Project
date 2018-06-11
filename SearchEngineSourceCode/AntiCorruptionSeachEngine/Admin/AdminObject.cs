/**
* \class AdminObject.cs
* \brief A class that represents an Admin object.
* \author Johnathan Falbo
* \date 16/04/2015
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntiCorruptionSeachEngine.admin
{
    public class AdminObject
    {
        private String _username;/*admins username*/
        private DateTime _loginTimeStamp;/*admins login time*/
        private DateTime _lastLoginTimeStamp;/*admins last login time*/

        /**
* Name:         public String GetUserName()  
* Description:  used to get the admins user name
* Return:       String username.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public String GetUserName()
        {
            return _username;
        }

        /**
* Name:         public void SetUserName(String username)
* Description:  sets the admins username
* Arguments:    username: admin user name to set too
* Return:       Nothing being returned.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public void SetUserName(String username)
        {
            _username = username;
        }

        /**
* Name:         public DateTime GetLoginTimeStamp()
* Description:  Called to get the last login date time
* Return:       DateTime of login.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public DateTime GetLoginTimeStamp()
        {
            return _loginTimeStamp;
        }

        /**
* Name:         public void SetLoginTimeStamp(DateTime loginTimeStamp)
* Description:  Called to set the login DateTime.
* Arguments:    loginTimeStamp: login DateTime
* Return:       Nothing being returned.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public void SetLoginTimeStamp(DateTime loginTimeStamp)
        {
            _loginTimeStamp = loginTimeStamp;
        }
        /**
* Name:         public DateTime GetLastLoginTimeStamp() 
* Description:  Called to get admins last login.
* Return:       DateTime: last login.    
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public DateTime GetLastLoginTimeStamp()
        {
            return _lastLoginTimeStamp;
        }
        /**
* Name:         public void SetLastLoginTimeStamp(DateTime lastLoginTimeStamp)
* Description:  Called to set the admins last login.
* Arguments:    lastLoginTimeStamp: admins last login.
* Return:       Nothing being returned.      
* Author:       Johnathan Falbo
* Date:         16/04/2015
* */
        public void SetLastLoginTimeStamp(DateTime lastLoginTimeStamp)
        {
            _lastLoginTimeStamp = lastLoginTimeStamp;
        }
    }
}