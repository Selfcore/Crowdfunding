FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Crowdfunding.sln ./
COPY Directory.Packages.props ./
COPY Directory.Build.props ./

COPY src/Crowdfunding.Domain/Crowdfunding.Domain.csproj src/Crowdfunding.Domain/
COPY src/Crowdfunding.Application/Crowdfunding.Application.csproj src/Crowdfunding.Application/
COPY src/Crowdfunding.Infrastructure.Postgres/Crowdfunding.Infrastructure.Postgres.csproj src/Crowdfunding.Infrastructure.Postgres/
COPY src/Crowdfunding.Api/Crowdfunding.Api.csproj src/Crowdfunding.Api/

RUN dotnet restore src/Crowdfunding.Api/Crowdfunding.Api.csproj

RUN echo "=== NuGet package after restore ===" && \
    ls -la /root/.nuget/packages/microsoft.codeanalysis.analyzers/3.3.3 || true

COPY . .

RUN echo "=== NuGet package after COPY ===" && \
    ls -la /root/.nuget/packages/microsoft.codeanalysis.analyzers/3.3.3 || true

RUN dotnet publish src/Crowdfunding.Api/Crowdfunding.Api.csproj \
    -c Release \
    -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /publish/ .

ENTRYPOINT ["dotnet", "Crowdfunding.Api.dll"]