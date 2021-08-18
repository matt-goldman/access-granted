using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Med_Man.ADSearch
{
    public class DirectorySearcher
    {
        public SearchResult GetUserDetails(string username)
        {

        }
    }
}
/*
 * <?xml version='1.0' encoding='utf-8'?>
<SettingsFile xmlns="http://schemas.microsoft.com/VisualStudio/2004/01/settings" CurrentProfile="(Default)" GeneratedClassNamespace="VisReg.Properties" GeneratedClassName="Settings">
  <Profiles />
  <Settings>
    <Setting Name="LDAPPath" Type="System.String" Scope="Application">
      <Value Profile="(Default)">muh.org.au</Value>
    </Setting>
    <Setting Name="SearchBase" Type="System.String" Scope="Application">
      <Value Profile="(Default)">OU=MUH Users,DC=MUH,DC=UNI,DC=MQ,DC=EDU,DC=AU</Value>
    </Setting>
    <Setting Name="SenderEmail" Type="System.String" Scope="Application">
      <Value Profile="(Default)">matt.goldman@muh.org.au</Value>
    </Setting>
  </Settings>
</SettingsFile>




    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Configuration;
using System.DirectoryServices;
using Newtonsoft.Json;

namespace VisRegUWP.Controllers
{
    public class SearchadController : ApiController
    {

        static string LogFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/Logs/Debug.log");

        static void WriteToLog(string logEntry)
        {
            DateTime now = DateTime.Now;

            string LogData = "INFO: " + now.ToString() + " | " + logEntry + Environment.NewLine;

            System.IO.File.AppendAllText(LogFilePath, LogData);

        }

        private static DirectoryEntry CreateDirectoryEntry()
        {
            WriteToLog("Retrieving default settings");

            string path = VisReg.Properties.Settings.Default.LDAPPath;
            string SearchBase = "LDAP://" + VisReg.Properties.Settings.Default.SearchBase;
            WriteToLog("Default settings retrieved. Creating directoryEntry");

            return new DirectoryEntry(path)
            {
                Path = SearchBase,
                AuthenticationType = AuthenticationTypes.Secure
            };
        }

        private List<Models.ADUser> SearchForUser(string SearchTerm)
        {
            List<Models.ADUser> UserList = new List<Models.ADUser>();

            if (SearchTerm.Length == 0)
            {
                //ad a single made up user to the list
                Models.ADUser DummyUser = new Models.ADUser();
                DummyUser.Firstname = "No search term provided";
                DummyUser.Lastname = "";

                UserList.Add(DummyUser);
            }
            else
            {
                WriteToLog("Getting search results.");
                SearchResultCollection resultList = new DirectorySearcher(CreateDirectoryEntry())
                {
                    Filter = ("(&(objectClass=user) (cn=*" + SearchTerm + "*))"),
                    PropertiesToLoad =
                    {
                        "givenName",
                        "sn",
                        "sAMAccountName",
                        "mail"
                    }
                }.FindAll();

                WriteToLog("Search retrieved.");
                WriteToLog("Iterating through search results.");

                foreach (SearchResult result in resultList)
                {
                    Models.ADUser thisUser = new Models.ADUser();

                    //DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                    // thisUser.Firstname = result.Properties["givenName"][0].ToString() ?? "Firstname not found";


                    //try
                    //{
                    //    thisUser.Firstname = result.Properties["givenName"][0].ToString();
                    thisUser.Firstname = result.Properties.Contains("givenName") ? result.Properties["givenName"][0].ToString() : "Firstname not found";
                    //}
                    //catch
                    //{
                    //    thisUser.Firstname = "Firstname not found";
                    //}
                    //try
                    //{
                    //   thisUser.Lastname = result.Properties["sn"][0].ToString();
                    thisUser.Lastname = result.Properties.Contains("sn") ? result.Properties["sn"][0].ToString() : "Last name not found";
                    //}
                    //catch
                    //{
                    //    thisUser.Lastname = "Lastname not found";
                    //}
                    //try
                    //{
                    //    thisUser.EmailAddress = result.Properties["mail"][0].ToString();
                    thisUser.EmailAddress = result.Properties.Contains("mail") ? result.Properties["mail"][0].ToString() : "Email address not found";
                    //}
                    //catch
                    //{
                    //    thisUser.EmailAddress = "Email address not found";
                    //}

                    UserList.Add(thisUser);
                }

                WriteToLog("Search results iteration complete.");
            }

            return UserList;
        }

        public HttpResponseMessage Get(string searchTerm)
        {
            List<Models.ADUser> UserList = SearchForUser(searchTerm);

            string serialisedList = Json<List<Models.ADUser>>(UserList).ToString();

            var jsonstring = JsonConvert.SerializeObject(UserList);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonstring, System.Text.Encoding.UTF8, "application/json");

            return response;
        }
    }
}

  */


