/**
* \class EditDidYouKnow.aspx.cs
* \brief A class that represents an Admin object.
* \author David Stoddard
* \date 16/04/2015
*/
using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.IO;
using System.Xml.Schema;

namespace AntiCorruptionSeachEngine.admin
{
    public partial class EditDidYouKnow : System.Web.UI.Page
    {
        /*Global variables that point to various key locations for the didyouknow banner to be updated,deleted or added*/
        private const string strXMLFileName = "../App_Data/Advertisement.ads"; // Change path here.
        private const string strXSDFileName = "../App_Data/Advertisements.xsd"; // Change path here.
        private const string strImagesPath = "../Images/";
        string[] extensions = { ".jpg", ".tiff", ".bmp", ".png", ".gif", ".jpeg" };

        /**
        * Name:         protected void Page_load(object sender, EventArgs e)
        * Description:  Does all functionality necessary when page is first loaded and checking if the user is an Admin.
        * Arguments:    sender: Object being sent. Not currently used.
        *              e:      Any events being sent. Not currently used.
        * Return:       Nothing being returned.
        * Author:       David Stoddard
        * Date:         16/04/2015
        * */
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Admin"] != null)
            {
                AdminObject aO = (AdminObject)Session["Admin"];
            }
            else
            {
                Response.Redirect("login.aspx");
            }
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }

        /**
        * Name:         private void BindGrid()
        * Description:  Reads the current dataset, retrieves the list of data items and binds that data to the gridview. 
        * Arguments:    None.
        * Return:       Nothing being returned.
        * Author:       David Stoddard
        * Date:         16/04/2015
        * */
        private void BindGrid()
        {
            DataSet ds = ReadDataSet();
            if (ds != null && ds.HasChanges())
            {
                gridView1.DataSource = ds;
                gridView1.DataBind();
            }
            else
            {
                gridView1.DataBind();
            }

        }

        /**
       * Name:         private DataSet WriteXml(DataSet ds)
       * Description:  Writes the default XML which is stated at App_Data/Advertisements.xsd 
       * Arguments:    ds: Passing dataset.
       * Return:       ds: Dataset is returned.
       * Author:       David Stoddard
       * Date:         16/04/2015
       * */
        private DataSet WriteXml(DataSet ds)
        {
            ds.ReadXmlSchema(Server.MapPath(strXSDFileName));

            foreach (DataTable t in ds.Tables)
            {
                var row = t.NewRow();
                t.Rows.Add(row);
            }

            return ds;
        }

        /**
       * Name:         private DataSet ReadDataSet()
       * Description:  Does all functionality necessary when page is first loaded.
       * Arguments:    ds: Passing dataset and writes the default XML which is stated at App_Data/Advertisements.xsd
       * Return:       ds: Dataset is returned.
       * Author:       David Stoddard
       * Date:         16/04/2015
       * */
        private DataSet ReadDataSet()
        {
            DataSet ds = new DataSet();

            foreach (DataTable dataTable in ds.Tables) //Improves performance when reading large XML files.
                dataTable.BeginLoadData();

            ds.ReadXml(Server.MapPath(strXMLFileName));

            foreach (DataTable dataTable in ds.Tables)
                dataTable.EndLoadData();

            return ds;
        }

        /**
       * Name:         private void SaveDataSet(DataSet ds)
       * Description:  If data is present in the dataset write the xml to Advertisments.ads
       * Arguments:    ds: Passing dataset
       * Return:       Nothing is returned.
       * Author:       David Stoddard
       * Date:         16/04/2015
       * */
        private void SaveDataSet(DataSet ds)
        {
            if (ds != null)
                ds.WriteXml(Server.MapPath(strXMLFileName));
        }

        /**
       * Name:         private void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
       * Description:  If data is present in the dataset write the xml to Advertisments.ads
       * Arguments:    sender: Not used
       *               e: GridViewPageEventArgs is used to index the new rows and/or columns. (Rows in this case)
       * Return:       Nothing is returned.
       * Author:       David Stoddard
       * Date:         16/04/2015
       * */
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        /**
      * Name:         protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
      * Description:  Deletes rows from the grid which is triggered by the delete button.
      * Arguments:    sender: Not used.
      *               e: Not used.
      * Return:       Nothing is returned.
      * Author:       David Stoddard
      * Date:         16/04/2015
      * */
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataSet ds = ReadDataSet();
            
            if (ds.Tables[0].Rows.Count > 1)
            {
                ds.Tables[0].Rows[e.RowIndex].Delete();
            }

            ds.AcceptChanges();
            SaveDataSet(ds);
            BindGrid();
        }


        /**
          * Name:         protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
          * Description:  Checks for the row index of the current item being edited.
          * Arguments:    sender: Not used.
          *               e: Not used.
          * Return:       Nothing is returned.
          * Author:       David Stoddard
          * Date:         16/04/2015
          * */
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView1.EditIndex = e.NewEditIndex;
            BindGrid(); // without binding it will clear fields before editing.
        }

        /**
          * Name:         protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
          * Description:  While updating the gridview data for an item is saved to the textboxes.
          * Arguments:    sender: Not used.
          *               e: Not used.
          * Return:       Nothing is returned.
          * Author:       David Stoddard
          * Date:         16/04/2015
          * */
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int index = gridView1.EditIndex;
            GridViewRow row = gridView1.Rows[index];
            DataSet ds = ReadDataSet();
            
            string strImageUrl = strImagesPath + ((DropDownList)row.FindControl("editImageUrl")).Text.Trim();
            string strNavigateUrl = ((TextBox)row.FindControl("editNavigateUrl")).Text.Trim();
            string strAlternateText = ((TextBox)row.FindControl("editAlternateText")).Text.Trim();
            string strImpressions = ((TextBox)row.FindControl("editImpressions")).Text.Trim();
            string strKeyword = ((TextBox)row.FindControl("editKeyword")).Text.Trim();

            gridView1.EditIndex = -1;
            BindGrid();

            int i = gridView1.Rows[e.RowIndex].DataItemIndex;
            ds.Tables[0].Rows[i][0] = strImageUrl; //XML tables
            ds.Tables[0].Rows[i][1] = strNavigateUrl;
            ds.Tables[0].Rows[i][2] = strAlternateText;
            ds.Tables[0].Rows[i][3] = strImpressions;
            ds.Tables[0].Rows[i][4] = strKeyword;

            ds.AcceptChanges();
            SaveDataSet(ds);
            BindGrid();
        }

        /**
         * Name:         protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
         * Description:  Cancels the edit and no changes are applied.
         * Arguments:    sender: Not used.
         *               e: Not used.
         * Return:       Nothing is returned.
         * Author:       David Stoddard
         * Date:         16/04/2015
         * */
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridView1.EditIndex = -1;
            BindGrid();
        }

        /**
         * Name:         protected void buttonAddRow_Click(object sender, GridViewCancelEditEventArgs e)
         * Description:  Adds a row by checking the data that the user inputed and applies it to the XML file.
         * Arguments:    sender: Not used.
         *               e: Not used.
         * Return:       Nothing is returned.
         * Author:       David Stoddard
         * Date:         16/04/2015
         **/
        protected void buttonAddRow_Click(object sender, EventArgs e)
        {
            DataSet ds = ReadDataSet();

            if (ds.Tables.Count == 0)
            {
                WriteXml(ds);
            }

            DataRow dr = ds.Tables[0].NewRow();
            string strImageUrl = strImagesPath + dropDownListImageUrl.SelectedItem.Text.Trim();
            string strNavigateUrl = textboxNavigateUrl.Text.Trim();
            string strAlternateText = textboxAlternateText.Text.Trim();

            string strImpressions = textboxImpressions.Text.Trim();
            string strKeyword = textboxKeyword.Text.Trim();

            dr["ImageUrl"] = strImageUrl;
            dr["NavigateUrl"] = strNavigateUrl;
            dr["AlternateText"] = strAlternateText;
            dr["Impressions"] = MyExtensions.ToNullableInt32(strImpressions);
            dr["Keyword"] = strKeyword;

            ds.Tables[0].Rows.Add(dr);

            ds.AcceptChanges();
            SaveDataSet(ds);
            BindGrid();
        }

        /**
         * Name:         protected void AjaxFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
         * Description:  This function allows the image uploader to succesfully save to the Images folder.
         * Arguments:    sender: Not used.
         *               e: Gets the filename of the selected image.
         * Return:       Nothing is returned.
         * Author:       David Stoddard
         * Date:         16/04/2015
         **/
        protected void AjaxFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            string filename = e.FileName;
            string strDestPath = Server.MapPath(strImagesPath);
            AjaxFileUpload1.SaveAs(@strDestPath + filename);
        }

        /**
         * Name:         protected void gridView1_RowDataBound(object sender, GridViewRowEventArgs e)
         * Description:  This function allows the image uploader to succesfully save to the Images folder.
         * Arguments:    sender: Not used.
         *               e: Gets the row type to find the image located in the dropdownlists. (Either images dropdownlist from Add or Edit)
         * Return:       Nothing is returned.
         * Author:       David Stoddard
         * Date:         16/04/2015
         **/
        protected void gridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0) 
                {
                    DropDownList editDropDownList = (DropDownList)e.Row.FindControl("editImageUrl");
                    DirectoryInfo di = new DirectoryInfo(Server.MapPath(strImagesPath));
                    FileInfo[] fi = di.EnumerateFiles().Where(f => extensions.Contains(f.Extension)).ToArray();
                    editDropDownList.DataSource = fi;
                    editDropDownList.DataBind();
                }

                if ((e.Row.RowState & DataControlRowState.Alternate) > 0)
                {
                    DirectoryInfo di = new DirectoryInfo(Server.MapPath(strImagesPath));
                    FileInfo[] fi = di.EnumerateFiles().Where(f => extensions.Contains(f.Extension)).ToArray();
                    dropDownListImageUrl.DataSource = fi;
                    dropDownListImageUrl.DataBind();
                }
            }
        }


    }
    
    public static class MyExtensions
    {
        /**
         * Name:         public static int? ToNullableInt32(this string s)
         * Description:  Optional function to parse data to a acceptable integer format. Example: Strings to add impressions is converted to 0.
         * Arguments:    s: String passed
         * Return:       0 is returned.
         * Author:       David Stoddard
         * Date:         16/04/2015
         **/
        public static int? ToNullableInt32(this string s)
        {
            int i;
            if (Int32.TryParse(s, out i)) return i;
            return 0;
        }
    }
}