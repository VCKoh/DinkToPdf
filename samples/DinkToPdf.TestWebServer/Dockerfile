#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

# Install libwkhtmltox dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        zlib1g \
        fontconfig \
        libfreetype6 \
        libx11-6 \
        libxext6 \
        libxrender1 \
		libjpeg62-turbo

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["samples/DinkToPdf.TestWebServer/DinkToPdf.TestWebServer.csproj", "samples/DinkToPdf.TestWebServer/"]

RUN dotnet restore "samples/DinkToPdf.TestWebServer/DinkToPdf.TestWebServer.csproj"

COPY . .
WORKDIR "/src/samples/DinkToPdf.TestWebServer"

RUN dotnet build "DinkToPdf.TestWebServer.csproj" --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DinkToPdf.TestWebServer.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DinkToPdf.TestWebServer.dll"]
