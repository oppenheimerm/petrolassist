
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PA.Core.Models;

namespace PA.Datastore.EFCore
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider,
            string initUser, string initUserPassword)
        {
            using (var context = new ApplicationManagmentDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationManagmentDbContext>>()))
            {
                // Initial initial user
                var adminID = await EnsureUser(serviceProvider, initUser, initUserPassword);


                //  Add Roles to database
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.EmployeeRole);
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.StationsReadRole);
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.StationsCreateEditRole);
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.VendorsReadRole);
                await EnsureRole(serviceProvider, adminID, Core.Authorization.Constants.VendorsCreateEditRole);

                var seeded = SeedDB(context, adminID);
                /*if (seeded)
                {
                    await EnsureInitEmployee(adminID, context);
                }*/
            }

        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string initUser, string initPassword)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(initUser);
            if (user == null)
            {

                user = new IdentityUser { 
                    UserName = initUser,
                    Email = "",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, initPassword);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }



        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                              string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static bool SeedDB(ApplicationManagmentDbContext context, string initialUserId)
        {
            if (context.PetrolVendors.Any())
            {
                return false;   // DB has been seeded
            }

            var countries = new Country[]
            {
                new Country { CountryName = "England", CountryCode = "ENG"},
                new Country { CountryName = "Scotland", CountryCode = "SCT"},
                new Country { CountryName = "Wales", CountryCode = "WLS"}
            };

            context.Countries.AddRange(countries);
            context.SaveChanges();

            var vendors = new Vendor[] {
                new Vendor { VendorName = "ASDA Petrol", VendorAddress = "Asda House South Bank Great Wilson Street Leeds", VendorPostcode = "LS11 5AD", CountryId = 1, VendorCode = "ASDA", VendorLogo = "asda" },
                new Vendor { VendorName = "Shell UK", VendorAddress = "Shell Centre London",VendorPostcode = "SE1 7NA", CountryId = 1, VendorCode = "SHUK", VendorLogo = "shell" },
                new Vendor { VendorName = "BP UK", VendorAddress = "1 St James's Square, St. James's, London", VendorPostcode = "SW1Y 4PD", CountryId = 1, VendorCode = "BPUK", VendorLogo = "bp" },
                new Vendor { VendorName = "Texaco UK", VendorAddress = "Va​lero Energy Ltd 1 Canada Square​ London", VendorPostcode = "E14 5AA", CountryId = 1, VendorCode = "TXUK", VendorLogo = "texaco" },
                new Vendor { VendorName = "Esso UK", VendorAddress = "Ermyn House, Ermyn Way Leatherhead Surrey", VendorPostcode = "KT22 8UX", CountryId = 1,VendorCode = "ESUK", VendorLogo = "esso" },
                new Vendor { VendorName = "JETUK", VendorAddress = "7th Floor, 200-202 Aldersgate Street, London", VendorPostcode = "EC1A 4HD", CountryId = 1, VendorCode = "JTUK", VendorLogo = "jet" }
            };

            context.PetrolVendors.AddRange(vendors);
            context.SaveChanges();

            //  We use the geometryFactory with srid equal to 4326 (WGS 84). This is the standard in cartography
            //  and GPS systems. Therefore, for locations on our planet, it is the most used.
            //  https://gavilan.blog/2020/01/07/entity-framework-core-3-1-spatial-queries-nearby-places/
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            //  new Coordinate(long, lat)

            var stations = new Station[]
            {
                new Station { StationName = "Asda Petrol Bethnal Green  &Vallance Road", StationAddress = "112 Vallance Rd, London", StationPostcode = "E1 5BW", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.0639825, 51.522136)), VendorId = 1, CountryId = 1},
                new Station { StationName = "Asda Petrol Bethnal Green  &Vallance Road", StationAddress = "139-149 Whitechapel Rd, London", StationPostcode = "E1 1DT", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.066128499999999993, 51.517756000000013)), VendorId = 2, CountryId = 2},
                new Station { StationName = "BP Cambridge Heath Rd", StationAddress = "319 Cambridge Heath Rd, London", StationPostcode = "E2 9LH", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.055950200000000012, 51.5286397 )), VendorId = 3, CountryId = 1},
                new Station { StationName = "Asda Petrol Bethnal Green  &Vallance Road", StationAddress = "ST KATHERINES, 77-101 The Hwy, London", StationPostcode = "E1W 2BN", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.06346539999999999, 51.5095198)), VendorId = 4, CountryId = 4},
                new Station { StationName = "Shell Old Street", StationAddress = "198-208 Old St, London", StationPostcode = "EC1V 9BP", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.0904657, 51.525169)), VendorId = 2, CountryId = 1},
                new Station { StationName = "Texaco Grove Road", StationAddress = "51, 53 Grove Rd., Bow, London", StationPostcode = "E3 5DU", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.036384, 51.52735149999999)), VendorId = 4, CountryId = 1},
                new Station { StationName = "BP", StationAddress = "102-106 The Hwy, London", StationPostcode = "E1W 2BU", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.06055000000000001, 51.5092828)), VendorId = 3, CountryId = 1},
                new Station { StationName = "Texaco", StationAddress = "241 City Rd, London", StationPostcode = "EC1V 1JQ", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.0948134, 51.5297989)), VendorId = 4, CountryId = 1},
                new Station { StationName = "Asda Old Kent Road Superstore", StationAddress = "Parking lot, 464 - 504 Olmar St, London", StationPostcode = "SE1 5AY", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.06055000000000001, 51.483714)), VendorId = 1, CountryId = 1},
                new Station { StationName = "Shell High St, Old Woking", StationAddress = "65 High St, Old Woking, Woking ", StationPostcode = "GU22 9LN", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.5453992, 51.3023991)), VendorId = 2, CountryId = 1},
                new Station { StationName = "Asda Woking Sheerwater Superstore", StationAddress = "Forsyth Rd, Sheerwater Woking", StationPostcode = "GU21 5SE", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-0.5800787, 51.3193734)), VendorId = 1, CountryId = 1},
                new Station { StationName = "Shell  48 Victoria Rd", StationAddress = "48 Victoria Rd, Glasgow", StationPostcode = "G42 7AA", GeoLocation = geometryFactory.CreatePoint( new Coordinate(-4.2633713, 55.8411979)), VendorId = 2, CountryId = 2},
            };
            context.PetrolStations.AddRange(stations);
            context.SaveChanges();

            return true;
        }
    }
}
