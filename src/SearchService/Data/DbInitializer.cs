﻿using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService;

public class DbInitializer
{
    public static async Task InitiDb(WebApplication app)
    {
        await DB.InitAsync("SearchDB", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>().Key(x => x.Make, KeyType.Text).Key(x => x.Model, KeyType.Text).Key(x => x.Color, KeyType.Text).CreateAsync();

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

        var items = await httpClient.GetItemForSearchDb();

        if (items.Count > 0) await DB.SaveAsync(items);
    }
}
