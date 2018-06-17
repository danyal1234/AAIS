using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Web;

namespace AssetApp.Models
{
    public class ADUser
    {

        public string displayName { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string mail { get; set; }
        public string telephoneNumber { get; set; }
        public string allowed { get; set; }
        public string[] groups { get; set; }


        public static ADUser finduser(string Name)
        {

            var userPrincipalName = Name + "@civic.mississauga.ca";
            

            DirectoryEntry de = new DirectoryEntry();
            de.Path = "LDAP://civic.mississauga.ca";
            DirectorySearcher deSearch = new DirectorySearcher(de);
            deSearch.PropertiesToLoad.Add("department");
            deSearch.PropertiesToLoad.Add("company");
            deSearch.PropertiesToLoad.Add("DisplayName");
            deSearch.PropertiesToLoad.Add("mail");
            deSearch.PropertiesToLoad.Add("TelephoneNumber");
            deSearch.PropertiesToLoad.Add("memberOf");
            deSearch.SearchScope = SearchScope.Subtree;            
            deSearch.Filter = "(&(objectClass=user)(userPrincipalName=" + userPrincipalName + "))";
            SearchResultCollection results = deSearch.FindAll();

           
            StringBuilder groupNames = new StringBuilder();
            SearchResult deresult = deSearch.FindOne();

            if (deresult == null)
            {
                ADUser aduser2 = new ADUser();                
                return aduser2;
            }





            int propertyCount = deresult.Properties["memberOf"].Count;
            String dn;
            int equalsIndex, commaIndex;

            for (int propertyCounter = 0; propertyCounter < propertyCount;
                propertyCounter++)
            {
                dn = (String)deresult.Properties["memberOf"][propertyCounter];

                equalsIndex = dn.IndexOf("=", 1);
                commaIndex = dn.IndexOf(",", 1);
                if (-1 == equalsIndex)
                {
                    return null;
                }
                groupNames.Append(dn.Substring((equalsIndex + 1),
                            (commaIndex - equalsIndex) - 1));
                groupNames.Append(",");
            }




            //Change it to no if you want to validate the user agains a AD group
            var allowed = "yes";
            var groups = groupNames.ToString().Split(',');
            foreach (var r in groups)
            {
                if (r.ToString() == "CMLC")
                {
                    allowed = "yes";
                }
            }



            ADUser aduser = new ADUser();
            foreach (SearchResult result in results)
            {
                aduser.displayName = GetProperty(result, "displayName");
                aduser.company = GetProperty(result, "company");
                aduser.department = GetProperty(result, "department");
                aduser.mail = GetProperty(result, "mail");
                aduser.telephoneNumber = GetProperty(result, "TelephoneNumber");
                aduser.allowed = allowed;
                aduser.groups = groups;
            }
            return aduser;

        }


        private static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}