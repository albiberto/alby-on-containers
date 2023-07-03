FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProductDataManager/ProductDataManager.csproj", "ProductDataManager/"]
RUN dotnet restore "ProductDataManager/ProductDataManager.csproj"
COPY . .
WORKDIR "/src/ProductDataManager"
RUN dotnet build "ProductDataManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductDataManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductDataManager.dll"]
