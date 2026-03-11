# Stage 1: Build, test, publish
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files first for layer caching on restore
COPY PRReviewAssistant.sln ./
COPY src/PRReviewAssistant.API/PRReviewAssistant.API.csproj src/PRReviewAssistant.API/
COPY tests/PRReviewAssistant.Tests/PRReviewAssistant.Tests.csproj tests/PRReviewAssistant.Tests/

RUN dotnet restore

# Copy all remaining source files
COPY . .

# Build in Release mode
RUN dotnet build --no-restore -c Release

# Run tests — a test failure stops the build
RUN dotnet test --no-build -c Release \
    --logger "console;verbosity=normal" \
    --results-directory /tmp/test-results

# Publish the API project
RUN dotnet publish src/PRReviewAssistant.API/PRReviewAssistant.API.csproj \
    --no-build -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 8080

# Install curl for HEALTHCHECK
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "PRReviewAssistant.API.dll"]
