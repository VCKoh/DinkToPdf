#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base

# Install libwkhtmltox dependencies
RUN apk add libgdiplus \
--no-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted
RUN apk add --no-cache \
  libstdc++ \
  libx11 \
  libc6-compat \
  libxrender \
  libxext \
  libssl1.1 \
  ca-certificates \
  fontconfig \
  freetype \
  ttf-dejavu \
  ttf-droid \
  ttf-freefont \
  ttf-liberation \
  ttf-ubuntu-font-family \
  # wkhtmltopdf \
        icu-libs \
        tzdata \
&& apk add --no-cache --virtual .build-deps \
  msttcorefonts-installer \
\
# Install microsoft fonts
&& update-ms-fonts \
&& fc-cache -f \
\
# Clean up when done
&& rm -rf /tmp/* \
&& apk del .build-deps

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Set TimeZone
ENV TZ=Asia/Singapore
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

# RUN cp /usr/local/bin/wkhtmlto* /usr/bin/
ENV LD_DEBUG=all
#RUN ln -s /usr/lib/libwkhtmltox.dll /usr/lib/libwkhtmltox.so
ENV PATH=/app;/app/runtimes/win-x64/native;/app/runtimes/linux-x64/native;/usr/lib;$PATH

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["samples/DinkToPdf.TestWebServer/DinkToPdf.TestWebServer.csproj", "samples/DinkToPdf.TestWebServer/"]

RUN dotnet restore "samples/DinkToPdf.TestWebServer/DinkToPdf.TestWebServer.csproj"

COPY . .
WORKDIR "/src/samples/DinkToPdf.TestWebServer"

RUN dotnet build "DinkToPdf.TestWebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DinkToPdf.TestWebServer.csproj" -c Release -o /app/publish

FROM surnet/alpine-wkhtmltopdf:3.10-0.12.5-full as wkhtmltopdf

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=wkhtmltopdf /bin/wkhtmltopdf .
COPY --from=wkhtmltopdf /bin/libwkhtmltox.so .

ENTRYPOINT ["dotnet", "DinkToPdf.TestWebServer.dll"]
