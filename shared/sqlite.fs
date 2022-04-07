module SQLiteUtil

open System.Data.SQLite

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
