module Program

open System
open System.Data.SQLite
open Dapper
open FSharp.Json
open Suave
open Suave.Filters
open Suave.Operators

let webServerConfig: SuaveConfig =
    let ip = Net.IPAddress.Parse("0.0.0.0")

    let httpBinding =
        { scheme = HTTP
          socketBinding = { ip = ip; port = 8080us } }

    let homeFolder = IO.Path.GetFullPath("./public")

    { defaultConfig with
        bindings = [ httpBinding ]
        homeFolder = Some homeFolder }

let sendJson data =
    Successful.OK(Json.serialize data)
    >=> Writers.setMimeType "application/json"

/// SQLite データベースを操作する
///
/// 成功したらコールバックの戻り値 `v` が `Some v` の形式で返り、失敗したら `None` が返る
let useSQLiteConn (dataSource: string) (f: SQLiteConnection -> 'a) : 'a option =
    let connStr =
        SQLiteConnectionStringBuilder(DataSource = dataSource)
            .ToString()

    use conn = new SQLiteConnection(connStr)

    try
        conn.Open()
        let webPart = f conn
        conn.Close()
        Some webPart
    with
    | :? SQLiteException -> None

type Comment = { id: int64; body: string }

let timeline =
    GET
    >=> path "/timeline"
    >=> warbler (fun _ctx ->
        useSQLiteConn "db.sqlite" (fun conn ->
            let comments =
                conn.Query<Comment>("select * from timeline")
                |> Seq.toList

            sendJson comments)
        |> Option.orDefault (fun () -> ServerErrors.INTERNAL_ERROR "Something went wrong while collecting comments"))

let app =
    choose [ GET
             >=> path "/"
             >=> Files.browseFileHome "index.html"
             timeline
             GET >=> Files.browseHome
             RequestErrors.NOT_FOUND "Page not found" ]

[<EntryPoint>]
let main _args =
    startWebServer webServerConfig app
    0
