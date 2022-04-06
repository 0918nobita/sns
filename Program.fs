module Program

open System
open FSharp.Json
open Suave
open Suave.Filters
open Suave.Operators

let webServerConfig: SuaveConfig =
    let ip = Net.IPAddress.Parse("0.0.0.0")
    let httpBinding = {
        scheme = HTTP
        socketBinding = {
            ip = ip
            port = 8080us
        }
    }
    let homeFolder = IO.Path.GetFullPath("./public")
    {
        defaultConfig with
            bindings = [httpBinding]
            homeFolder = Some homeFolder
    }

let app =
    choose [
        GET >=> path "/" >=> Files.browseFileHome "index.html"
        GET
            >=> path "/timeline"
            >=> warbler (fun _ctx ->
                let connStr = Data.SQLite.SQLiteConnectionStringBuilder(DataSource = "db.sqlite").ToString()
                use conn = new Data.SQLite.SQLiteConnection(connStr)
                try
                    conn.Open()
                    use cmd = conn.CreateCommand()
                    cmd.CommandText <- "SELECT * from timeline"
                    cmd.Prepare()
                    use reader = cmd.ExecuteReader()
                    let mutable comments = []
                    while reader.Read() do
                        comments <- reader.GetString(1)::comments
                    conn.Close()
                    Successful.OK (Json.serialize comments) >=> Writers.setMimeType "application/json"
                with
                | :? Data.SQLite.SQLiteException ->
                    ServerErrors.INTERNAL_ERROR "Something went wrong while collecting comments"
            )
        GET >=> Files.browseHome
        RequestErrors.NOT_FOUND "Page not found"
    ]

[<EntryPoint>]
let main _args =
    startWebServer webServerConfig app
    0
