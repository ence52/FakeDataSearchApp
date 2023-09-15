using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Transport;
using FakeDataSearchApp;
using OfficeOpenXml;

class Program
{

    static async Task Main(string[] args)
    {
        //Console.WriteLine("Please enter the name of index you want to search");
        //string indexName = Console.ReadLine().Trim();
        ElasticsearchClient client = connectElastic();

        await StartMenuAsync(client);

    }

    static ElasticsearchClient connectElastic()
    {
        //Connection
        var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
            .CertificateFingerprint("810582c380d6c7ae7817ddbe8749f39323cec0ea39c94a0ab89e583dd297075a")
            .DefaultIndex("bigdata")
            .Authentication(new BasicAuthentication("elastic", "jGX7ucJo+9+7iIfTroF1"));

        var client = new ElasticsearchClient(settings);
        return client;
    }

    static async Task TermQueryAsync(ElasticsearchClient client)
    {
        //Entries from user
        Console.WriteLine("Please enter the field: ");
        string fieldName = Console.ReadLine().Trim();
        Console.WriteLine("Please enter the value: ");
        string value = Console.ReadLine().Trim();
        Console.WriteLine("Please enter max data count (max 10000): ");
        int.TryParse(Console.ReadLine(), out int dataCount);


        //Preparing query
        var termQuery = new TermQuery(fieldName) { Value = value.Trim(), CaseInsensitive = true };
        if (dataCount > 10000)
        {
            dataCount = 10000;
        }
        //Applying query
        var response = await client.SearchAsync<Order>(s => s.Size(dataCount)
        .Query(q => q
            .Bool(b => b
                .Should(s => s
                    .Term(termQuery)
                    .Fuzzy(fz => fz
                        .Field(fieldName)
                        .Value(value)
                        .Fuzziness(new Fuzziness("auto")))
                    .MatchPhrase(mp=>mp.Field(fieldName))
                    .Prefix(p => p
                        .Field(fieldName)
                        .Value(value)))

                )

            )
        );
        CheckDocuments(response);




    }
    static async Task MatchQueryFullTextAsync(ElasticsearchClient client)
    {
        Console.WriteLine("Please enter the value: ");
        string searchWord = Console.ReadLine().Trim();
        Console.WriteLine("Please enter max data count (max 10000): ");
        int.TryParse(Console.ReadLine(), out int dataCount);
        if (dataCount > 10000)
        {
            dataCount = 10000;
        }
        var searchResponse = await client.SearchAsync<Order>(s => s.Size(dataCount)
        .Query(q => q
            .Bool(b => b
                .Should(s => s
                    .MultiMatch(m =>m
                        .Query(searchWord)
                        .Type(TextQueryType.BoolPrefix)
                        )
                    .MultiMatch(m => m
                        .Query(searchWord)
                        .Fuzziness(new Fuzziness("auto"))
                        )
                    )
                )
            )
        );

        CheckDocuments(searchResponse);

    }



    static async Task StartMenuAsync(ElasticsearchClient client)
    {
        //User chooses a query
        Console.WriteLine("Choose query:\n\t1.Term Query\n\t2.Match Query (Full Text)");
        int.TryParse(Console.ReadLine(), out int chosenQuery);
        if (chosenQuery == null) ;
        //Query switch
        switch (chosenQuery)
        {
            case 1:
                await TermQueryAsync(client);
                break;
            case 2:
                await MatchQueryFullTextAsync(client);
                break;
            default:
                break;
        }
    }
    static void CheckDocuments(SearchResponse<Order> searchResponse)
    {
        //Checking response
        if (searchResponse.IsValidResponse)
        {
            if (searchResponse.Documents.Count != 0)
            {
                Console.WriteLine("{0} documents found! Do you want to export them to excel file? [Y,N]", searchResponse.Documents.Count);
                char.TryParse(Console.ReadLine(), out char option);

                switch (option)
                {
                    case 'y':
                        Console.WriteLine("Please enter the name of .xlsx file");
                        string fileName = Console.ReadLine().Trim();
                        CreateExcel(searchResponse, fileName);
                        break;
                    default:
                        break;
                }
            }

            else { Console.WriteLine("There is no document matches query"); }
        }
    }

    private static void CreateExcel(SearchResponse<Order> searchResponse, string fileName)
    {   
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //Path
        string directory = @"C:\\Users\\FNY01698\\Desktop\\exceller\\" + fileName + ".xlsx";
        FileInfo excelFile = new FileInfo(directory);

        using (ExcelPackage package = new ExcelPackage(excelFile))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");
            int row = 2;
            foreach (var item in searchResponse.Documents)
            {
                worksheet.Cells[row, 2].Value = item.Id;
                worksheet.Cells[row, 3].Value = item.Food;
                worksheet.Cells[row, 4].Value = item.Quantity;
                worksheet.Cells[row, 5].Value = item.Price;
                worksheet.Cells[row, 6].Value = item.TotalPrice;
                worksheet.Cells[row, 7].Value = item.OrderTime.ToShortDateString() + "  " + item.OrderTime.ToShortTimeString();
                worksheet.Cells[row, 8].Value = item.User.FirstName;
                worksheet.Cells[row, 9].Value = item.User.LastName;
                worksheet.Cells[row, 10].Value = item.User.FullName;
                worksheet.Cells[row, 11].Value = item.User.EmailAddress;
                worksheet.Cells[row, 12].Value = item.User.BirthDay.ToShortDateString();
                worksheet.Cells[row, 13].Value = item.User.PhoneNumber;
                worksheet.Cells[row, 14].Value = item.User.Address.Street;
                worksheet.Cells[row, 15].Value = item.User.Address.BuildingNumber;
                worksheet.Cells[row, 16].Value = item.User.Address.City;
                worksheet.Cells[row, 17].Value = item.User.Address.State;
                worksheet.Cells[row, 18].Value = item.User.Address.Zip;

                row++;
            }
            worksheet.Cells[1, 2].Value = "ID";
            worksheet.Cells[1, 3].Value = "Food";
            worksheet.Cells[1, 4].Value = "Quantity";
            worksheet.Cells[1, 5].Value = "Price";
            worksheet.Cells[1, 6].Value = "Total Price";
            worksheet.Cells[1, 7].Value = "Order Time";
            worksheet.Cells[1, 8].Value = "First Name";
            worksheet.Cells[1, 9].Value = "Last Name";
            worksheet.Cells[1, 10].Value = "Full Name";
            worksheet.Cells[1, 11].Value = "Email Address";
            worksheet.Cells[1, 12].Value = "Birthday";
            worksheet.Cells[1, 13].Value = "Phone Number";
            worksheet.Cells[1, 14].Value = "Street";
            worksheet.Cells[1, 15].Value = "Building Number";
            worksheet.Cells[1, 16].Value = "City";
            worksheet.Cells[1, 17].Value = "State";
            worksheet.Cells[1, 18].Value = "Zip";

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            worksheet.Cells["B1:R1"].Style.Font.Bold = true;
            // Save file
            package.Save();
        }

        Console.WriteLine("xlsx created");
    }
}