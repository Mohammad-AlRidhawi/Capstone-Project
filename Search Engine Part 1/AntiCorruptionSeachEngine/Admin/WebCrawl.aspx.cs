/**
* \class WebCrawl.aspx.cs
* \brief A class that represents the webcrawler
* \author Duncan Reitboeck 
* \date 16/04/2015
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AntiCorruptionSeachEngine.admin
{
    public partial class WebCrawl : System.Web.UI.Page
    {
        /* CurrentCrawlNumber is the current entry in the database. 
         * CurrentWordId is the id for linking websites and words.
         * Duplicate for if URL is already stored in database.
         * TempHtml for storing html of pages.*/
        int currentCrawlPage; /*Change to long for future larger databases.*/
        int currentWordId; /*Change to long for future larger databases.*/
        string tempHtml;
        Boolean duplicate;


        /**
       * Name:         protected void Page_Load(object sender, EventArgs e)  
       * Description:  Called on page load. rejects user if they are not logged and redirects to login page.
       *               if they are logged in they are allowed to access the page.
       * Arguments:    sender: Object being sent. Not currently used.
       *               e:      Any events being sent. Not currently used.
       * Return:       Nothing being returned.      
       * Author:       Johnathan Falbo
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
        }


        /**
       * Name:         public void CrawlClick(object sender, EventArgs e)    
       * Description:  Takes url input from user, checks for validity and begins to crawl.
       * Arguments:    sender: Object being sent. Not currently used.
       *               e:      Any events being sent. Not currently used.
       * Return:       Nothing being returned.       
       * Author:       Duncan Reitboeck
       * Date:         09/04/2015
       * */
        public void CrawlClick(object sender, EventArgs e)
        {
            SearchEntities db = new SearchEntities();

            /*Page for starting recursive crawl.*/
            currentCrawlPage = 0;

            /*Check for empty input in web crawl.*/
            if (crawlTextBox.Text == null | crawlTextBox.Text == "" | crawlTextBox.Text == " " | (crawlTextBox.Text.Replace(" ", "") == ""))
            {
                errorText.Text = "*NO URL ENTERED*.";
                return;
            }

            /*String containing url to crawl*/
            string startUrl = crawlTextBox.Text;

            /*Check that website has http:// prefix or add if not*/
            if (!startUrl.Contains("http"))
                startUrl = "http://" + startUrl;

            /*Crawl the url*/
            WebCrawlStart(startUrl);

            /*Continue the recursive crawl with new anchors*/
            RecursiveCrawl();
        }

        /**
        * Name:         public void WebCrawlStart(string url)      
        * Description:  The basic functionality for web crawl. Calls other functions for full functionality.
        * Arguments:    url: The url being scanned.
        * Return:       Nothing being returned.     
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        public void WebCrawlStart(string url)
        {
            /*Attempt to load website based on url given in arguement*/
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
            }

            /*Return if website can not be loaded.*/
            catch
            {
                errorText.Text = "*INVALID WEBSITE*";
                return;
            }

            /*Create web client and database*/
            WebClient wc = new WebClient();
            tempHtml = "";

            /*Create readable string containing all information from current web page.*/
            if (url != null)
            {
                tempHtml = wc.DownloadString(url);
            }

            /*Check that page has links*/
            if (tempHtml.Contains("href"))
            {
                /*Regex for anchors*/
                string hrefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";

                /*Check for link in page by looking at anchor regex and comparing to content.*/
                Match m = Regex.Match(tempHtml, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still links in web page.*/
                while (m.Success)
                {
                    /*Create temporary string of new anchor.*/
                    CrawlNext(url, m.Groups[1].ToString());

                    m = m.NextMatch();
                }
            }
        }

        /**
        * Name:         protected void CrawlNext(string oldUrl, string url)    
        * Description:  Crawls the next url in database.
        * Arguments:    oldUrl: The original information of the crawl. This is used when links don't have the full website.
        *                       For example /About.com instead of www.website/About.com 
        *               url: Url being scanned.
        * Return:       Nothing being returned.   
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void CrawlNext(string oldUrl, string url)
        {
            /*Check that website has http:// prefix or add if not. This is used to ensure the web page is a full page.
              For example /ExtensionOfOriginalPage.com would have original url added to it.*/
            if (!url.Contains("http"))
                url = oldUrl + url;

            /*Check that website is valid extension.*/
            if (url.Contains(".css") || url.Contains(".xml") || url.Contains(".ico") || url.Contains(".gif"))
                return;

            /*Attempt to load website based on url given in arguement*/
            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
            }

            /*Return if website can not be loaded*/
            catch
            {
                return;
            }

            /*Create web client and database*/
            WebClient wc = new WebClient();
            tempHtml = "";

            /*Create readable string containing all information from current web page.*/
            if (url != null)
            {
                tempHtml = wc.DownloadString(url);
            }

            SearchEntities db = new SearchEntities();

            /*Check word id for future use.*/
            //currentWordId = 0;
            currentWordId = (from w in db.words orderby w.id descending select w.id).FirstOrDefault();

            /*If not found set word id to 0.*/
            if (currentWordId != 0)
                currentWordId++;

            /*Reset duplicate boolean.*/
            duplicate = false;

            /*New url to be added to database.*/
            AntiCorruptionSeachEngine.website curUrl = new AntiCorruptionSeachEngine.website();

            /*Check that entry does not already exist in database or if URL is to long.*/
            foreach (website tempUrl in db.websites)
            {
                /*Ignore forward slashes in situations where anchor uses multiple. 
                 * Ex. www.website.com// compared to www.website.com/ */
                string dbUrl = tempUrl.anchor.Replace("/", "");
                string compareUrl = url.Replace("/", "");

                /*Compare database URL to current URL.*/
                if (dbUrl.Equals(compareUrl))
                {
                    duplicate = true;
                    break;
                }
            }

            /*If URL instance does not exist add to database.*/
            if (duplicate == false)
            {
                curUrl.anchor = url;

                /*Check if the page is free or not.*/
                curUrl.cost = DetermineCost();

                db.websites.Add(curUrl);

                /*Continue to next match and save changes in database.*/
                db.SaveChanges();

                /*If first page in crawl.*/
                if (currentCrawlPage == 0)
                {
                    currentCrawlPage = (from w in db.websites orderby w.id descending select w.id).FirstOrDefault();
                }

                /*Increase the counter to the new website id.*/
                else
                {
                    currentCrawlPage++;
                }

                /*Get keywords for current website.*/
                CollectKeywords(tempHtml);

                /*Check what industries this page relates to.*/
                DetermineIndustry();

                /*Check what industries this page relates to.*/
                DetermineCountry();

            }
        }

        /**
        * Name:         protected void RecursiveCrawl() 
        * Description:  Checks next website in database to crawl.
        * Arguments:    No arguments sent.
        * Return:       Nothing being returned.     
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void RecursiveCrawl()
        {
            /*Create database.*/
            SearchEntities db = new SearchEntities();

            String tempUrl = (from s in db.websites
                              where s.id == currentCrawlPage
                              select s.anchor).FirstOrDefault();

            /*Check for null then proceed to webcrawl function with current database entry url as arguement.*/
            if (tempUrl != null)
                WebCrawlStart(tempUrl);
        }

        /**
        * Name:         protected void CollectKeywords(string html)   
        * Description:  Collects tags from HTML then calls IsolateKeywords function to add them to database.
        * Arguments:    html: The current website's HTML.
        * Return:       Nothing being returned.     
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void CollectKeywords(string html)
        {
            SearchEntities db = new SearchEntities();

            ///*Get website ID for keyword.*/
            //int webId = (from i in db.websites orderby i.id descending select i.id).FirstOrDefault();

            /*Temporary strings for isolating keywords*/
            string tempTitleTag = "";
            string tempKeyWordTag = "";
            string tempHeaderTag = "";
            string tempTextTag = "";

            /*Isolate title tag from html.*/
            if (html.Contains("<title>"))
            {
                tempTitleTag = " " + (html.Substring((html.IndexOf("<title>") + 7), (html.IndexOf("</title>") - (html.IndexOf("<title>") + 7))));
            }

            /*Isolate keywords section from metadata in html.*/
            if (html.Contains("\"keywords\""))
            {
                tempKeyWordTag = html.Substring(html.IndexOf("\"keywords\"") + 10);
                tempKeyWordTag = tempKeyWordTag.Substring(0, (tempKeyWordTag.IndexOf("/") + 1));
                tempKeyWordTag = tempKeyWordTag.Substring(tempKeyWordTag.IndexOf("\""));
            }

            if (html.Contains("<h1"))
            {
                /*Regex for anchors*/
                string hrefPattern = "<h1[^>]*>(.*)</h1>";

                /*Check for link in page by looking at anchor regex and comparing to content.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still links in web page.*/
                while (m.Success)
                {
                    /*Create temporary string of new anchor.*/
                    string newHeader = m.Groups[1].ToString();
                    tempHeaderTag = tempHeaderTag + newHeader;

                    m = m.NextMatch();
                }
            }

            /*Get all header 1s and add them to the header tag section.*/
            if (html.Contains("<h1"))
            {
                /*Regex for header.*/
                string hrefPattern = "<h1[^>]*>(.*)</h1>";

                /*Check for header in page.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still other headers on page.*/
                while (m.Success)
                {
                    /*Add header text to temporary header section.*/
                    string newHeader = m.Groups[1].ToString();
                    tempHeaderTag = tempHeaderTag + newHeader;

                    m = m.NextMatch();
                }
            }

            /*Get all header 2s and add them to the header tag section.*/
            if (html.Contains("<h2"))
            {
                /*Regex for header.*/
                string hrefPattern = "<h2[^>]*>(.*)</h2>";

                /*Check for header in page.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still other headers on page.*/
                while (m.Success)
                {
                    /*Add header text to temporary header section.*/
                    string newHeader = m.Groups[1].ToString();
                    tempHeaderTag = tempHeaderTag + newHeader + " ";

                    m = m.NextMatch();
                }
            }

            /*Get all header 3s and add them to the header tag section.*/
            if (html.Contains("<h3"))
            {
                /*Regex for header.*/
                string hrefPattern = "<h3[^>]*>(.*)</h3>";

                /*Check for header in page.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still other headers on page.*/
                while (m.Success)
                {
                    /*Add header text to temporary header section.*/
                    string newHeader = m.Groups[1].ToString();
                    tempHeaderTag = tempHeaderTag + newHeader + " ";

                    m = m.NextMatch();
                }
            }

            /*Get all paragraphs and add them to the text section.*/
            if (html.Contains("<p"))
            {
                /*Regex for paragraph.*/
                string hrefPattern = "<p[^>]*>(.*)</p>";

                /*Check for paragraph in page.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still other paragraphs on page.*/
                while (m.Success)
                {
                    /*Add header text to temporary text section.*/
                    string newText = m.Groups[1].ToString();
                    tempTextTag = tempTextTag + newText + " ";

                    m = m.NextMatch();
                }
            }

            /*Get all text from lists.*/
            if (html.Contains("<ul"))
            {
                /*Regex for paragraph.*/
                string hrefPattern = "<ul[^>]*>(.*)</ul>";

                /*Check for paragraph in page.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still other paragraphs on page.*/
                while (m.Success)
                {
                    /*Add header text to temporary text section.*/
                    string newText = m.Groups[1].ToString();
                    tempTextTag = tempTextTag + newText + " ";

                    m = m.NextMatch();
                }
            }

            /*Get all text from lists.*/
            if (html.Contains("<li"))
            {
                /*Regex for paragraph.*/
                string hrefPattern = "<li[^>]*>(.*)</li>";

                /*Check for paragraph in page.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still other paragraphs on page.*/
                while (m.Success)
                {
                    /*Add header text to temporary text section.*/
                    string newText = m.Groups[1].ToString();
                    tempTextTag = tempTextTag + newText + " ";

                    m = m.NextMatch();
                }
            }

            /*Get all text from anchor tags.*/
            if (html.Contains("<a href"))
            {
                /*Regex for paragraph.*/
                string hrefPattern = "<a href[^>]*>(.*)</a>";

                /*Check for paragraph in page.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still other paragraphs on page.*/
                while (m.Success)
                {
                    /*Add header text to temporary text section.*/
                    string newText = m.Groups[1].ToString();
                    tempTextTag = tempTextTag + newText + " ";

                    m = m.NextMatch();
                }
            }

            /*Get all blockquotes and add them to the text section.*/
            if (html.Contains("<blockquote"))
            {
                /*Regex for paragraph.*/
                string hrefPattern = "<blockquote[^>]*>(.*)</blockquote>";

                /*Check for paragraph in page.*/
                Match m = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                        TimeSpan.FromSeconds(1));

                /*Loop while there are still other paragraphs on page.*/
                while (m.Success)
                {
                    /*Add header text to temporary text section.*/
                    string newText = m.Groups[1].ToString();
                    tempTextTag = tempTextTag + newText + " ";

                    m = m.NextMatch();
                }
            }

            /*Get keywords from title tag.*/
            if (tempTitleTag != "")
            {
                /*Call function with title tag to get all key words and store in database.*/
                IsolateKeywords(tempTitleTag, 3, false);
            }

            /*Get keywords from keyword tag.*/
            if (tempKeyWordTag != "")
            {
                /*Call function with key word tag to get all key words and store in database.*/
                IsolateKeywords(tempKeyWordTag, 5, true);
            }

            /*Get keywords from header tag.*/
            if (tempHeaderTag != "")
            {
                /*Boolean value to check if strings have extra unnecessary code.*/
                Boolean seperatedComplete = false;

                /*Remove all unwanted code before looking at keywords.*/
                while (seperatedComplete == false)
                {
                    if (tempHeaderTag.Contains("<"))
                    {
                        tempHeaderTag = tempHeaderTag.Substring(tempHeaderTag.IndexOf(">") + 1);
                    }

                    else
                        seperatedComplete = true;
                }
                /*Call function with header tags to get all key words and store in database.*/
                IsolateKeywords(tempHeaderTag, 2, false);
                seperatedComplete = false;
            }


            /*Get keywords from paragraph/blockquote tag.*/
            if (tempTextTag != "")
            {

                /*Contains all unwanted characters and non word code.*/
                string newTextTag = tempTextTag;

                /*Reset temporary string variable containing html text.*/
                tempTextTag = "";

                /*Check for inner tags and extract all text inbetween.*/
                while (newTextTag.Contains("<"))
                {
                    /*Check if current 0 location is inside a tag.*/
                    if ((newTextTag.IndexOf("<") == 0) || ((newTextTag.IndexOf(">")) < (newTextTag.IndexOf("<"))))
                    {
                        /*Remove anything up to and including >.*/
                        newTextTag = newTextTag.Substring(newTextTag.IndexOf(">") + 1);
                    }

                    /*If 0 location is the beginning of text to be saved.*/
                    else
                    {
                        /*Take all text before opening character <*/
                        tempTextTag += newTextTag.Substring(0, (newTextTag.IndexOf("<"))) + " ";

                        /*Remove all text up until opening character < */
                        newTextTag = newTextTag.Substring(newTextTag.IndexOf("<"));
                    }
                }

                /*Call function with paragraph tags to get all key words and store in database.*/
                IsolateKeywords(tempTextTag, 1, false);

                /*Add text tags to website in database.*/
                var tempWebsite = (from w in db.websites where w.id == currentCrawlPage select w).FirstOrDefault();

                /*Check that title is not to long.*/
                if (tempTitleTag.Length > 99)
                    tempTitleTag = tempTitleTag.Substring(0, 99);

                tempWebsite.info = tempTextTag;
                tempWebsite.title = tempTitleTag;

                /*If there is nothing in info enter the anchor again.*/
                if (tempWebsite.title == null)
                {
                    if (tempWebsite.anchor.Length > 99)
                        tempWebsite.title = tempWebsite.anchor.Substring(0, 99);

                    else
                        tempWebsite.title = tempWebsite.anchor;
                }

                /*If there is nothing in info enter the anchor again.*/
                if (tempWebsite.info == null)
                {
                    tempWebsite.info = tempWebsite.title;
                }
                db.SaveChanges();
            }
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

        /**
        * Name:         protected void IsolateKeywords(string tempTag, int rank, bool isKeyTag)          
        * Description:  Extracts keywords and adds them to the database with a link to the website they relate to and a ranking.
        * Arguments:    tempTag:    The string which contains all the keywords to seperate.
        *               rank:       The ranking of the current tag which will be used for each keyword.
        *               isKeyTag:   Checks if the tag is a keyword tag in metadata. 
        * Return:       Nothing being returned.    
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void IsolateKeywords(string tempTag, int rank, bool isKeyTag)
        {
            if (tempTag != "")
            {
                SearchEntities db = new SearchEntities();
                int indexTag = 0;
                string tempWord = "";

                /*Scan all words from tag.*/
                while (indexTag < tempTag.Length)
                {
                    /*Section for key words meta tag only.*/
                    /*Isolate next word based on comma character in temporary tag.*/
                    if (isKeyTag && tempTag.Contains(","))
                    {
                        /*If keyword tag is written with a comma then space ignore the comma and space (ex. key, word, keyword) */
                        if (tempTag[tempTag.IndexOf(",") + 1] == ' ')
                            indexTag = (tempTag.IndexOf(",") + 2);

                        /*If keyword tag is written with just a comma skip it (ex. key,word,keyword) */
                        else
                            indexTag = (tempTag.IndexOf(",") + 2);

                        /*Remove previous text from string.*/
                        tempTag = tempTag.Substring(indexTag);

                        /*If there is still another word in tag grab string until that point.*/
                        if (tempTag.Contains(","))
                            tempWord = tempTag.Substring(0, tempTag.IndexOf(","));

                        else
                            tempWord = tempTag;
                    }

                    /*Isolate next word in tag.*/
                    else if (tempTag.Contains(" "))
                    {
                        /*Isolate next word based on space character in temporary tag.*/
                        indexTag = (tempTag.IndexOf(" ") + 1);
                        tempTag = tempTag.Substring(indexTag);

                        /*If there is still another word in tag grab string until that point.*/
                        if (tempTag.Contains(" "))
                            tempWord = tempTag.Substring(0, tempTag.IndexOf(" "));

                        /*If there are no more words in the string.*/
                        else
                            tempWord = tempTag;
                    }

                    /*If there are no more words in string.*/
                    else
                    {
                        indexTag = tempTag.Length;
                        tempWord = tempTag;
                    }

                    /*Check for invalid characters/words.*/
                    if (!Regex.IsMatch(tempWord, "^[a-zA-Z]+[-]?[a-zA-Z]+$"))
                        continue;

                    /*Change keyword to lower case*/
                    tempWord = tempWord.ToLower();

                    /*Check tables for word.*/
                    var wordExists = (from web
                                        in db.link_website_words
                                      where web.word_id ==
                                              (from w in db.words
                                               where (w.phrase.Equals(tempWord))
                                               select w.id).FirstOrDefault()
                                      select web).ToList();

                    /*If word does not already exist in database add new word.*/
                    if (wordExists.Count() == 0)
                    {
                        /*Create new word instance to hold new key word.*/
                        word newWord = new word();
                        link_website_words linkWord = new link_website_words();

                        newWord.phrase = tempWord;
                        db.words.Add(newWord);
                        db.SaveChanges();

                        if (currentWordId == 0)
                        {
                            db.SaveChanges();

                            currentWordId = (from w in db.words orderby w.id descending select w.id).FirstOrDefault();
                        }

                        linkWord.website_id = currentCrawlPage;
                        linkWord.word_id = currentWordId;
                        linkWord.rank = rank;
                        db.link_website_words.Add(linkWord);

                        currentWordId++;
                        db.SaveChanges();
                    }

                    /*If word is already in database increase rank by given amount.*/
                    else
                    {
                        foreach (link_website_words webPage in wordExists)
                        {
                            if (webPage.website_id == currentCrawlPage)
                            {
                                webPage.rank += rank;
                                duplicate = true;
                                db.SaveChanges();
                                break;
                            }
                        }

                        /*If word exists but not linked to the current website.*/
                        if (duplicate != true)
                        {
                            link_website_words linkWord = new link_website_words();
                            linkWord.website_id = currentCrawlPage;
                            linkWord.word_id = (from wId in wordExists select wId.word_id).FirstOrDefault();
                            linkWord.rank = rank;
                            db.link_website_words.Add(linkWord);
                            db.SaveChanges();
                        }

                        duplicate = false;
                    }
                }
            }
        }

        /**
        * Name:         protected void DetermineIndustry()        
        * Description:  Scans for mentions of industry in website updates database with information.
        * Arguments:    No arguments being sent.
        * Return:       Nothing being returned.
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void DetermineIndustry()
        {
            SearchEntities db = new SearchEntities();
            string tempKeyword;
            link_industry_website tempLink = new link_industry_website();

            /*Check each stored industry for connection to web crawl.*/
            foreach (industry currentIndustry in db.industries)
            {
                /*Make a new string of keywords.*/
                tempKeyword = currentIndustry.keywords;

                /*Check each keyword (seperated by ,) for connection to html.*/
                do
                {
                    /*Check HTML for first word in keywords list.*/
                    if (tempKeyword.Contains(",") && tempHtml.Contains(tempKeyword.Substring(0, tempKeyword.IndexOf(","))))
                    {
                        /*Add to database.*/
                        tempLink.industry_id = currentIndustry.id;
                        tempLink.website_id = currentCrawlPage;
                        db.link_industry_website.Add(tempLink);
                        break;
                    }

                    /*Check HTML for only, or final word in keywords list.*/
                    else if (tempHtml.Contains(tempKeyword))
                    {
                        /*Add to database.*/
                        tempLink.industry_id = currentIndustry.id;
                        tempLink.website_id = currentCrawlPage;
                        db.link_industry_website.Add(tempLink);
                        break;
                    }

                    /*Move to next keyword.*/
                    tempKeyword = tempKeyword.Substring(tempKeyword.IndexOf(",") + 1);
                }
                while (tempKeyword.Contains(","));

                /*Save database entry.*/
                db.SaveChanges();
            }
        }

        /**
        * Name:         protected void DetermineCountry() 
        * Description:  Scans for mention of countries in website updates database with information.
        * Arguments:    No arguments sent.
        * Return:       Nothing being returned.     
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected void DetermineCountry()
        {
            SearchEntities db = new SearchEntities();
            string tempCountry;
            link_country_website tempLink = new link_country_website();

            /*Check each stored industry for connection to web crawl.*/
            foreach (country currentCountry in db.countries)
            {
                /*Make a new string of keywords.*/
                tempCountry = currentCountry.name;

                if (tempHtml.Contains(tempCountry))
                {
                    /*Add to database.*/
                    tempLink.country_id = currentCountry.id;
                    tempLink.website_id = currentCrawlPage;
                    db.link_country_website.Add(tempLink);
                    break;
                }

                /*Save database entry.*/
                db.SaveChanges();
            }
        }

        /**
        * Name:         protected bool DetermineCost()    
        * Description:  Scans for mention of common payment methods in websites.
        * Arguments:    No arguments sent.
        * Return:       If the website is a pay website return true otherwise false.     
        * Author:       Duncan Reitboeck
        * Date:         09/04/2015
        * */
        protected bool DetermineCost()
        {
            /*Currently just targeting common paid website terms.*/
            /*More work to be done in future iterations.*/

            /*Check for pay now in HTML.*/
            if (tempHtml.Contains("buy now") || tempHtml.Contains("Buy Now") || tempHtml.Contains("buynow"))
            {
                return true;
            }

            /*Check for add to cart.*/
            if (tempHtml.Contains("add to cart") || tempHtml.Contains("Add To Cart") || tempHtml.Contains("addtocart") ||
                tempHtml.Contains("Add to Cart") || tempHtml.Contains("AddToCart") || tempHtml.Contains("AddtoCart"))
            {
                return true;
            }

            /*Return false if no pricing information is found.*/
            return false;
        }
    }
}