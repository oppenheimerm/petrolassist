using Microsoft.AspNetCore.Identity;
using PA.Core.Models;
using PA.Datastore.EFCore;

namespace PA.Web.Admin.TmpData
{
    public class InitRepository : IInitRepository
    {

        private readonly ApplicationManagmentDbContext Context;
        private readonly ILogger<InitRepository> Logger;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;


        public InitRepository(ApplicationManagmentDbContext context, ILogger<InitRepository> logger,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> _roleManager)
        {
            Context = context;
            Logger = logger;
            UserManager = userManager;
            RoleManager = _roleManager;
        }

        public async Task SeedDB()
        {

            /*var countries = new Country[]
            {
                new Country { CountryName = "England", CountryCode = "ENG"},
                new Country { CountryName = "Scotland", CountryCode = "SCT"},
                new Country { CountryName = "Wales", CountryCode = "WLS"}
            };

            await Context.Countries.AddRangeAsync(countries);
            await Context.SaveChangesAsync();

            var vendors = new Vendor[] {
                new Vendor { VendorName = "ASDA Petrol", VendorAddress = "Asda House South Bank Great Wilson Street Leeds", VendorPostcode = "LS11 5AD", CountryId = 1, VendorCode = "ASDA", VendorLogo = "asda" },
                new Vendor { VendorName = "Shell UK", VendorAddress = "Shell Centre London",VendorPostcode = "SE1 7NA", CountryId = 1, VendorCode = "SHUK", VendorLogo = "shell" },
                new Vendor { VendorName = "BP UK", VendorAddress = "1 St James's Square, St. James's, London", VendorPostcode = "SW1Y 4PD", CountryId = 1, VendorCode = "BPUK", VendorLogo = "bp" },
                new Vendor { VendorName = "Texaco UK", VendorAddress = "Va​lero Energy Ltd 1 Canada Square​ London", VendorPostcode = "E14 5AA", CountryId = 1, VendorCode = "TXUK", VendorLogo = "texaco" },
                new Vendor { VendorName = "Esso UK", VendorAddress = "Ermyn House, Ermyn Way Leatherhead Surrey", VendorPostcode = "KT22 8UX", CountryId = 1,VendorCode = "ESUK", VendorLogo = "esso" },
                new Vendor { VendorName = "JETUK", VendorAddress = "7th Floor, 200-202 Aldersgate Street, London", VendorPostcode = "EC1A 4HD", CountryId = 1, VendorCode = "JTUK", VendorLogo = "jet" }
            };

            await Context.PetrolVendors.AddRangeAsync(vendors);
            await Context.SaveChangesAsync();*/

            var stations = new Station[]
            {
                new Station { StationName = "Asda Petrol, Bethnal Green & Vallance Road", StationAddress = "112 Vallance Rd, London", StationPostcode = "E1 5BW", Latitude = 51.522136, Longitude = -0.0639825, VendorId = 1, CountryId = 1},
                new Station { StationName = "Shell Whitechaple", StationAddress = "139-149 Whitechapel Rd, London", StationPostcode = "E1 1DT", Latitude = 51.517756000000013, Longitude = -0.066128499999999993, VendorId = 2, CountryId = 2},
                new Station { StationName = "BP Cambridge Heath Rd", StationAddress = "319 Cambridge Heath Rd, London", StationPostcode = "E2 9LH", Latitude = 51.5286397, Longitude = -0.055950200000000012, VendorId = 3, CountryId = 1},
                new Station { StationName = "Asda Petrol Bethnal Green  &Vallance Road", StationAddress = "ST KATHERINES, 77-101 The Hwy, London", StationPostcode = "E1W 2BN", Latitude = 51.5095198, Longitude = -0.06346539999999999, VendorId = 1, CountryId = 4},
                new Station { StationName = "Shell Old Street", StationAddress = "198-208 Old St, London", StationPostcode = "EC1V 9BP", Latitude = 51.525169 , Longitude = -0.0904657, VendorId = 2, CountryId = 1},
                new Station { StationName = "Texaco Grove Road", StationAddress = "51, 53 Grove Rd., Bow, London", StationPostcode = "E3 5DU", Latitude = 51.52735149999999, Longitude = -0.036384, VendorId = 5, CountryId = 1},
                new Station { StationName = "BP", StationAddress = "102-106 The Hwy, London", StationPostcode = "E1W 2BU", Latitude = 51.5092828, Longitude = -0.06055000000000001, VendorId = 3, CountryId = 1},
                new Station { StationName = "Texaco", StationAddress = "241 City Rd, London", StationPostcode = "EC1V 1JQ", Latitude = 51.5297989, Longitude = -0.0948134, VendorId = 4, CountryId = 1},
                new Station { StationName = "Asda Old Kent Road Superstore", StationAddress = "Parking lot, 464 - 504 Olmar St, London", StationPostcode = "SE1 5AY", Latitude = 51.483714, Longitude = -0.0690958, VendorId = 1, CountryId = 1},
                new Station { StationName = "Shell High St, Old Woking", StationAddress = "65 High St, Old Woking, Woking ", StationPostcode = "GU22 9LN", Latitude = 51.3023991, Longitude = -0.5453992, VendorId = 2, CountryId = 1},
                new Station { StationName = "Asda Woking Sheerwater Superstore", StationAddress = "Forsyth Rd, Sheerwater Woking", StationPostcode = "GU21 5SE", Latitude = 51.3193734, Longitude = -0.5800787, VendorId = 1, CountryId = 1},
                new Station { StationName = "Shell  48 Victoria Rd", StationAddress = "48 Victoria Rd, Glasgow", StationPostcode = "G42 7AA", Latitude = 55.8411979, Longitude = -4.2633713, VendorId = 2, CountryId = 2},
            };
            await Context.PetrolStations.AddRangeAsync(stations);
            await Context.SaveChangesAsync();

            var newUserAdded = await AddInitialUser();
            if (newUserAdded.Succeeded)
            {
                //  Add Roles to database
                await EnsureRole(newUserAdded.Id, Core.Authorization.Constants.EmployeeRole);
                await EnsureRole(newUserAdded.Id, Core.Authorization.Constants.StationsReadRole);
                await EnsureRole(newUserAdded.Id, Core.Authorization.Constants.StationsCreateEditRole);
                await EnsureRole(newUserAdded.Id, Core.Authorization.Constants.VendorsReadRole);
                await EnsureRole(newUserAdded.Id, Core.Authorization.Constants.VendorsCreateEditRole);                
            }

        }

        private async Task<TmpUser> AddInitialUser()
        {
            var userExist = await UserManager.FindByNameAsync("");
            if (userExist == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "",
                    Email = "",
                    EmailConfirmed = true,
                    FirstName = "",
                    LastName = "",
                    Initials = "",
                    Title = "",
                    JoinDate = DateTime.Now,
                    PrimaryDepartment = PrimaryDepartment.IT_ENGINEERING,
                };

                var result = await UserManager.CreateAsync(user, "");

                if (result.Succeeded)
                {
                    TmpUser NewUser = new TmpUser
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Succeeded = true
                    };

                    return NewUser;
                }
                else
                {
                    var FailedCreation = new TmpUser();
                    FailedCreation.Succeeded = true;
                    return FailedCreation;
                }
            }
            else
            {
                TmpUser existingUser = new TmpUser
                {
                    Id = userExist.Id,
                    UserName = userExist.UserName,
                    Succeeded = true
                };
                return existingUser;
            }





        }



        private async Task<IdentityResult> EnsureRole(string uid, string role)
        {

            IdentityResult IR;
            if (!await RoleManager.RoleExistsAsync(role))
            {
                IR = await RoleManager.CreateAsync(new IdentityRole(role));
            }

            var user = await UserManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await UserManager.AddToRoleAsync(user, role);
            return IR;
        }

    }


    public class TmpUser
    {
        public string? UserName { get; set; }
        public string? Id { get; set; }
        public bool Succeeded { get; set; }
    }
}
