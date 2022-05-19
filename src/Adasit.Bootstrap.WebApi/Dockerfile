#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Adasit.Bootstrap.WebApi/Adasit.Bootstrap.WebApi.csproj", "Adasit.Bootstrap.WebApi/"]
RUN dotnet restore "Adasit.Bootstrap.WebApi/Adasit.Bootstrap.WebApi.csproj"
COPY . .
WORKDIR "/src/Adasit.Bootstrap.WebApi"
RUN dotnet build "Adasit.Bootstrap.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adasit.Bootstrap.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adasit.Bootstrap.WebApi.dll"]