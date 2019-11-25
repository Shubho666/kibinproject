FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Kibin/Kibin.csproj", "Kibin/"]
RUN dotnet restore "Kibin/Kibin.csproj"
COPY . .
WORKDIR "/src/Kibin"
RUN dotnet build "Kibin.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Kibin.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Kibin.dll"]