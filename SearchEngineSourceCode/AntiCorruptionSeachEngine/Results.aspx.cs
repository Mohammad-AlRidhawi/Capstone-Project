/**
* \class Results.aspx.cs
* \brief A class that represents the results page.
* \author Duncan Reitboeck 
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
    public partial class Results : System.Web.UI.Page
    {
        /*Number of entries per page.*/
        const int perPage = 20;

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
            SearchEntities db = new SearchEntities();

            if (!Page.IsPostBack)
            {
                if (Request["phrase"] != null)
                {
                    /*Input searched word in to results.*/
                    string searchedWord = Request["phrase"];
                    searchforLabel.Text = searchedWord;

                    /*Head office label.*/
                    if (Session["HeadOffice"] != null)
                    {
                        officeLabel.Text = Convert.ToString(Session["HeadOffice"]);
                        officeLabel.Visible = true;
                    }

                    /*Business label.*/
                    if (Session["BusinessIn"] != null)
                    {
                        businessInLabel.Text = Convert.ToString(Session["BusinessIn"]);
                        businessInLabel.Visible = true;
                    }

                    /*Industry label.*/
                    if (Session["Industry"] != null)
                    {
                        industryLabel.Text = Convert.ToString(Session["Industry"]);
                        industryLabel.Visible = true;
                    }

                    /*List of each website for display.*/
                    List<website> websiteList = new List<website>();

                    /*Variable containing the page the user is on.*/
                    int pageNum = 0;

                    Session["Websites"] = websiteList;
                    Session["Pages"] = pageNum;
                    SeperateWords(searchedWord);
                }
            }
        }

        /**
        * Name:         protected void SeperateWords(string phrase)      
        * Description:  Seperates words in to a list when user enters multiple words.
        * Arguments:    phrase: The words entered in to search by user.              
        * Return:       Nothing being returned.
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void SeperateWords(string phrase)
        {
            List<string> allSearchWords = new List<string>();

            phrase = phrase.ToLower();

            /*Loop until all words are collected.*/
            while (true)
            {
                /*Check for multi words and check user did not put space character at end of input.*/
                if (phrase.Contains(" ") && phrase != " ")
                {
                    /*Add first word to database.*/
                    allSearchWords.Add(phrase.Substring(0, (phrase.IndexOf(" "))));

                    /*Cut space from input.*/
                    phrase = phrase.Substring(phrase.IndexOf(" ") + 1);
                }

                else if (phrase != "")
                {
                    /*Add last word then exit loop.*/
                    allSearchWords.Add(phrase);
                    break;
                }
            }
            getResults(allSearchWords);
        }

        /**
        * Name:         protected void getResults(List<string> searchWords)    
        * Description:  This function makes a list of results then saves the list to session data.
        * Arguments:    searchWords: A list of all words entered by the user.   
        * Return:       Nothing being returned.
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void getResults(List<string> searchWords)
        {
            /*Flag to determine if web list is already created or not.*/
            Boolean loopFlag = false;

            /*Flag to determine if web page contains word during loop.*/
            Boolean foundWord = false;

            SearchEntities db = new SearchEntities();
            List<website> webList = (List<website>)Session["Websites"];

            foreach (string searchWord in searchWords)
            {
                /*First word in search. Add websites that contain the word to website list.*/
                if (loopFlag == false)
                {
                    /*Get all websites that with matching keyword to phrase.*/
                    var webIds = (from web
                                  in db.link_website_words
                                  where web.word_id ==
                                     (from w in db.words
                                      where (w.phrase.Equals(searchWord))
                                      select w.id).FirstOrDefault()
                                  select web);

                    /*Take the ID of each website fitting credentials.*/
                    foreach (link_website_words id in webIds)
                    {
                        website tempSite = (from w in db.websites where (w.id == id.website_id) select w).FirstOrDefault();

                        /*Fix nulls in database before trying to capture words.*/
                        /*If there is nothing in info enter the anchor again.*/
                        if (tempSite.title == null)
                        {
                            tempSite.title = tempSite.anchor;
                        }

                        /*If there is nothing in info enter the anchor again.*/
                        if (tempSite.info == null)
                        {
                            tempSite.info = tempSite.title;
                        }

                        /*If the stored information from tags contains the searched word.*/
                        if (tempSite.info.Contains(searchWord))
                        {
                            /*If there is enough room before and after take the predetermined 150 starting 50 prior to the found word.*/
                            if (tempSite.info.IndexOf(searchWord) > 50 &&
                                (tempSite.info.IndexOf(searchWord) + 100 < (tempSite.info.Length)))
                            {
                                tempSite.info = tempSite.info.Substring((tempSite.info.IndexOf(searchWord) - 50), (150));
                            }

                            /*If there is enough room before but not after take before the word and stop at the end of the string.*/
                            else if (tempSite.info.IndexOf(searchWord) > 50)
                            {
                                tempSite.info = tempSite.info.Substring((tempSite.info.IndexOf(searchWord) - 50));
                            }

                            /*If there is enough room after but not before take from the word until 150 is up.*/
                            else if (tempSite.info.IndexOf(searchWord) + 150 < (tempSite.info.Length))
                            {
                                tempSite.info = tempSite.info.Substring(0, (150));
                            }
                        }

                        /*If searched word is found but not in the text of the web page.*/
                        else
                        {
                            /*Check to make sure there are at least enough characters to make up the result string.*/
                            if (tempSite.info.Length > 150)
                                tempSite.info = tempSite.info.Substring(0, 150);
                        }
                        /*Add website to website list.*/
                        tempSite.rank = 1;
                        tempSite.word_rank = id.rank;
                        webList.Add(tempSite);
                    }
                    loopFlag = true;
                }

                /*Second or higher word in search. Remove websites from website list that don't match.*/
                else
                {
                    /*Get all websites that with matching keyword to phrase.*/
                    var webIds = (from web
                                in db.link_website_words
                                  where web.word_id ==
                                     (from w in db.words
                                      where (w.phrase.Equals(searchWord))
                                      select w.id).FirstOrDefault()
                                  select web);

                    /*Check each website in current list for non-matching websites.*/
                    foreach (website currentWebsite in webList)
                    {
                        /*Reset flag for next website in list.*/
                        foundWord = false;

                        /*Check each website id in new list.*/
                        foreach (link_website_words id in webIds)
                        {
                            /*If website in new list is found in old website list set flag and exit loop.*/
                            if (id.website_id == currentWebsite.id)
                            {
                                currentWebsite.word_rank += id.rank;
                                foundWord = true;
                                break;
                            }
                        }

                        /*If found stop loop.*/
                        if (foundWord == true)
                        {
                            break;
                        }
                    }
                }

                /*If no results are found show message.*/
                if (webList.Count() == 0)
                {
                    notFoundLabel.Text = "No results matching your search.";
                }
            }
            Session["Websites"] = webList;
            BindData();
        }

        /**
        * Name:         protected void NextClick(object sender, EventArgs e)  
        * Description:  When user selects next button new results are displayed.  
        * Arguments:    sender: Object being sent. Not currently used.
        *               e:      Any events being sent. Not currently used.
        * Return:       Nothing being returned.  
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void NextClick(object sender, EventArgs e)
        {
            /*Increase global variable containing page number.*/
            int page = (int)Session["Pages"];
            ++page;
            Session["Pages"] = page;
            BindData();
        }

        /**
        * Name:         protected void PrevClick(object sender, EventArgs e) 
        * Description:  When user selects previous button new results are displayed.
        * Arguments:    sender: Object being sent. Not currently used.
        *               e:      Any events being sent. Not currently used.
        * Return:       Nothing being returned.      
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        **/
        protected void PrevClick(object sender, EventArgs e)
        {
            /*Decrease global variable containing page number.*/
            int page = (int)Session["Pages"];
            --page;
            Session["Pages"] = page;
            BindData();
        }

        /**
        * Name:         protected void CalcPage()  
        * Description:  Calculates which buttons (previous, next or both) are visible depending on the current page and the amount of results.
        * Arguments:    No arguments sent.  
        * Return:       Nothing being returned.
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void CalcPage()
        {
            List<website> websiteList = (List<website>)Session["Websites"];
            int pageNum = (int)Session["Pages"];
            /*Calculate pages by divided total entries by the amount per page then take one extra page if there are any left over.*/
            int pages = (websiteList.Count() / perPage);

            /*Add an extra page for leftover entries.*/
            if (websiteList.Count() % perPage != 0)
                ++pages;

            /*If there is only one page previous and next are invisible.*/
            if (pages <= 1)
            {
                prevButton.Visible = false;
                nextButton.Visible = false;
            }

            /*If user is on first page and there is more than one page make previous invisble and next visible.*/
            else if (pageNum == 0)
            {
                prevButton.Visible = false;
                nextButton.Visible = true;
            }

            /*If user is on the last page set next to invisible and previous to visible.*/
            else if (pageNum == (pages - 1))
            {
                prevButton.Visible = true;
                nextButton.Visible = false;
            }

            /*If user is in a middle page set both previous and next to visible.*/
            else
            {
                prevButton.Visible = true;
                nextButton.Visible = true;
            }
        }

        /**
        * Name:         protected void BindData       
        * Description:  Binds the data saved in the session to the repeater.
        * Arguments:    No arguments sent.
        * Return:       Nothing being returned.
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void BindData()
        {
            int pageNum = (int)Session["Pages"];
            List<website> webList = (List<website>)Session["Websites"];

            webList = AdvancedSearch(webList);

            /*Check for page click.*/
            CalcPage();
            /*Display entries depending on what page the user is on and pre determined amount per page.*/
            results.DataSource = webList.Skip(pageNum * perPage).Take(perPage);
            results.DataBind();
        }

        /**
        * Name:         protected List<website> AdvancedSearch(List<website> websiteList)
        * Description:  Ranks websites based on cost, industry and countries that match information stored in the database
        *               and input from the user. 
        * Arguments:    websiteList: List of websites matching the users search. Currently only matching words entered by user
        *               and not ordered.
        * Return:       Returns a list of websites  
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        **/
        protected List<website> AdvancedSearch(List<website> websiteList)
        {
            SearchEntities db = new SearchEntities();

            /*Search each website in list.*/
            foreach (website currentSite in websiteList)
            {
                currentSite.rank = 0;

                /*Compare session information for head office and country of website in database.*/
                /*Session information stored from search credentials.*/
                if (Convert.ToString(Session["HeadOffice"]) != null)
                {
                    string tempCountry = Convert.ToString(Session["HeadOffice"]);
                    var countryId = (from c in db.countries where (c.name == tempCountry) select c.id).FirstOrDefault();

                    foreach (link_country_website country in db.link_country_website)
                    {
                        if ((country.website_id == currentSite.id) && (country.country_id == countryId))
                        {
                            currentSite.rank++;
                            break;
                        }
                    }
                }

                /*Compare session information for business in and country of website in database.*/
                /*Session information stored from search credentials.*/
                if (Convert.ToString(Session["BusinessIn"]) != null)
                {
                    string tempCountry = Convert.ToString(Session["BusinessIn"]);
                    var countryId = (from c in db.countries where (c.name == tempCountry) select c.id).FirstOrDefault();

                    foreach (link_country_website country in db.link_country_website)
                    {
                        if ((country.website_id == currentSite.id) && (country.country_id == countryId))
                        {
                            currentSite.rank++;
                            break;
                        }
                    }
                }

                /*Compare session information for industry and industry of website in database.*/
                /*Session information stored from search credentials.*/
                if (Convert.ToString(Session["Industry"]) != null)
                {
                    string tempIndustry = Convert.ToString(Session["Industry"]);
                    var indId = (from i in db.industries where (i.name == tempIndustry) select i.id).FirstOrDefault();

                    foreach (link_industry_website industryTemp in db.link_industry_website)
                    {
                        if ((industryTemp.website_id == currentSite.id) && (industryTemp.industry_id == indId))
                        {
                            currentSite.rank++;
                            break;
                        }
                    }
                }

                /*Increase rank of non profit websites to a higher value to ensure they display first in list.*/
                if (currentSite.cost != null)
                {
                    if (currentSite.cost == false)
                    {
                        currentSite.rank += 10;
                    }
                }
            }

            /*Order list by the amount of search credentials found. List is then ordered by the word ranking.*/
            websiteList = (from w in websiteList orderby w.rank descending, w.word_rank descending select w).ToList();
            return websiteList;
        }

        /**
        * Name:         protected void ReturnSearchClick(object sender, EventArgs e)       
        * Description:  Returns to main search page.
        * Arguments:    sender: Object being sent. Not currently used.
        *               e:      Any events being sent. Not currently used.
        * Return:       Nothing being returned.       
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        **/
        protected void ReturnSearchClick(object sender, EventArgs e)
        {
            Response.Redirect("~/MainSearchPage.aspx");        
        }
    }
}