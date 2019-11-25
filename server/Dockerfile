FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
EXPOSE 80
EXPOSE 443
EXPOSE 27017
# Copy csproj and restore as distinct layers
WORKDIR /src
COPY *.sln ./
COPY Kibin/Kibin.csproj ./Kibin/
COPY Kibin.Tests/Kibin.Tests.csproj ./Kibin.Tests/
RUN dotnet restore
# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app
# Build runtime image
WORKDIR /app
ENTRYPOINT ["dotnet", "Kibin.dll"]