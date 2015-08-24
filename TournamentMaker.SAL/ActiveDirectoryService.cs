using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Net;

namespace TournamentMaker.SAL
{
    public class ActiveDirectoryService : IDisposable
    {
        public PrincipalContext PrincipalContext { get; set; }

        public ActiveDirectoryService()
        {
            try
            {
                PrincipalContext = new PrincipalContext(ContextType.Domain, "SIEGE", "barco", "barco");
            }
            catch (Exception)
            {
                PrincipalContext = null;
            }
        }

        public string GetUserPicture(string userName)
        {
                using (var userPrincipal = GetUser(userName))
                {
                        if (userPrincipal == null) return null;

                    return
                        String.Format(
                            "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3D{0}+{1}%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto",WebUtility.UrlEncode(userPrincipal.GivenName),WebUtility.UrlEncode(userPrincipal.Surname));
                }
        }

        public UserPrincipal GetUser(string matricule)
        {
            try
            {
                return UserPrincipal.FindByIdentity(PrincipalContext, matricule);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Dispose()
        {
            if (PrincipalContext == null)
                return;

            PrincipalContext.Dispose();
            PrincipalContext = null;
        }
    }
}