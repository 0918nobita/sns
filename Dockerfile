FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder
WORKDIR /app
COPY Program.fs ./
COPY sns.fsproj ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=builder /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "sns.dll"]
