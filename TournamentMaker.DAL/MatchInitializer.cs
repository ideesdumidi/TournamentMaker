using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using TournamentMaker.BO;

namespace TournamentMaker.DAL
{
    public class MatchInitializer : DropCreateDatabaseIfModelChanges<MatchContext>
    {
        protected override void Seed(MatchContext context)
        {
            var sports = new List<Sport>
            {
                new Sport { Name = "Baby-foot", Key = "babyfoot", MaxPlayers = 2, MaxTeams = 2, MinTeams = 2},
                new Sport { Name = "Fifa", Key = "fifa", MaxPlayers = 4,MaxTeams = 4,MinTeams = 2},
                new Sport { Name = "7 Wonders", Key = "seven-wonders", MaxPlayers = 1, MaxTeams = 7,MinTeams = 3},
            };
            context.Sports.AddRange(sports);

            context.Players.AddRange(new List<Player>
            {
                new Player
                {
                    Email = "moussa.toure@axa.com",
                    Firstname = "Moussa",
                    Lastname = "Toure",
                    Matricule = @"SIEGE\s608102",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dmoussa+tourre%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "thomas.bachelier@axa.com",
                    Firstname = "Thomas",
                    Lastname = "Bachelier",
                    Matricule = @"SIEGE\s123456",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dthomas+bachelier%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "antoine.bastin@axa.fr",
                    Firstname = "Antoine",
                    Lastname = "Bastin",
                    Matricule = @"SIEGE\s123457",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dantoine+bastin%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "mounir.bedjir@axa.com",
                    Firstname = "Mounir",
                    Lastname = "Bedjir",
                    Matricule = @"SIEGE\s123458",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dmounir+bedjir%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "lassina.camara@axa.com",
                    Firstname = "Lassina",
                    Lastname = "Camara",
                    Matricule = @"SIEGE\s123459",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dlassina+camara%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "sambo.chhor@axa.com",
                    Firstname = "Sambo",
                    Lastname = "Chhor",
                    Matricule = @"SIEGE\s123467",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dsambo+chhor%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "simon.dib@axa.com",
                    Firstname = "Simon",
                    Lastname = "Dib",
                    Matricule = @"SIEGE\s123468",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dsimon+dib%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "vincent.ducrocq@axa.com",
                    Firstname = "Vincent",
                    Lastname = "Ducrocq",
                    Matricule = @"SIEGE\s123469",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dvincent+ducrocq%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "jerome.firlej@axa.com",
                    Firstname = "Jerome",
                    Lastname = "Firlej",
                    Matricule = @"SIEGE\s123567",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Djerome+firlej%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                },
                new Player
                {
                    Email = "christophe.ourdouillie@axa.com",
                    Firstname = "Christophe",
                    Lastname = "Ourdouillie",
                    Matricule = @"SIEGE\s123568",
                    Ranks = new Collection<Rank>{new Rank{Sport = sports.First(), Level = 1000}, new Rank{Sport = sports.Last(), Level = 1000}},
                    Picture = "https://groupdirectory.corp.intraxa/getPicture.do?employeeDn=CN%3Dchristophe+ourdouillie%2COU%3Dpeople%2COU%3Daxa-france%2COU%3Daxa%2CDC%3Dgd%2CDC%3Daxaldap&pictureType=employeePhoto"
                }
            });

            context.SaveChanges();
        }
    }
}
