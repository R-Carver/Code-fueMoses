using System;
using System.DirectoryServices;

namespace WebApplication1.ActiveDirectory
{
    //Important: 
    //This class manages the connection to a Windows Server Active Directory (it won't work for an Azure AD)
    //Password, Connection, Path and Username need to be added for it to work
    public class ADConnection : IDisposable
    {
        //DirectoryEntry
        public DirectoryEntry DirConnection { get; private set; }

        //AD Login Data
        private readonly string path = "";
        private readonly string username = "";
        private readonly string password = "";

        /// <summary>
        /// Constructor for ADConnection,
        /// initializes the Connection
        /// </summary>
        public ADConnection()
        {
            DirConnection = createDirectoryEntry();
        }

        /// <summary>
        /// Checks if a User with a specific Emailadress exists within the Active Director
        /// </summary>
        /// <param name="email">email address to check</param>
        /// <returns>true, if email exists in Active Directory</returns>
        public bool userEmailexists(string email)
        {
            bool result = false;

            using (DirectorySearcher search = new DirectorySearcher(DirConnection))
            {

                SearchResult userfind = search.FindOne();
                if (userfind != null)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Creates an Entry for the local ActiveDirecotry
        /// </summary>
        /// <returns>DirectoryEntry for the Active Directory</returns>
        private DirectoryEntry createDirectoryEntry()
        {
            DirectoryEntry ldapconnection = new DirectoryEntry(path, username, password)
            {
                AuthenticationType = AuthenticationTypes.Secure,
            };
            return ldapconnection;
        }

        /// <summary>
        /// Auto generated, closes Connection
        /// </summary>
        public void Dispose()
        {
            if (DirConnection != null)
                DirConnection.Dispose();
        }
    }
}