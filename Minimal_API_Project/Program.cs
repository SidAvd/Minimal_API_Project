using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Minimal_API_Project.Data;
using Minimal_API_Project.Models;

var builder = WebApplication.CreateBuilder(args);

// Adding the Database to the App
var connectionString = builder.Configuration.GetConnectionString("ApiDbConnectionString");
builder.Services.AddDbContext<MinimalApiContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// To ignore cycles when using JSON (many-to-many relationship)
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Workers actions
var workers = app.MapGroup("/workers");
workers.MapGet("/", GetAllWorkers);
workers.MapGet("/{id}", GetWorker);
workers.MapPost("/", CreateWorker);
workers.MapPut("/{id}", UpdateWorker);
workers.MapDelete("/{id}", DeleteWorker);

static async Task<IResult> GetAllWorkers(MinimalApiContext db)
{
    return TypedResults.Ok(await db.Workers.Select(w => new WorkerDTO()
    {
        Id = w.Id,
        Name = w.Name,
        HireDate = w.HireDate
    }).ToArrayAsync());
}

static async Task<IResult> GetWorker(int id, MinimalApiContext db)
{
    var worker = await db.Workers.FirstOrDefaultAsync(w => w.Id == id);
    if (worker == null)
    {
        return TypedResults.NotFound();
    }
    return TypedResults.Ok(new WorkerDTO()
    {
        Id = worker.Id,
        Name = worker.Name,
        HireDate = worker.HireDate
    });
}

static async Task<IResult> CreateWorker(WorkerDTO workerDTO, MinimalApiContext db)
{
    var worker = new Worker()
    {
        Name = workerDTO.Name,
        HireDate = workerDTO.HireDate
    };
    db.Workers.Add(worker);
    await db.SaveChangesAsync();

    workerDTO.Id = worker.Id;

    return Results.Created($"/{workerDTO.Id}", workerDTO);
}

static async Task<IResult> UpdateWorker(int id, WorkerDTO workerDTO, MinimalApiContext db)
{
    if (id != workerDTO.Id)
    {
        return Results.BadRequest();
    }

    var worker = await db.Workers.FindAsync(id);
    if (worker is null)
    {
        return Results.NotFound();
    }

    worker.Name = workerDTO.Name;
    worker.HireDate = workerDTO.HireDate;

    await db.SaveChangesAsync();

    return Results.NoContent();
}

static async Task<IResult> DeleteWorker(int id, MinimalApiContext db)
{
    var worker = await db.Workers.FirstOrDefaultAsync(w => w.Id == id);
    if (worker == null)
    {
        return TypedResults.NotFound();
    }

    // If the worker is related to at least one errand, they cannot be deleted
    if (db.ErrandWorkers.Any(ew => ew.WorkerId == id))
    {
        return Results.BadRequest();
    }

    db.Workers.Remove(worker);
    await db.SaveChangesAsync();
    return Results.NoContent();
}

// Errands actions
var errands = app.MapGroup("/errands");
errands.MapGet("/", GetAllErrands);
errands.MapGet("/{id}", GetErrand);
errands.MapPost("/", CreateErrand);
errands.MapPut("/{id}", UpdateErrand);
errands.MapDelete("/{id}", DeleteErrand);

static async Task<IResult> GetAllErrands(MinimalApiContext db)
{
    return TypedResults.Ok(await db.Errands.Select(e => new ErrandDTO()
    {
        Id = e.Id,
        Name = e.Name,
        IsCompleted = e.IsCompleted,
        Description = e.Description
    }).ToArrayAsync());
}

static async Task<IResult> GetErrand(int id, MinimalApiContext db)
{
    var errand = await db.Errands.Select(e => new ErrandDTO()
    {
        Id = e.Id,
        Name = e.Name,
        IsCompleted = e.IsCompleted,
        Description = e.Description
    }).FirstOrDefaultAsync(e => e.Id == id);
    if (errand == null)
    {
        return TypedResults.NotFound();
    }
    return TypedResults.Ok(errand);
}

static async Task<IResult> CreateErrand(ErrandDTO errandDTO, MinimalApiContext db)
{
    var errand = new Errand()
    {
        Id = errandDTO.Id,
        Name = errandDTO.Name,
        IsCompleted = errandDTO.IsCompleted,
        Description = errandDTO.Description
    };
    db.Errands.Add(errand);
    await db.SaveChangesAsync();

    errandDTO.Id = errand.Id;

    return Results.Created($"/{errandDTO.Id}", errandDTO);
}

static async Task<IResult> UpdateErrand(int id, ErrandDTO errandDTO, MinimalApiContext db)
{
    if (id != errandDTO.Id)
    {
        return Results.BadRequest();
    }

    var errand = await db.Errands.FindAsync(id);

    if (errand is null)
    {
        return Results.NotFound();
    }

    errand.Name = errandDTO.Name;
    errand.IsCompleted = errandDTO.IsCompleted;
    errand.Description = errandDTO.Description;

    await db.SaveChangesAsync();

    return Results.NoContent();
}

static async Task<IResult> DeleteErrand(int id, MinimalApiContext db)
{
    var errand = await db.Errands.FirstOrDefaultAsync(e => e.Id == id);
    if (errand == null)
    {
        return TypedResults.NotFound();
    }

    // If the errand is related to at least one worker, it cannot be deleted
    if (db.ErrandWorkers.Any(ew => ew.ErrandId == id))
    {
        return Results.BadRequest();
    }

    db.Errands.Remove(errand);
    await db.SaveChangesAsync();

    return Results.NoContent();
}

// ErransWorker actions
var errandWorkers = app.MapGroup("/errandworkers");
errandWorkers.MapGet("/", GetAllErrandWorkers);
errandWorkers.MapGet("/{ErrandId}/{WorkerId}", GetErrandWorker);
errandWorkers.MapPost("/", CreateErrandWorker);
errandWorkers.MapDelete("/{ErrandId}/{WorkerId}", DeleteErrandWorker);

static async Task<IResult> GetAllErrandWorkers(MinimalApiContext db)
{
    return TypedResults.Ok(await db.ErrandWorkers.Select(ew => new ErrandWorkerDTO()
    {
        ErrandId = ew.ErrandId,
        WorkerId = ew.WorkerId
    }).ToArrayAsync());
}

static async Task<IResult> GetErrandWorker(int errandId, int workerId, MinimalApiContext db)
{
    var errandWorker = await db.ErrandWorkers
        .FirstOrDefaultAsync(ew => ew.ErrandId == errandId && ew.WorkerId == workerId);
    if (errandWorker == null)
    {
        return TypedResults.NotFound();
    }
    return TypedResults.Ok(new ErrandWorkerDTO { ErrandId = errandWorker.ErrandId, WorkerId = errandWorker.WorkerId} );
}

static async Task<IResult> CreateErrandWorker(ErrandWorkerDTO errandWorkerDTO, MinimalApiContext db)
{
    // If the chosen worker doesn't exist or the chosen errand doesn't exist, BadRequest is returned
    if (!db.Workers.Any(w => w.Id == errandWorkerDTO.WorkerId)
        || !db.Errands.Any(e => e.Id == errandWorkerDTO.ErrandId))
    {
        return Results.BadRequest();
    }

    // ErrandWorkerDTO to ErrandWorker
    ErrandWorker errandWorker = new() {
        ErrandId = errandWorkerDTO.ErrandId,
        WorkerId = errandWorkerDTO.WorkerId
    };
    db.ErrandWorkers.Add(errandWorker);
    try
    {
        await db.SaveChangesAsync();
    }
    catch (DbUpdateException)
    {
        if (db.ErrandWorkers.Any(ew => ew.ErrandId == errandWorkerDTO.ErrandId 
            && ew.WorkerId == errandWorkerDTO.WorkerId))
        {
            return Results.Conflict();
        }
        else
        {
            throw;
        }
    }
    

    return Results
        .Created($"/{errandWorker.ErrandId}/{errandWorker.WorkerId}"
        , new ErrandWorkerDTO { ErrandId = errandWorker.ErrandId, WorkerId = errandWorker.WorkerId });
}

static async Task<IResult> DeleteErrandWorker(int errandId, int workerId, MinimalApiContext db)
{
    var errandWorker = await db.ErrandWorkers
        .FirstOrDefaultAsync(ew => ew.ErrandId == errandId && ew.WorkerId == workerId);
    if (errandWorker == null)
    {
        return TypedResults.NotFound();
    }

    db.ErrandWorkers.Remove(errandWorker);
    await db.SaveChangesAsync();

    return Results.NoContent();
}

app.Run();
