using Application.Interfaces;
using Domain;
using Domain.StaticClasses;
using Npgsql.Replication;

namespace Infrastructure.Persistence;

public class Seed
{
    public static async Task SeedData(PsqlDbContext context, IPasswordHasher passwordHasher)
    {
        // check only users. if no users exist, db is completely empty
        if (!context.Users.Any())
        {
            var users = GenerateUsers(passwordHasher);
            await context.Users.AddRangeAsync(users);

            var organizations = GenerateOrganizations(users[0].Id);
            await context.Organizations.AddRangeAsync(organizations);

            List<string> userIds = [.. users.Select(u => u.Id)];
            var organizationMembers = GenerateOrganizationMembers(userIds, organizations[0].Id);
            await context.OrganizationMembers.AddRangeAsync(organizationMembers);

            var products = GenerateProducts(organizations[0].Id);
            await context.Products.AddRangeAsync(products);

            var saleRecords = GenerateSaleRecords(products, organizations[0].Id);
            await context.SaleRecords.AddRangeAsync(saleRecords);
        }

        await context.SaveChangesAsync();
    }

    private static List<User> GenerateUsers(IPasswordHasher passwordHasher)
    {
        return
        [
            new User("testowner@test.com", passwordHasher.HashPassword("P4$$word"), "TestOwner"),
            new User(
                "testmanager@test.com",
                passwordHasher.HashPassword("P4$$word"),
                "TestManager"
            ),
            new User(
                "testemployee@test.com",
                passwordHasher.HashPassword("P4$$word"),
                "TestEmployee"
            ),
            new User("testuser@test.com", passwordHasher.HashPassword("P4$$word"), "TestUser"),
        ];
    }

    private static List<Organization> GenerateOrganizations(string creatorId)
    {
        return [new Organization("TestOrganization", creatorId)];
    }

    private static List<OrganizationMember> GenerateOrganizationMembers(
        List<string> userIds,
        string organizationId
    )
    {
        return
        [
            new OrganizationMember(userIds[0], organizationId, UserRole.Owner),
            new OrganizationMember(userIds[1], organizationId, UserRole.Manager),
            new OrganizationMember(userIds[2], organizationId, UserRole.Employee),
        ];
    }

    private static List<Product> GenerateProducts(string organizationId)
    {
        return
        [
            new Product(
                "product 1",
                "pro-1-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                200,
                50,
                200,
                7,
                "other"
            ),
            new Product(
                "product 2",
                "pro-2-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                300,
                10,
                400,
                4,
                "other"
            ),
            new Product(
                "product 3",
                "pro-3-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                200,
                50,
                200,
                7,
                "other"
            ),
            new Product(
                "product 4",
                "pro-4-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                600,
                50,
                200,
                9,
                "other"
            ),
            new Product(
                "product 5",
                "pro-5-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                200,
                400,
                200,
                7,
                "other"
            ),
            new Product(
                "product 6",
                "pro-6-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                280,
                5,
                290,
                7,
                "other"
            ),
            new Product(
                "product 7",
                "pro-7-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                200,
                90,
                200,
                3,
                "other"
            ),
            new Product(
                "product 8",
                "pro-8-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                270,
                70,
                270,
                14,
                "other"
            ),
            new Product(
                "product 9",
                "pro-9-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                200,
                50,
                900,
                2,
                "other"
            ),
            new Product(
                "product 10",
                "pro-10-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                1000,
                500,
                200,
                10,
                "other"
            ),
            new Product(
                "product 11",
                "pro-11-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                700,
                50,
                200,
                7,
                "other"
            ),
            new Product(
                "product 12",
                "pro-12-1",
                "vendor",
                organizationId,
                0.50m,
                2.50m,
                200,
                50,
                500,
                7,
                "other"
            ),
        ];
    }

    private static List<SaleRecord> GenerateSaleRecords(
        List<Product> products,
        string organizationId
    )
    {
        return
        [
            // Day 1 (20 days ago)
            new SaleRecord(
                10,
                products[0].Sku,
                organizationId,
                products[0].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                45,
                products[1].Sku,
                organizationId,
                products[1].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                120,
                products[2].Sku,
                organizationId,
                products[2].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                75,
                products[3].Sku,
                organizationId,
                products[3].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                200,
                products[4].Sku,
                organizationId,
                products[4].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                30,
                products[5].Sku,
                organizationId,
                products[5].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                160,
                products[6].Sku,
                organizationId,
                products[6].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                55,
                products[7].Sku,
                organizationId,
                products[7].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                90,
                products[8].Sku,
                organizationId,
                products[8].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                15,
                products[9].Sku,
                organizationId,
                products[9].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                180,
                products[10].Sku,
                organizationId,
                products[10].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            new SaleRecord(
                40,
                products[11].Sku,
                organizationId,
                products[11].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-20))
            ),
            // Day 2 (19 days ago)
            new SaleRecord(
                80,
                products[0].Sku,
                organizationId,
                products[0].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                20,
                products[1].Sku,
                organizationId,
                products[1].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                145,
                products[2].Sku,
                organizationId,
                products[2].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                60,
                products[3].Sku,
                organizationId,
                products[3].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                175,
                products[4].Sku,
                organizationId,
                products[4].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                100,
                products[5].Sku,
                organizationId,
                products[5].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                35,
                products[6].Sku,
                organizationId,
                products[6].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                190,
                products[7].Sku,
                organizationId,
                products[7].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                70,
                products[8].Sku,
                organizationId,
                products[8].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                130,
                products[9].Sku,
                organizationId,
                products[9].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                25,
                products[10].Sku,
                organizationId,
                products[10].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            new SaleRecord(
                155,
                products[11].Sku,
                organizationId,
                products[11].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-19))
            ),
            // Day 3 (18 days ago)
            new SaleRecord(
                110,
                products[0].Sku,
                organizationId,
                products[0].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                65,
                products[1].Sku,
                organizationId,
                products[1].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                5,
                products[2].Sku,
                organizationId,
                products[2].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                185,
                products[3].Sku,
                organizationId,
                products[3].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                50,
                products[4].Sku,
                organizationId,
                products[4].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                140,
                products[5].Sku,
                organizationId,
                products[5].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                85,
                products[6].Sku,
                organizationId,
                products[6].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                0,
                products[7].Sku,
                organizationId,
                products[7].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                165,
                products[8].Sku,
                organizationId,
                products[8].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                95,
                products[9].Sku,
                organizationId,
                products[9].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                115,
                products[10].Sku,
                organizationId,
                products[10].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            new SaleRecord(
                30,
                products[11].Sku,
                organizationId,
                products[11].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-18))
            ),
            // Day 4 (17 days ago)
            new SaleRecord(
                195,
                products[0].Sku,
                organizationId,
                products[0].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                40,
                products[1].Sku,
                organizationId,
                products[1].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                170,
                products[2].Sku,
                organizationId,
                products[2].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                25,
                products[3].Sku,
                organizationId,
                products[3].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                105,
                products[4].Sku,
                organizationId,
                products[4].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                55,
                products[5].Sku,
                organizationId,
                products[5].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                135,
                products[6].Sku,
                organizationId,
                products[6].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                80,
                products[7].Sku,
                organizationId,
                products[7].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                15,
                products[8].Sku,
                organizationId,
                products[8].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                160,
                products[9].Sku,
                organizationId,
                products[9].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                70,
                products[10].Sku,
                organizationId,
                products[10].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            new SaleRecord(
                120,
                products[11].Sku,
                organizationId,
                products[11].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-17))
            ),
            // Day 5 (16 days ago)
            new SaleRecord(
                35,
                products[0].Sku,
                organizationId,
                products[0].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                150,
                products[1].Sku,
                organizationId,
                products[1].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                90,
                products[2].Sku,
                organizationId,
                products[2].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                200,
                products[3].Sku,
                organizationId,
                products[3].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                45,
                products[4].Sku,
                organizationId,
                products[4].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                175,
                products[5].Sku,
                organizationId,
                products[5].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                60,
                products[6].Sku,
                organizationId,
                products[6].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                125,
                products[7].Sku,
                organizationId,
                products[7].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                20,
                products[8].Sku,
                organizationId,
                products[8].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                185,
                products[9].Sku,
                organizationId,
                products[9].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                75,
                products[10].Sku,
                organizationId,
                products[10].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            new SaleRecord(
                140,
                products[11].Sku,
                organizationId,
                products[11].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-16))
            ),
            // Day 6 (15 days ago)
            new SaleRecord(
                155,
                products[0].Sku,
                organizationId,
                products[0].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                85,
                products[1].Sku,
                organizationId,
                products[1].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                30,
                products[2].Sku,
                organizationId,
                products[2].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                110,
                products[3].Sku,
                organizationId,
                products[3].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                190,
                products[4].Sku,
                organizationId,
                products[4].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                65,
                products[5].Sku,
                organizationId,
                products[5].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                15,
                products[6].Sku,
                organizationId,
                products[6].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                145,
                products[7].Sku,
                organizationId,
                products[7].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                100,
                products[8].Sku,
                organizationId,
                products[8].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                50,
                products[9].Sku,
                organizationId,
                products[9].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                170,
                products[10].Sku,
                organizationId,
                products[10].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            new SaleRecord(
                5,
                products[11].Sku,
                organizationId,
                products[11].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-15))
            ),
            // Day 7 (14 days ago)
            new SaleRecord(
                125,
                products[0].Sku,
                organizationId,
                products[0].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                0,
                products[1].Sku,
                organizationId,
                products[1].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                180,
                products[2].Sku,
                organizationId,
                products[2].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                70,
                products[3].Sku,
                organizationId,
                products[3].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                130,
                products[4].Sku,
                organizationId,
                products[4].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                20,
                products[5].Sku,
                organizationId,
                products[5].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                195,
                products[6].Sku,
                organizationId,
                products[6].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                55,
                products[7].Sku,
                organizationId,
                products[7].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                115,
                products[8].Sku,
                organizationId,
                products[8].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                165,
                products[9].Sku,
                organizationId,
                products[9].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                40,
                products[10].Sku,
                organizationId,
                products[10].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            new SaleRecord(
                90,
                products[11].Sku,
                organizationId,
                products[11].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14))
            ),
            // Day 8 (13 days ago)
            new SaleRecord(
                60,
                products[0].Sku,
                organizationId,
                products[0].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                135,
                products[1].Sku,
                organizationId,
                products[1].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                25,
                products[2].Sku,
                organizationId,
                products[2].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                155,
                products[3].Sku,
                organizationId,
                products[3].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                80,
                products[4].Sku,
                organizationId,
                products[4].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                200,
                products[5].Sku,
                organizationId,
                products[5].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                45,
                products[6].Sku,
                organizationId,
                products[6].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                170,
                products[7].Sku,
                organizationId,
                products[7].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                10,
                products[8].Sku,
                organizationId,
                products[8].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                120,
                products[9].Sku,
                organizationId,
                products[9].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                85,
                products[10].Sku,
                organizationId,
                products[10].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            new SaleRecord(
                50,
                products[11].Sku,
                organizationId,
                products[11].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-13))
            ),
            // Day 9 (12 days ago)
            new SaleRecord(
                175,
                products[0].Sku,
                organizationId,
                products[0].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                95,
                products[1].Sku,
                organizationId,
                products[1].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                140,
                products[2].Sku,
                organizationId,
                products[2].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                15,
                products[3].Sku,
                organizationId,
                products[3].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                160,
                products[4].Sku,
                organizationId,
                products[4].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                45,
                products[5].Sku,
                organizationId,
                products[5].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                110,
                products[6].Sku,
                organizationId,
                products[6].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                30,
                products[7].Sku,
                organizationId,
                products[7].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                190,
                products[8].Sku,
                organizationId,
                products[8].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                75,
                products[9].Sku,
                organizationId,
                products[9].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                5,
                products[10].Sku,
                organizationId,
                products[10].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            new SaleRecord(
                145,
                products[11].Sku,
                organizationId,
                products[11].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-12))
            ),
            // Day 10 (11 days ago)
            new SaleRecord(
                100,
                products[0].Sku,
                organizationId,
                products[0].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                180,
                products[1].Sku,
                organizationId,
                products[1].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                55,
                products[2].Sku,
                organizationId,
                products[2].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                130,
                products[3].Sku,
                organizationId,
                products[3].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                20,
                products[4].Sku,
                organizationId,
                products[4].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                165,
                products[5].Sku,
                organizationId,
                products[5].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                70,
                products[6].Sku,
                organizationId,
                products[6].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                195,
                products[7].Sku,
                organizationId,
                products[7].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                35,
                products[8].Sku,
                organizationId,
                products[8].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                150,
                products[9].Sku,
                organizationId,
                products[9].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                90,
                products[10].Sku,
                organizationId,
                products[10].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            new SaleRecord(
                25,
                products[11].Sku,
                organizationId,
                products[11].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-11))
            ),
            // Day 11 (10 days ago)
            new SaleRecord(
                185,
                products[0].Sku,
                organizationId,
                products[0].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                50,
                products[1].Sku,
                organizationId,
                products[1].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                115,
                products[2].Sku,
                organizationId,
                products[2].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                80,
                products[3].Sku,
                organizationId,
                products[3].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                0,
                products[4].Sku,
                organizationId,
                products[4].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                140,
                products[5].Sku,
                organizationId,
                products[5].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                175,
                products[6].Sku,
                organizationId,
                products[6].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                65,
                products[7].Sku,
                organizationId,
                products[7].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                105,
                products[8].Sku,
                organizationId,
                products[8].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                40,
                products[9].Sku,
                organizationId,
                products[9].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                160,
                products[10].Sku,
                organizationId,
                products[10].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            new SaleRecord(
                200,
                products[11].Sku,
                organizationId,
                products[11].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10))
            ),
            // Day 12 (9 days ago)
            new SaleRecord(
                70,
                products[0].Sku,
                organizationId,
                products[0].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                155,
                products[1].Sku,
                organizationId,
                products[1].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                10,
                products[2].Sku,
                organizationId,
                products[2].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                190,
                products[3].Sku,
                organizationId,
                products[3].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                85,
                products[4].Sku,
                organizationId,
                products[4].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                120,
                products[5].Sku,
                organizationId,
                products[5].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                30,
                products[6].Sku,
                organizationId,
                products[6].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                145,
                products[7].Sku,
                organizationId,
                products[7].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                95,
                products[8].Sku,
                organizationId,
                products[8].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                5,
                products[9].Sku,
                organizationId,
                products[9].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                170,
                products[10].Sku,
                organizationId,
                products[10].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            new SaleRecord(
                55,
                products[11].Sku,
                organizationId,
                products[11].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-9))
            ),
            // Day 13 (8 days ago)
            new SaleRecord(
                130,
                products[0].Sku,
                organizationId,
                products[0].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                25,
                products[1].Sku,
                organizationId,
                products[1].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                195,
                products[2].Sku,
                organizationId,
                products[2].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                60,
                products[3].Sku,
                organizationId,
                products[3].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                150,
                products[4].Sku,
                organizationId,
                products[4].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                35,
                products[5].Sku,
                organizationId,
                products[5].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                100,
                products[6].Sku,
                organizationId,
                products[6].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                180,
                products[7].Sku,
                organizationId,
                products[7].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                45,
                products[8].Sku,
                organizationId,
                products[8].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                115,
                products[9].Sku,
                organizationId,
                products[9].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                20,
                products[10].Sku,
                organizationId,
                products[10].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            new SaleRecord(
                165,
                products[11].Sku,
                organizationId,
                products[11].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8))
            ),
            // Day 14 (7 days ago)
            new SaleRecord(
                40,
                products[0].Sku,
                organizationId,
                products[0].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                110,
                products[1].Sku,
                organizationId,
                products[1].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                75,
                products[2].Sku,
                organizationId,
                products[2].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                200,
                products[3].Sku,
                organizationId,
                products[3].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                15,
                products[4].Sku,
                organizationId,
                products[4].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                185,
                products[5].Sku,
                organizationId,
                products[5].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                55,
                products[6].Sku,
                organizationId,
                products[6].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                125,
                products[7].Sku,
                organizationId,
                products[7].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                170,
                products[8].Sku,
                organizationId,
                products[8].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                90,
                products[9].Sku,
                organizationId,
                products[9].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                135,
                products[10].Sku,
                organizationId,
                products[10].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            new SaleRecord(
                0,
                products[11].Sku,
                organizationId,
                products[11].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7))
            ),
            // Day 15 (6 days ago)
            new SaleRecord(
                160,
                products[0].Sku,
                organizationId,
                products[0].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                75,
                products[1].Sku,
                organizationId,
                products[1].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                120,
                products[2].Sku,
                organizationId,
                products[2].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                35,
                products[3].Sku,
                organizationId,
                products[3].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                195,
                products[4].Sku,
                organizationId,
                products[4].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                80,
                products[5].Sku,
                organizationId,
                products[5].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                145,
                products[6].Sku,
                organizationId,
                products[6].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                20,
                products[7].Sku,
                organizationId,
                products[7].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                65,
                products[8].Sku,
                organizationId,
                products[8].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                180,
                products[9].Sku,
                organizationId,
                products[9].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                105,
                products[10].Sku,
                organizationId,
                products[10].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            new SaleRecord(
                50,
                products[11].Sku,
                organizationId,
                products[11].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-6))
            ),
            // Day 16 (5 days ago)
            new SaleRecord(
                25,
                products[0].Sku,
                organizationId,
                products[0].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                190,
                products[1].Sku,
                organizationId,
                products[1].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                60,
                products[2].Sku,
                organizationId,
                products[2].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                155,
                products[3].Sku,
                organizationId,
                products[3].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                95,
                products[4].Sku,
                organizationId,
                products[4].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                10,
                products[5].Sku,
                organizationId,
                products[5].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                175,
                products[6].Sku,
                organizationId,
                products[6].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                110,
                products[7].Sku,
                organizationId,
                products[7].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                140,
                products[8].Sku,
                organizationId,
                products[8].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                55,
                products[9].Sku,
                organizationId,
                products[9].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                200,
                products[10].Sku,
                organizationId,
                products[10].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            new SaleRecord(
                85,
                products[11].Sku,
                organizationId,
                products[11].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5))
            ),
            // Day 17 (4 days ago)
            new SaleRecord(
                170,
                products[0].Sku,
                organizationId,
                products[0].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                45,
                products[1].Sku,
                organizationId,
                products[1].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                100,
                products[2].Sku,
                organizationId,
                products[2].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                5,
                products[3].Sku,
                organizationId,
                products[3].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                135,
                products[4].Sku,
                organizationId,
                products[4].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                195,
                products[5].Sku,
                organizationId,
                products[5].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                40,
                products[6].Sku,
                organizationId,
                products[6].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                160,
                products[7].Sku,
                organizationId,
                products[7].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                75,
                products[8].Sku,
                organizationId,
                products[8].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                125,
                products[9].Sku,
                organizationId,
                products[9].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                30,
                products[10].Sku,
                organizationId,
                products[10].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            new SaleRecord(
                185,
                products[11].Sku,
                organizationId,
                products[11].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-4))
            ),
            // Day 18 (3 days ago)
            new SaleRecord(
                90,
                products[0].Sku,
                organizationId,
                products[0].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                165,
                products[1].Sku,
                organizationId,
                products[1].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                15,
                products[2].Sku,
                organizationId,
                products[2].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                120,
                products[3].Sku,
                organizationId,
                products[3].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                55,
                products[4].Sku,
                organizationId,
                products[4].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                150,
                products[5].Sku,
                organizationId,
                products[5].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                80,
                products[6].Sku,
                organizationId,
                products[6].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                0,
                products[7].Sku,
                organizationId,
                products[7].Id,
                0,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                200,
                products[8].Sku,
                organizationId,
                products[8].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                35,
                products[9].Sku,
                organizationId,
                products[9].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                145,
                products[10].Sku,
                organizationId,
                products[10].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            new SaleRecord(
                110,
                products[11].Sku,
                organizationId,
                products[11].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3))
            ),
            // Day 19 (2 days ago)
            new SaleRecord(
                50,
                products[0].Sku,
                organizationId,
                products[0].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                130,
                products[1].Sku,
                organizationId,
                products[1].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                185,
                products[2].Sku,
                organizationId,
                products[2].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                45,
                products[3].Sku,
                organizationId,
                products[3].Id,
                3,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                170,
                products[4].Sku,
                organizationId,
                products[4].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                25,
                products[5].Sku,
                organizationId,
                products[5].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                115,
                products[6].Sku,
                organizationId,
                products[6].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                70,
                products[7].Sku,
                organizationId,
                products[7].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                155,
                products[8].Sku,
                organizationId,
                products[8].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                100,
                products[9].Sku,
                organizationId,
                products[9].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                10,
                products[10].Sku,
                organizationId,
                products[10].Id,
                1,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            new SaleRecord(
                190,
                products[11].Sku,
                organizationId,
                products[11].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2))
            ),
            // Day 20 (yesterday)
            new SaleRecord(
                140,
                products[0].Sku,
                organizationId,
                products[0].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                60,
                products[1].Sku,
                organizationId,
                products[1].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                20,
                products[2].Sku,
                organizationId,
                products[2].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                175,
                products[3].Sku,
                organizationId,
                products[3].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                105,
                products[4].Sku,
                organizationId,
                products[4].Id,
                6,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                30,
                products[5].Sku,
                organizationId,
                products[5].Id,
                2,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                165,
                products[6].Sku,
                organizationId,
                products[6].Id,
                9,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                85,
                products[7].Sku,
                organizationId,
                products[7].Id,
                5,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                120,
                products[8].Sku,
                organizationId,
                products[8].Id,
                7,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                195,
                products[9].Sku,
                organizationId,
                products[9].Id,
                10,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                55,
                products[10].Sku,
                organizationId,
                products[10].Id,
                4,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
            new SaleRecord(
                145,
                products[11].Sku,
                organizationId,
                products[11].Id,
                8,
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))
            ),
        ];
    }
}
