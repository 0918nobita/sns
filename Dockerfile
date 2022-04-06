FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder
WORKDIR /app
COPY Program.fs ./
COPY sns.fsproj ./
COPY initDB.fsx ./
RUN dotnet restore
RUN dotnet publish -c Release -o out
RUN dotnet fsi initDB.fsx

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=builder /app/out .
COPY --from=builder /app/db.sqlite .
EXPOSE 8080
ENTRYPOINT ["dotnet", "sns.dll"]
