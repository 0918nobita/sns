#r "nuget: System.Data.SQLite.Core, 1.0.115.5"

#load "sqlite.fs"

open System.IO

if File.Exists("db.sqlite") then File.Delete("db.sqlite")

SQLiteUtil.useSQLiteConn "db.sqlite" (fun conn ->
    use cmd = conn.CreateCommand()

    cmd.CommandText <- "CREATE TABLE timeline(id INTEGER PRIMARY KEY, body TEXT NOT NULL)"
    cmd.ExecuteNonQuery() |> ignore

    [
        "Saluton!"
        "Bonan matenon!"
        "Bonan tagon!"
        "Bonan vesperon!"
        "Bonan nokton!"
    ]
    |> List.iter (fun body ->
        cmd.CommandText <- "INSERT INTO timeline(body) VALUES (@body)"
        cmd.Parameters.AddWithValue("body", body) |> ignore
        cmd.ExecuteNonQuery() |> ignore
        cmd.Parameters.Clear()
    )
)
